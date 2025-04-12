namespace Auction_System.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;

	namespace Auction_System.Models
	{
		public class Purchase
		{
			public int Id { get; set; } // Unique identifier for the purchase

			[Required]
			public string? BuyerId { get; set; } // Buyer who made the purchase
			public AppUser? Buyer { get; set; }

			[Required]
			public int ItemId { get; set; } // Item that was purchased
			public Item? Item { get; set; }

			[Required]
			public decimal PurchasePrice { get; set; } // Price at which the item was purchased

			[Required]
			public DateTime PurchaseDate { get; set; } = DateTime.UtcNow; // Date and time of purchase
		}
	}
}
