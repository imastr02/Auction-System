namespace Auction_System.Models
{
	public class MpesaResponse
	{
		public bool IsSuccessful { get; set; }
		public string TransactionId { get; set; }
		public string ErrorMessage { get; set; }
		public bool Simulated { get; set; }
	}
}