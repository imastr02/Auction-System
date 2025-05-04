// Models/Dtos/MpesaCallbackDtos.cs
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Auction_System.Models.Dtos
{
	public class MpesaCallbackResponse
	{
		[JsonProperty("Body")]
		public MpesaCallbackBody Body { get; set; }
	}

	public class MpesaCallbackBody
	{
		[JsonProperty("stkCallback")]
		public StkCallback StkCallback { get; set; }
	}

	public class StkCallback
	{
		[JsonProperty("MerchantRequestID")]
		public string MerchantRequestID { get; set; }

		[JsonProperty("CheckoutRequestID")]
		public string CheckoutRequestID { get; set; }

		[JsonProperty("ResultCode")]
		public int ResultCode { get; set; }

		[JsonProperty("ResultDesc")]
		public string ResultDesc { get; set; }

		[JsonProperty("CallbackMetadata")]
		public CallbackMetadata CallbackMetadata { get; set; }
	}

	public class CallbackMetadata
	{
		[JsonProperty("Item")]
		public List<MetadataItem> Items { get; set; }
	}

	public class MetadataItem
	{
		[JsonProperty("Name")]
		public string Name { get; set; }

		[JsonProperty("Value")]
		public object Value { get; set; }
	}
}
