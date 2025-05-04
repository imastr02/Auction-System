using System.ComponentModel.DataAnnotations;

namespace Auction_System.Models
{
	public class MpesaSettings
	{
		[Required]
		public bool TestMode { get; set; }

		[Range(0, 1)]
		public double TestFailureRate { get; set; } = 0.2;

		[Range(0, 10000)]
		public int SimulatedDelayMs { get; set; } = 1500;

		[Required]
		public string ConsumerKey { get; set; }

		[Required]
		public string ConsumerSecret { get; set; }

		[Required]
		public string BusinessShortCode { get; set; }

		[Required]
		public string PassKey { get; set; }

		[Required]
		public string CallbackUrl { get; set; }

		[Required]
		public string AuthUrl { get; set; }

		[Required]
		public string STKPushUrl { get; set; }
	}
}