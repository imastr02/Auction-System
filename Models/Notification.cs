using System.ComponentModel.DataAnnotations.Schema;

namespace Auction_System.Models
{
	public class Notification
	{
		public int Id { get; set; }
		public string? UserId { get; set; }
		public int ItemId { get; set; }
		public string? Message { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public bool IsRead { get; set; } = false;
		public NotificationType Type { get; set; }

		[ForeignKey("UserId")]
		public AppUser User { get; set; }

		[ForeignKey("ItemId")]
		public Item Item { get; set; }
	}

	public enum NotificationType
	{
		AuctionWon,
		AuctionEnded,
		Outbid,
		PaymentReminder
	}

}