using Auction_System.Models;
using Auction_System.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;



namespace Auction_System.Pages.Buyer
	{
		public class RemoveFromWatchListModel : PageModel
		{
			private readonly ApplicationDbContext _context;
			private readonly UserManager<AppUser> _userManager;

			public RemoveFromWatchListModel(ApplicationDbContext context, UserManager<AppUser> userManager)
			{
				_context = context;
				_userManager = userManager;
			}

			[BindProperty(SupportsGet = true)]
			public int ItemId { get; set; }

			public async Task<IActionResult> OnGetAsync()
			{
				// Get the current user (buyer)
				var buyer = await _userManager.GetUserAsync(User);
				if (buyer == null)
				{
					return RedirectToPage("/Account/Login"); // Redirect to login if the user is not authenticated
				}

				// Find the watchlist item to remove
				var watchlistItem = await _context.WatchLists
					.FirstOrDefaultAsync(w => w.BuyerId == buyer.Id && w.ItemId == ItemId);

				if (watchlistItem != null)
				{
					// Remove the item from the watchlist
					_context.WatchLists.Remove(watchlistItem);
					await _context.SaveChangesAsync();

					//TempData["Message"] = "Item removed from your watchlist successfully!";
				}
				//else
				//{
				//	TempData["Message"] = "Item not found in your watchlist.";
				//}

				return RedirectToPage("/Buyer/ViewItems");
			}
		}
	}


