using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Auction_System.Models;
using System.Threading.Tasks;
using Auction_System.Services;
using Microsoft.EntityFrameworkCore;

namespace Auction_System.Pages.Payment
{
	public class CheckoutModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		[BindProperty(SupportsGet = true, Name = "itemId")]
		public int ItemId { get; set; }

		public Item Item { get; set; }

		[BindProperty]
		public string PhoneNumber { get; set; }

		public CheckoutModel(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> OnGetAsync()
		{
			Item = await _context.Items
				.Include(i => i.Seller)
				.Include(i => i.AuctionEvent)
				.FirstOrDefaultAsync(i => i.Id == ItemId);

			if (Item == null || Item.IsSold || Item.AuctionEvent.EndTime < DateTime.UtcNow)
			{
				return RedirectToPage("../Buyer/Inbox");
			}

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			var item = await _context.Items.FindAsync(ItemId);

			if (item == null || item.IsSold)
			{
				return RedirectToPage("../Buyer/Inbox");
			}

			// Simulate payment processing
			item.IsSold = true;
			item.SoldAt = DateTime.UtcNow;
			await _context.SaveChangesAsync();

			TempData["PhoneNumber"] = PhoneNumber;
			return RedirectToPage("Success", new { itemId = ItemId });
		}
	}
}