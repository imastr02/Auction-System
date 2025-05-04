using System.ComponentModel.DataAnnotations.Schema;

namespace Auction_System.Models
{
	// Models/MpesaTransaction.cs
	public class MpesaTransaction
	{
		public int Id { get; set; }
		public string MerchantRequestId { get; set; }
		public string CheckoutRequestId { get; set; }
		public string PhoneNumber { get; set; }
		public decimal Amount { get; set; }
		public DateTime TransactionDate { get; set; }
		public string Status { get; set; } // "Requested", "Completed", "Failed"

		[ForeignKey("Item")]
		public int ItemId { get; set; }
		public Item Item { get; set; }

		[ForeignKey("User")]
		public string UserId { get; set; }
		public AppUser User { get; set; }
	}
}
