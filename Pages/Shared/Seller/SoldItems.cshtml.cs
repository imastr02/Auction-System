//using Auction_System.Models;
//using Auction_System.Services;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.EntityFrameworkCore;

//namespace Auction_System.Pages.Seller
//{
//	public class SoldItemsModel : PageModel
//	{
//		private readonly ApplicationDbContext _context;
//		private readonly UserManager<AppUser> _userManager;

//		public List<Item> SoldItems { get; set; } = new();

//		public SoldItemsModel(
//			ApplicationDbContext context,
//			UserManager<AppUser> userManager)
//		{
//			_context = context;
//			_userManager = userManager;
//		}

//		public async Task OnGetAsync()
//		{
//			var user = await _userManager.GetUserAsync(User);
//			if (user == null) return;

//			// Fetch sold items belonging to the current seller
//			SoldItems = await _context.Items
//				.Include(i => i.Winner) // Buyer details
//				.Include(i => i.AuctionEvent)
//				.Where(i => i.SellerId == user.Id && i.IsSold)
//				.OrderByDescending(i => i.AuctionEvent.EndTime)
//				.AsNoTracking()
//				.ToListAsync();
//		}

//		public async Task<IActionResult> OnPostMarkAsDeliveredAsync(int itemId)
//		{
//			var user = await _userManager.GetUserAsync(User);
//			if (user == null) return RedirectToPage("/Login");

//			var item = await _context.Items
//				.FirstOrDefaultAsync(i => i.Id == itemId && i.SellerId == user.Id);

//			if (item != null)
//			{
//				item.IsDelivered = true;
//				item.DeliveryDate = DateTime.UtcNow;
//				await _context.SaveChangesAsync();
//			}

//			return RedirectToPage();
//		}
//	}
//}

using Auction_System.Models;
using Auction_System.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Auction_System.Pages.Seller
{
	public class SoldItemsModel : PageModel
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<AppUser> _userManager;

		public List<Item> SoldItems { get; set; } = new();

		public SoldItemsModel(
			ApplicationDbContext context,
			UserManager<AppUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task OnGetAsync(string sellerId = null)
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null) return;

			// Determine target user ID
			string targetUserId = user.Id;

			if (!string.IsNullOrEmpty(sellerId))
			{
				// Validate admin access
				if (User.IsInRole("Admin"))
				{
					targetUserId = sellerId;
				}
			}

			SoldItems = await _context.Items
				.Include(i => i.Winner)
				.Include(i => i.AuctionEvent)
				.Where(i => i.SellerId == targetUserId && i.IsSold)
				.OrderByDescending(i => i.AuctionEvent.EndTime)
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<IActionResult> OnPostMarkAsDeliveredAsync(int itemId)
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null) return RedirectToPage("/Login");

			// Only allow seller to mark delivery
			var item = await _context.Items
				.FirstOrDefaultAsync(i => i.Id == itemId && i.SellerId == user.Id);

			if (item == null) return NotFound();

			if (!item.IsDelivered)
			{
				item.IsDelivered = true;
				item.DeliveryDate = DateTime.UtcNow;
				await _context.SaveChangesAsync();
			}

			return RedirectToPage();
		}
	}
}