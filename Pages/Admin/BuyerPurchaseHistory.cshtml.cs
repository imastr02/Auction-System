using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Auction_System.Models;
using Auction_System.Services;

namespace Auction_System.Pages.Admin
{
	public class BuyerPurchaseHistoryModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		public BuyerPurchaseHistoryModel(ApplicationDbContext context)
		{
			_context = context;
		}

		public AppUser Buyer { get; set; }
		public List<Item> Purchases { get; set; }

		public async Task<IActionResult> OnGetAsync(string id)
		{
			if (string.IsNullOrEmpty(id)) return NotFound();

			Buyer = await _context.Users.FindAsync(id);
			if (Buyer == null) return NotFound();

			Purchases = await _context.Items
				.Where(i => i.WinnerId == id)
				.ToListAsync();

			return Page();
		}
	}
}
