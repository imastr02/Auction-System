using System.ComponentModel.DataAnnotations;

namespace Auction_System.Models
{
	public class ItemFeedback
	{
		public int Id { get; set; }
		[Required]
		public string? BuyerId { get; set; }
		public AppUser? Buyer {  get; set; }

		[Required]
		public int ItemId {  get; set; }
		public Item? Item { get; set; }

		[Required]
		[MaxLength(500)]
		public string? Feedback {  get; set; }
		public DateTime FeedbackDate { get; set; } = DateTime.Now;
	}
}
