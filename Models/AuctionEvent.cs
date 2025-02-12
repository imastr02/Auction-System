namespace Auction_System.Models
{
	public class AuctionEvent
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public Item? Item { get; set; }
		public int ItemId { get; set; }
		public DateTime StartingTime { get; set; }
		public DateTime EndingTime { get; set; }

		public ICollection<Item>? Items { get; set; }	

	}
}
