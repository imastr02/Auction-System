using Auction_System.Models;
using Auction_System.Models.Auction_System.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Auction_System.Services
{
	public class PurchaseService
	{
		private readonly ApplicationDbContext _context;

		public PurchaseService(ApplicationDbContext context)
		{
			_context = context;
		}

		// Record a purchase
		public async Task RecordPurchaseAsync(string buyerId, int itemId, decimal purchasePrice)
		{
			var purchase = new Purchase
			{
				BuyerId = buyerId,
				ItemId = itemId,
				PurchasePrice = purchasePrice
			};

			_context.Purchases.Add(purchase);
			await _context.SaveChangesAsync();
		}

		// Check if a buyer has purchased a specific item
		public async Task<bool> HasPurchasedItemAsync(string buyerId, int itemId)
		{
			return await _context.Purchases
				.AnyAsync(p => p.BuyerId == buyerId && p.ItemId == itemId);
		}
	}
}