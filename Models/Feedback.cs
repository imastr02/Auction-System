using System.ComponentModel.DataAnnotations;

namespace Auction_System.Models
{
	public class Feedback
	{
		public int Id { get; set; }

		[Required]
		[Range(1,5)]
		public int Rating { get; set; }

		[MaxLength(500)]
		public string? Comment { get; set; }

		[Required]
		public DateTime FeedbackDate { get; set; } = DateTime.Now;

		// Foreign key to the item
		public int ItemId { get; set; }
		public Item? Item { get; set; }

		// Foreign key to the buyer (winner)
		public string? BuyerId { get; set; }
		public AppUser? Buyer { get; set; }

		// Foreign key to the seller
		public string? SellerId { get; set; }
		public AppUser? Seller { get; set; }
	}
}
