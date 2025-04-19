using Auction_System.Models;
using Auction_System.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Auction_System.Pages.Buyer
{
    public class AuctionWonModel : PageModel
    {
		private readonly UserManager<AppUser> _userManager;
		private readonly ApplicationDbContext _context;

		public AuctionWonModel(UserManager<AppUser> userManager, ApplicationDbContext context)
		{
			_userManager = userManager;
			_context = context;
		}
		public AppUser CurrentUser { get; set; }
		public AppUser Seller {  get; set; }
		public List<Item> WonItems { get; set; } = new List<Item>();
		public async Task<IActionResult> OnGetAsync()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null) return RedirectToPage("/Account/Login");

			CurrentUser = await _context.Users
					  .Include(u => u.Bids)
						  .ThenInclude(b => b.Item)
					  .Include(u => u.ItemsWon)
					  .ThenInclude(i => i.AuctionEvent)
					  .Include(u => u.ItemsWon)
					  .ThenInclude(i => i.Category)
					  .Include(u => u.ItemsWon)
					  .ThenInclude(i => i.Seller)
					  .FirstOrDefaultAsync(u => u.Id == user.Id);

			WonItems = CurrentUser!.ItemsWon
		  .OrderByDescending(i => i.AuctionEvent?.EndTime)
		  .ToList();

			return Page();
		}
    }
}
