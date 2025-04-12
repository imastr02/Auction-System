using System.ComponentModel.DataAnnotations;

namespace Auction_System.Models
{
	public class Rating
	{
		public int Id { get; set; }

		[Required]
		[Range(1,5, ErrorMessage = "Rating must be from 1 to 5")]
		public int Score { get; set; } // rating score like 1 to 5 stars

		[MaxLength(500)]
		public string? Comment { get; set; }

		[Required]
		public DateTime RatingTime { get; set; } = DateTime.Now;

		public string? SellerId { get; set; } // User being rated 
		public virtual AppUser? Seller { get; set; }

		public string? BuyerId { get; set; } // User giving the rating 
		public virtual AppUser? Buyer { get; set; }
		
		public int ItemId { get; set; } // Item the rating is being associated with
		public virtual Item? Item { get; set; }
	}

}
