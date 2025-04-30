using Auction_System.Enums;
using Auction_System.Models;
using Auction_System.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using static Auction_System.Models.AuctionEvent;


namespace Auction_System.Pages.Buyer
{
    public class BiddingProcessModel(ApplicationDbContext context, UserManager<AppUser> userManager, WatchlistService watchlistService, EmailSender emailSender) : PageModel
    {
		private readonly ApplicationDbContext _context = context;
		private readonly UserManager<AppUser> _userManager = userManager;
		private readonly WatchlistService _watchlistService = watchlistService;
		private readonly EmailSender _emailSender = emailSender;
		private Item urlHelper;

		public Item Item { get; set; } = default!;
		public AppUser? Seller { get; set; }
		//public bool IsAuctionActive { get; set; }
		public bool IsInWatchlist { get; set; }


		// Add this property
		public bool HasFeedback { get; set; }
		public bool IsWinner { get; set; }

		//public AuctionStatus AuctionStatus {  get; set; }

		[BindProperty]
		public decimal BidAmount { get; set; }


		[BindProperty]
		public AuctionEvent AuctionEvent { get; set; }
		//public string message { get; set; }
		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			// Load the item with related data
			Item = await _context.Items
				.Include(i => i.Category) // Include category
				.Include(i => i.AuctionEvent) // Include auction event
				.Include(i => i.Bids!) // Include bids and their buyers
					.ThenInclude(b => b.Buyer)
				.Include(i => i.Seller) // Include seller
				.Include(i => i.Winner) // Include the Winner (AppUser)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (Item == null)
			{
				return NotFound();
			}



			// Check if Seller is null
			if (Item.Seller == null)
			{
				Console.WriteLine("Seller is null for this item.");
			}

			// Fetch the seller's profile
			Seller = Item.Seller;





			// Check if the item is in the user's watchlist
			var user = await _userManager.GetUserAsync(User);
			if (user != null)
			{
				IsInWatchlist = await _watchlistService.IsItemInWatchlistAsync(user.Id, Item.Id);
			}

			// Add this check for feedback
			if (User.Identity?.IsAuthenticated == true && Item?.WinnerId != null)
			{
				var currentUser = await _userManager.GetUserAsync(User);
				IsWinner = currentUser != null && Item.WinnerId == currentUser.Id;
				HasFeedback = await _context.Feedbacks
					.AnyAsync(f => f.ItemId == Item.Id && f.BuyerId == currentUser.Id);
			}
			

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			// Load the item with related data
			var item = await _context.Items
				.Include(i => i.AuctionEvent) // Include auction event
				.Include(i => i.Bids!) // Include bids
				.FirstOrDefaultAsync(m => m.Id == id);

			if (item == null || item.AuctionEvent == null)
			{
				return NotFound();
			}

			// Get the current user
			var buyer = await _userManager.GetUserAsync(User);
			if (buyer == null)
			{
				return RedirectToPage("/Account/Login");
			}
			// Check if the auction is active
			// Check if auction is active
			if (item.AuctionEvent?.Status != AuctionEventStatus.Active ||
				!item.AuctionEvent.IsActive)
			{
				ModelState.AddModelError("", "Bidding is closed.");
				return Page();
			}

			// Validate bid amount
			var highestBid = item.Bids?.OrderByDescending(b => b.Amount).FirstOrDefault();
			decimal currentHighestBid = highestBid?.Amount ?? 0;

			// Check if the bid is greater than or equal to the starting price
			if (BidAmount < item.StartingPrice)
			{
				ModelState.AddModelError("BidAmount", $"Your bid must be greater than or equal to the starting price of {item.StartingPrice:C}.");
				//AuctionStatus = item.GetAuctionStatus();
				Item = item; // Reload the item for the page
				return Page();
			}

			// Check if the bid is higher than the current highest bid
			if (highestBid != null && BidAmount <= highestBid.Amount)
			{
				ModelState.AddModelError("BidAmount", "Your bid must be higher than the current highest bid.");
				Item = item; // Reload the item for the page
				return Page();
			}

			// Add the bid
			var bid = new Bid
			{
				ItemId = item.Id,
				BuyerId = buyer.Id,
				Amount = BidAmount,
				BidTime = DateTime.Now
			};

			_context.Bids.Add(bid);
			await _context.SaveChangesAsync();

			TempData["Message"] = "Bid placed successfully!";
			return RedirectToPage("./Details", new { id = item.Id });
		}


		public async Task<IActionResult> OnPostRemoveBidAsync(int bidId)
		{
			// Get the bid to remove
			var bid = await _context.Bids
				.Include(b => b.Item)
					.ThenInclude(i => i.AuctionEvent)
				.FirstOrDefaultAsync(b => b.Id == bidId);

			if (bid == null || bid.Item == null || bid.Item.AuctionEvent == null)
			{
				return NotFound();
			}

			// Validate auction is still active
			if (bid.Item.AuctionEvent.Status != AuctionEventStatus.Active ||
				DateTime.Now > bid.Item.AuctionEvent.EndTime)
			{
				ModelState.AddModelError("", "Cannot remove bids after the auction has ended.");
				return Page();
			}

			// Verify current user is the bid owner
			var user = await _userManager.GetUserAsync(User);
			if (user == null || bid.BuyerId != user.Id)
			{
				return Forbid();
			}

			// Remove the bid
			_context.Bids.Remove(bid);
			await _context.SaveChangesAsync();

			TempData["Message"] = "Bid removed successfully!";
			return RedirectToPage("./BiddingProcess", new { id = bid.ItemId });
		}

