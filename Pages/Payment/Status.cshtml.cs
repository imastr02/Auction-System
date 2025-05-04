using Auction_System.Models;
using Auction_System.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Auction_System.Pages.Payment
{
	public class StatusModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		[BindProperty(SupportsGet = true)]
		public int ItemId { get; set; }

		public MpesaResponse PaymentResponse { get; set; }
		public Item Item { get; set; }

		public StatusModel(ApplicationDbContext context)
		{
			_context = context;
		}

		public IActionResult OnGet()
		{
			try
			{
				// Get payment response
				if (TempData["PaymentResponse"] is not string responseJson)
				{
					//_logger.LogWarning("Missing payment response data");
					return RedirectToPage("/Error");
				}

				PaymentResponse = JsonConvert.DeserializeObject<MpesaResponse>(responseJson);

				// Get item ID
				if (!int.TryParse(TempData["PaymentItemId"]?.ToString(), out int itemId))
				{
					//_logger.LogWarning("Invalid item ID in payment response");
					return RedirectToPage("/Error");
				}

				// Load item details
				Item = _context.Items
					.Include(i => i.Seller)
					.FirstOrDefault(i => i.Id == itemId);

				if (Item == null)
				{
					//_logger.LogWarning("Item {ItemId} not found", itemId);
					return RedirectToPage("/Error");
				}

				return Page();
			}
			catch (Exception ex)
			{
				//_logger.LogError(ex, "Error loading status page");
				return RedirectToPage("/Error");
			}
		}
	}
}