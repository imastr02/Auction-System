using Auction_System.Models;
using Auction_System.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Auction_System.Pages.Buyer
{
	public class AddToWatchListModel : PageModel
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<AppUser> _userManager;

		public AddToWatchListModel(ApplicationDbContext context, UserManager<AppUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		[BindProperty(SupportsGet = true)]
		public int ItemId { get; set; }
		public async Task<IActionResult> OnGetAsync()
		{
			//Get the current user (buyer)
			var buyer = await _userManager.GetUserAsync(User);
			if (buyer == null)
			{
				return RedirectToPage("/Account/Login");
			}

			//check if item is already in the watchList
			var existingWatchListItem = await _context.WatchLists.FirstOrDefaultAsync(w => w.BuyerId == buyer.Id && w.ItemId == ItemId);

			if (existingWatchListItem != null)
			{
				//Item already in the watchList
				TempData["Message"] = "Item added to WatchList";
				return RedirectToPage("/Buyer/ViewItems");
			}

			//Add item to the WatchList
			var watchListItem = new WatchList
			{
				BuyerId = buyer.Id,
				ItemId = ItemId,
				AddedTime = DateTime.Now
			};

			_context.WatchLists.Add(watchListItem);
			await _context.SaveChangesAsync();

			return RedirectToPage("/Buyer/ViewItems");
		}
	}
}
