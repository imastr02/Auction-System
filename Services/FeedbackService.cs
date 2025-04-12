using Auction_System.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction_System.Services
{
	public class FeedbackService
	{
		private readonly ApplicationDbContext _context;
		private readonly PurchaseService _purchaseService;

		public FeedbackService(ApplicationDbContext context, PurchaseService purchaseService)
		{
			_context = context;
			_purchaseService = purchaseService;
		}

		public async Task AddFeedbackAsync(string buyerId, int itemId, string feedback)
		{
			// Check if the buyer has purchased the item
			if (!await _purchaseService.HasPurchasedItemAsync(buyerId, itemId))
			{
				throw new InvalidOperationException("You can only leave feedback for items you have purchased.");
			}

			var itemFeedback = new ItemFeedback
			{
				BuyerId = buyerId,
				ItemId = itemId,
				Feedback = feedback
			};

			_context.ItemFeedbacks.Add(itemFeedback);
			await _context.SaveChangesAsync();
		}

		public async Task<List<ItemFeedback>> GetFeedbacksForItemAsync(int itemId)
		{
			return await _context.ItemFeedbacks
				.Include(f => f.Buyer)
				.Where(f => f.ItemId == itemId)
				.ToListAsync();
		}

		public async Task<bool> CanLeaveFeedbackAsync(string buyerId, int itemId)
		{
			return await _purchaseService.HasPurchasedItemAsync(buyerId, itemId);
		}
	}
}