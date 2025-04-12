using Auction_System.Models;
using Auction_System.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Auction_System.Pages.Buyer
{
    public class WatchListModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
		private readonly WatchlistService _watchlistService;

		public WatchListModel(ApplicationDbContext context, UserManager<AppUser> userManager, WatchlistService watchlistService)
        {
            _context = context;
            _userManager = userManager;
			_watchlistService = watchlistService;
        }

        public IList<WatchList> WatchListItems { get; set; } = new List<WatchList>();

        public async Task OnGetAsync()
        {
            //Get current user
            var buyer = await _userManager.GetUserAsync(User);
            if (buyer != null)
            {
				//Fetch the buyer watchlist with related items
				WatchListItems = await _context.WatchLists
					.Include(w => w.Item)
					.ThenInclude(i => i.Category)
					.Include(w => w.Item)
					.ThenInclude(i => i.AuctionEvent)
					.Where(w => w.BuyerId == buyer.Id)
					.ToListAsync();
			}
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

		public async Task<IActionResult> OnPostRemoveFromWatchlistAsync(int itemId)
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return RedirectToPage("/Account/Login");
			}

			await _watchlistService.RemoveFromWatchlistAsync(user.Id, itemId);
			TempData["Message"] = "Item removed from watchlist!";
			return RedirectToPage();
		}
	}
}
