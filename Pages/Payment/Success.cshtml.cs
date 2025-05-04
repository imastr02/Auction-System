using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Auction_System.Models;
using System.Threading.Tasks;
using Auction_System.Services;
using Microsoft.EntityFrameworkCore;

namespace Auction_System.Pages.Payment
{
	public class SuccessModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		[BindProperty(SupportsGet = true)]
		public int ItemId { get; set; }

		public Item Item { get; set; }
		public string PhoneNumber { get; set; }

		public SuccessModel(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> OnGetAsync()
		{
			PhoneNumber = TempData["PhoneNumber"]?.ToString();

			Item = await _context.Items
				.Include(i => i.Seller)
				.FirstOrDefaultAsync(i => i.Id == ItemId);

			if (Item == null)
			{
				return RedirectToPage("../Buyer/Inbox");
			}

			return Page();
		}
	}
}