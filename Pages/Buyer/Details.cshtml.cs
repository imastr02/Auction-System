using Auction_System.Enums;
using Auction_System.Models;
using Auction_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static Auction_System.Models.AuctionEvent;
namespace Auction_System.Pages.Buyer
{
	[Authorize]
	public class DetailsModel(ApplicationDbContext context, UserManager<AppUser> userManager, WatchlistService watchlistService) : PageModel
	{
		private readonly ApplicationDbContext _context = context;
		private readonly UserManager<AppUser> _userManager = userManager;
		private readonly WatchlistService _watchlistService = watchlistService;
		


		public Item Item { get; set; } = default!;
		public AppUser? Seller { get; set; }
		public AuctionStatus AuctionStatus { get; set; }

		[BindProperty]
		//public Feedback? Feedback { get; set; }

		public bool IsInWatchlist { get; set; }
		public bool IsWinner {  get; set; }
		public bool HasFeedback { get; set; }

		[BindProperty]
		public decimal BidAmount { get; set; }

		[BindProperty]
		public AuctionEvent AuctionEvent {  get; set; }
	
		public async Task<IActionResult> OnGetAsync(int? id, int itemId)
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
				.Include(i => i.Winner) // Include winner details
				.Include(i => i.Feedbacks) // Include feedback
					.ThenInclude(f => f.Buyer)
					.Include(i => i.Seller) // Include seller
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
			IsWinner = user != null && Item.WinnerId == user.Id;

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

		public async Task<IActionResult> OnPostAddToWatchlistAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return RedirectToPage("/Account/Login");
			}

			await _watchlistService.AddToWatchlistAsync(user.Id, id.Value);
			TempData["Message"] = "Item added to watchlist!";
			return RedirectToPage("./Details", new { id });
		}

		public async Task<IActionResult> OnPostRemoveFromWatchlistAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return RedirectToPage("/Account/Login");
			}

			await _watchlistService.RemoveFromWatchlistAsync(user.Id, id.Value);
			TempData["Message"] = "Item removed from watchlist!";
			return RedirectToPage("./Details", new { id });
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

			// Check if auction is active
			if (item.AuctionEvent?.Status != AuctionEventStatus.Active ||
				!item.AuctionEvent.IsActive)
			{
				ModelState.AddModelError("", "Bidding is closed.");
				return Page();
			}

		

			//TempData["Message"] = "Bid placed successfully!";
			return RedirectToPage("./Details", new { id = item.Id });
		}

		

	}
}