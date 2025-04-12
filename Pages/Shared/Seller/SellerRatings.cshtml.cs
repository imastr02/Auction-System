using Auction_System.Models;
using Auction_System.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Auction_System.Pages.Shared.Seller
{
	public class SellerRatingsModel(ApplicationDbContext context) : PageModel
	{
		private readonly ApplicationDbContext _context = context;

		public AppUser? Seller { get; set; }
		public List<Item> SoldItems { get; set; } = new();
		public double AverageRating { get; set; }

		public async Task<IActionResult> OnGetAsync(string id)
		{
			// Get seller info
			Seller = await _context.Users
				.FirstOrDefaultAsync(u => u.Id == id);

			if (Seller == null)
			{
				return NotFound();
			}

			// Get all items sold by this seller with feedback
			SoldItems = await _context.Items
				.Include(i => i.Feedbacks)
				.Include(i => i.Winner)
				.Include(i => i.Bids)
				.Where(i => i.SellerId == id && i.WinnerId != null)
				.OrderByDescending(i => i.AuctionDate)
				.ToListAsync();

			// Calculate average rating
			var allRatings = SoldItems
				.SelectMany(i => i.Feedbacks)
				.Select(f => f.Rating)
				.ToList();

			AverageRating = allRatings.Any() ? allRatings.Average() : 0;

			return Page();
		}
	}
}