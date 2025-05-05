using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Auction_System.Models;
using Auction_System.Services;

namespace Auction_System.Pages.Admin
{
	public class ViewSellerItemsModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		public ViewSellerItemsModel(ApplicationDbContext context)
		{
			_context = context;
		}

		public List<Item> Items { get; set; } = new();
		public string SellerEmail { get; set; }
		public string SellerId { get; set; }

		public async Task<IActionResult> OnGetAsync(string sellerId)
		{
			if (string.IsNullOrEmpty(sellerId))
			{
				return RedirectToPage("/Index");
			}

			SellerId = sellerId;

			var seller = await _context.Users.FirstOrDefaultAsync(u => u.Id == sellerId);
			if (seller == null)
			{
				return NotFound();
			}

			SellerEmail = seller.Email;

			Items = await _context.Items
				.Where(i => i.SellerId == sellerId)
				.ToListAsync();

			return Page();
		}

		public async Task<IActionResult> OnPostDeleteAsync(int id, string sellerId)
		{
			var item = await _context.Items.FindAsync(id);
			if (item != null)
			{
				_context.Items.Remove(item);
				await _context.SaveChangesAsync();
			}

			return RedirectToPage(new { sellerId });
		}
	}
}
