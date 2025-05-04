using Auction_System.Enums;
using System.ComponentModel.DataAnnotations;

namespace Auction_System.Models
{
	public class Item
	{
		public int Id { get; set; }

		[Required]
		public string? Title { get; set; }
		public string? Description { get; set; }

		[Range(0, double.MaxValue, ErrorMessage = "Positive Number")]
		[DataType(DataType.Currency)]
		public decimal StartingPrice { get; set; }
		public decimal EndingPrice { get; set; }
		public decimal CurrentPrice { get; set; }

		[Required]
		public DateTime StartingTime { get; set; }

		[Required]
		public DateTime EndingTime {  get; set; }

		//[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime AuctionDate { get; set; }

		public bool IsSold { get; set; }
		public DateTime? SoldAt { get; set; } // New property
		public bool IsPaid { get; set; } // New property
		public DateTime? PaidAt { get; set; } // New property
		public string? WinnerId {  get; set; }
		public AppUser? Winner {  get; set; }
		public decimal SoldPrice {  get; set; }

	

		public Bid? GetWinningBid()
		{
			return Bids?
				.OrderByDescending(b => b.Amount) // Order by bid amount (highest first)
				.FirstOrDefault(); // Get the highest bid
		}

		[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public string? ImagePath { get; set; } // path to the uploaded image.

		//Foreign key for the Category
		public int CategoryId { get; set; } // Category of the item.
		public Category? Category { get; set; }

		public virtual AppUser? Seller { get; set; }
		public string? SellerId {  get; set; } //User who listed the Item

		public AuctionEvent? AuctionEvent { get; set; }
		public int AuctionEventId {  get; set; }

		public ICollection<Rating>? Ratings { get; set; }
		public ICollection<Feedback>? Feedbacks { get; set; }
		public ICollection<Bid> ?Bids { get; set; }
		public ICollection<WatchList>? WatchLists { get; set; }
		



	}
}
