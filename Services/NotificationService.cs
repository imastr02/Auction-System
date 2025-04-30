using Auction_System.Models;
using Microsoft.EntityFrameworkCore;
using static Auction_System.Models.Notification;

namespace Auction_System.Services
{
	public class NotificationService
	{
		private readonly ApplicationDbContext _context;

		public NotificationService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task CreateAuctionEndNotificationsAsync(Item item)
		{
			// Get all unique bidders for this item
			var bidderIds = await _context.Bids
				.Where(b => b.ItemId == item.Id)
				.Select(b => b.BuyerId)
				.Distinct()
				.ToListAsync();

			// Create winner notification
			var winnerNotification = new Notification
			{
				UserId = item.WinnerId,
				ItemId = item.Id,
				Message = $"You won the auction for {item.Title}! Final price: {item.Bids.Max(b => b.Amount):C}",
				Type = NotificationType.AuctionWon
			};
			_context.Notifications.Add(winnerNotification);

			// Create notifications for other bidders
			foreach (var bidderId in bidderIds.Where(id => id != item.WinnerId))
			{
				var notification = new Notification
				{
					UserId = bidderId,
					ItemId = item.Id,
					Message = $"Auction ended for {item.Title}. Winning bid: {item.Bids.Max(b => b.Amount):C}",
					Type = NotificationType.AuctionEnded
				};
				_context.Notifications.Add(notification);
			}

			await _context.SaveChangesAsync();
		}
	}
}
