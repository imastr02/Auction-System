using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

public class MpesaCallbackModel : PageModel
{
	private readonly ILogger<MpesaCallbackModel> _logger;

	public MpesaCallbackModel(ILogger<MpesaCallbackModel> logger)
	{
		_logger = logger;
	}

	public async Task<IActionResult> OnPostAsync()
	{
		// Read the request body
		using (StreamReader reader = new StreamReader(Request.Body))
		{
			string requestBody = await reader.ReadToEndAsync();

			// Log the callback data for debugging
			_logger.LogInformation($"M-Pesa callback received: {requestBody}");

			// Parse the callback data
			dynamic callbackData = JsonConvert.DeserializeObject(requestBody);

			// Here you would typically:
			// 1. Validate the transaction
			// 2. Update your database
			// 3. Trigger any business logic (e.g., confirm order, send email)

			// For this example, we're just logging the data
			if (callbackData != null && callbackData.Body?.stkCallback != null)
			{
				string resultCode = callbackData.Body.stkCallback.ResultCode?.ToString();
				string resultDesc = callbackData.Body.stkCallback.ResultDesc?.ToString();
				string merchantRequestId = callbackData.Body.stkCallback.MerchantRequestID?.ToString();
				string checkoutRequestId = callbackData.Body.stkCallback.CheckoutRequestID?.ToString();

				if (resultCode == "0")
				{
					// Payment successful
					_logger.LogInformation($"Payment successful for request {checkoutRequestId}");

					// In a real application, you would update your database
					// and perform necessary business logic
				}
				else
				{
					// Payment failed
					_logger.LogWarning($"Payment failed for request {checkoutRequestId}: {resultDesc}");
				}
			}
		}

		// Always respond with a success to acknowledge receipt
		return new JsonResult(new { ResultCode = 0, ResultDesc = "Success" });
	}
}