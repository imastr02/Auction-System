using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auction_System.Models
{
	public class AuctionEvent
	{
		public int Id { get; set; }

		
		[StringLength(200)]
		public string? Name { get; set; }

		[Required]
		[Display(Name = "Start Time")]
		public DateTime StartTime { get; set; }

		[Required]
		[Display(Name = "End Time")]
		public DateTime EndTime { get; set; }

		[Required]
		public DateTime AuctionDate { get; set; }

		[StringLength(1000)]
		public string? Description { get; set; }

		[Required]
		public AuctionEventStatus Status { get; set; } = AuctionEventStatus.Scheduled;

		// Relationships
		public string? SellerId { get; set; }
		public AppUser? Seller { get; set; }

		public List<Item> Items { get; set; } = new List<Item>();

		// Calculated property (not mapped to database)
		[NotMapped]
		public bool IsActive =>
		  Status == AuctionEventStatus.Active &&
		  DateTime.Now >= StartTime &&
		  DateTime.Now <= EndTime;

		public void EndAuctionManually()
		{
			if (Status == AuctionEventStatus.Active || Status == AuctionEventStatus.Scheduled)
			{
				Status = AuctionEventStatus.Completed;
				EndTime = DateTime.Now; // Optional: Set EndTime to now
			}
		}
		public enum AuctionEventStatus
		{
			Scheduled,
			Active,
			Completed,
			Canceled
		}
	}
}