using System.ComponentModel.DataAnnotations;

namespace Auction_System.Models
{
	public class Item
	{
		public int Id { get; set; }
		public string? Title { get; set; }
		public string? Description { get; set; }

		[Range(0, double.MaxValue, ErrorMessage = "Positive Number")]
		[DataType(DataType.Currency)]
		public decimal? StartingPrice { get; set; }
		public decimal? EndingPrice { get; set; }
		public decimal? CurrentPrice { get; set; }
		public DateTime? StartingTime { get; set; }
		public DateTime? EndingTime {  get; set; }
		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public string? ImagePath { get; set; } // path to the uploaded image.

		//Foreign key for the Category
		public int CategoryId { get; set; } // Category of the item.
		public Category? Category { get; set; }

		public virtual AppUser? Seller { get; set; }
		public string? SellerId {  get; set; } //User who listed the Item

		public AuctionEvent? AuctionEvent { get; set; }
		public int AuctionEventId {  get; set; }

		public ICollection<Rating>? Ratings { get; set; }
		public ICollection<Bid>? Bids { get; set; }
		public ICollection<WatchList>? WatchLists { get; set; }
		
	}
}