		public async Task<IActionResult> OnPostEndAuctionAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			// Load the item with related data
			var item = await _context.Items
				.Include(i => i.AuctionEvent) // Include auction event
				.Include(i => i.Bids!) // Include bids
					.ThenInclude(b => b.Buyer) // Include buyer details
					.Include(i => i.Seller)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (item == null || item.AuctionEvent == null)
			{
				return NotFound();
			}

			// Get the current user
			var seller = await _userManager.GetUserAsync(User);
			if (seller == null || seller.Id != item.SellerId)
			{
				return Forbid(); // Only the seller can end the auction
			}

			// End the auction manually
			item.AuctionEvent.EndAuctionManually(); // Use the method from AuctionEvent model

			// Determine the winner
			if (item.Bids != null && item.Bids.Any())
			{
				// If only one bid exists, the bidder is the winner
				var winningBid = item.Bids.Count == 1
					? item.Bids.First()
					: item.Bids.OrderByDescending(b => b.Amount).First();

				item.WinnerId = winningBid.BuyerId;

				// Get the winner and send email notification
				var winner = await _userManager.FindByIdAsync(winningBid.BuyerId);
				if (winner != null)
				{
					await SendMailAsync(winner.UserName, winner.Email, item);
				}
				//if (winningBid != null)
				//{

				//	item.WinnerId = winningBid.BuyerId;
				//	var winner = await _userManager.FindByIdAsync(winningBid.BuyerId);
				//	var name = winner?.UserName;
				//	var email = winner?.Email;

				//	await SendMailAsync(name!, email!, item);

				//}
			}

			

			_context.Items.Update(item);
			await _context.SaveChangesAsync();

			// Create notifications
			var notificationService = new NotificationService(_context);
			await notificationService.CreateAuctionEndNotificationsAsync(item);

			TempData["Message"] = "Auction ended successfully!";
			return RedirectToPage("./Details", new { id = item.Id });
		}

		public async Task<bool> SendMailAsync(string name, string email,Item item)
		{
			// Get highest bid
			var highestBid = item.Bids?.OrderByDescending(b => b.Amount).FirstOrDefault();

			// Generate item link
			//var itemLink = urlHelper.Page(
			//	"/Items/Details",
			//	new { id = item.Id },
			//	protocol: HttpContext.Request.Scheme
			//);

			try
			{
				if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(name))
				{
					throw new ArgumentException("Name and email cannot be null or empty");
				}

				var fromAddress = new MailAddress("ahmedabeidahmed2002@gmail.com", "Auction System");
				var toAddress = new MailAddress(email);

				using var message = new MailMessage(fromAddress, toAddress)
				{
					Subject = "Auction Winner Notification",
					Body = $@"<html>
        <body style='font-family: Arial, sans-serif;'>
            <h1 style='color: #2c3e50;'>Congratulations, {WebUtility.HtmlEncode(name)}! 🎉</h1>
            
            <div style='background-color: #f8f9fa; padding: 20px; border-radius: 5px;'>
                <h2 style='color: #27ae60;'>You Won: {WebUtility.HtmlEncode(item.Title)}</h2>
                
                <table style='width: 100%; border-collapse: collapse; margin-top: 15px;'>
                    <tr>
                        <td style='padding: 8px; border: 1px solid #ddd; width: 30%;'><strong>Item Description:</strong></td>
                        <td style='padding: 8px; border: 1px solid #ddd;'>{WebUtility.HtmlEncode(item.Description)}</td>
                    </tr>
                    <tr>
                        <td style='padding: 8px; border: 1px solid #ddd;'><strong>Category:</strong></td>
                        <td style='padding: 8px; border: 1px solid #ddd;'>{WebUtility.HtmlEncode(item.Category?.CategoryName)}</td>
                    </tr>
                    <tr>
                        <td style='padding: 8px; border: 1px solid #ddd;'><strong>Final Price:</strong></td>
                        <td style='padding: 8px; border: 1px solid #ddd;'>{highestBid.Amount?.ToString("C")}</td>
                    </tr>
                    <tr>
                        <td style='padding: 8px; border: 1px solid #ddd;'><strong>Auction Ended:</strong></td>
                        <td style='padding: 8px; border: 1px solid #ddd;'>{item.AuctionEvent.EndTime.ToString("f")}</td>
                    </tr>
                </table>

              
            </div>

            <div style='margin-top: 25px;'>
                <h3>Next Steps:</h3>
                <ol>
                    <li>Contact the seller at {item.Seller.Email}</li>
                    <li>Arrange payment and delivery</li>
                    <li>Leave feedback after receiving the item</li>
                </ol>
            </div>

            <footer style='margin-top: 30px; color: #7f8c8d;'>
                <p>Best regards,<br>The Auction Team</p>
            </footer>
        </body>
    </html>",
					IsBodyHtml = true
				};

				using var smtpClient = new SmtpClient("smtp.gmail.com", 587)
				{
					EnableSsl = true,
					UseDefaultCredentials = false,
					Credentials = new NetworkCredential(
						"ahmedabeidahmed2002@gmail.com",
						"ewlv efbw ddoz qcwn"
					),
					DeliveryMethod = SmtpDeliveryMethod.Network
				};

				await smtpClient.SendMailAsync(message);
				return true;
			}
			catch (Exception ex)
			{
				// Log the exception (you should implement proper logging)
				Console.WriteLine($"Error sending email: {ex}");
				return false;
			}
		}
	}
}
