using System.ComponentModel.DataAnnotations;

namespace Auction_System.Models
{
	public class ItemDto
	{
		[Required, MaxLength(100)]
		public string? Title {  get; set; }

		[MaxLength(100)]
		public string? Description { get; set; }

		
		public decimal StartingPrice { get; set; }

	
		public DateTime? StartingTime { get; set; }

		public DateTime? EndingTime { get; set;}

		public int CategoryId {  get; set; }
		public int AuctionEventId {  get; set; }
		public IFormFile? ImageFile { get; set; }
	}
}
