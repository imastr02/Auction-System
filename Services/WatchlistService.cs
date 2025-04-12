using Auction_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Auction_System.Services
{
	
	public class WatchlistService
	{
		private readonly ApplicationDbContext _context;

		public WatchlistService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task AddToWatchlistAsync(string userId, int itemId)
		{
			var watchlistItem = new WatchList
			{
				BuyerId = userId,
				ItemId = itemId
			};

			_context.WatchLists.Add(watchlistItem);
			await _context.SaveChangesAsync();
		}

		public async Task RemoveFromWatchlistAsync(string userId, int itemId)
		{
			var watchlistItem = await _context.WatchLists
				.FirstOrDefaultAsync(w => w.BuyerId == userId && w.ItemId == itemId);

			if (watchlistItem != null)
			{
				_context.WatchLists.Remove(watchlistItem);
				await _context.SaveChangesAsync();
			}
		}

		public async Task<bool> IsItemInWatchlistAsync(string userId, int itemId)
		{
			return await _context.WatchLists
				.AnyAsync(w => w.BuyerId == userId && w.ItemId == itemId);
		}
	}
}
