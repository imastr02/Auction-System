using System.ComponentModel.DataAnnotations;

namespace Auction_System.Models
{
	public class Bid
	{
		public int Id { get; set; }

		public int ItemId {  get; set; } // Item the bid is placed on
		public Item? Item { get; set; } // Navigation property

		[DataType(DataType.Currency)]
		public decimal? Amount { get; set; }
		public DateTime BidTime { get; set; }

		public string? BuyerId {  get; set; } // User who made the bid
		public virtual AppUser? Buyer { get; set; } // Navigation property
	}
}
