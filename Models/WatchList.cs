namespace Auction_System.Models
{
	public class WatchList
	{
		public int Id { get; set; }
		public string? UserId { get; set; } // User who added item to the watchlist
		public AppUser? User { get; set; } // Navigation Property

		public int ItemId { get; set; } // Item being watched
		public Item? Item { get; set; }
		public DateTime AddedTime { get; set; }
	}
}
