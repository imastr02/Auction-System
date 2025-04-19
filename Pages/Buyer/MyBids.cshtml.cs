using Auction_System.Models;
using Auction_System.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Auction_System.Pages.Buyer
{
    public class MyBidsModel : PageModel
    {
		private readonly UserManager<AppUser> _userManager;
		private readonly ApplicationDbContext _context;

		public MyBidsModel(UserManager<AppUser> userManager, ApplicationDbContext context)
		{
			_userManager = userManager;
			_context = context;
		}

		//public AppUser? CurrentUser { get; set; }
		public List<Bid> UserBids { get; set; } = new List<Bid>();
		//public List<Item> WonItems { get; set; } = new List<Item>();
		public async Task<IActionResult> OnGetAsync()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null) return RedirectToPage("/Account/Login");


			UserBids = await _context.Bids
				.Where(b => b.BuyerId == user.Id)
				.Include(b => b.Item)
					.ThenInclude(i => i.Category)
				.Include(b => b.Item)
					.ThenInclude(i => i.Seller)
				.Include(b => b.Item)
					.ThenInclude(i => i.AuctionEvent)
				.OrderByDescending(b => b.BidTime)
				.ToListAsync();


			return Page();
		}
    }
}
