using Auction_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Auction_System.Services
{
	public class RatingService
	{
		private readonly ApplicationDbContext _context;

		public RatingService(ApplicationDbContext context)
		{
			_context = context;
		}


		public async Task AddRatingAsync(string buyerId, string sellerId, int rating)
		{
			if (buyerId == sellerId)
			{
				throw new InvalidOperationException("You cannot rate yourself.");
			}
			var sellerRating = new Rating
			{
				BuyerId = buyerId,
				SellerId = sellerId,
				Score = rating
			};

			_context.Ratings.Add(sellerRating);
			await _context.SaveChangesAsync();
		}

		public async Task<double> GetSellerRatingAsync(string sellerId)
		{
			var ratings = await _context.Ratings
				.Where(r => r.SellerId == sellerId)
				.ToListAsync();

			if (ratings.Any())
			{
				return ratings.Average(r => r.Score);
			}

			return 0; // Default rating if no ratings exist
		}
	}
}
