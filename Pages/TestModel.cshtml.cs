using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Auction_System.Pages
{
	using Microsoft.AspNetCore.Mvc.RazorPages;
	using Microsoft.Extensions.Configuration;
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class TestModel : PageModel
	{
		private readonly IConfiguration _configuration;

		public TestModel(IConfiguration configuration)
		{
			_configuration = configuration;

			// In a real application, you would inject and use a repository service
			// to fetch transactions from your database
			Transactions = GenerateSampleTransactions();
		}

		public List<TransactionViewModel> Transactions { get; private set; }

		public void OnGet()
		{
			// In a real application, you would fetch transactions from your database
			bool isTestMode = _configuration.GetValue<bool>("Mpesa:TestMode");
			ViewData["TestMode"] = isTestMode ? "Enabled" : "Disabled";
		}

		// This is just for demonstration purposes
		// In a real application, you would fetch this data from your database
		private List<TransactionViewModel> GenerateSampleTransactions()
		{
			var random = new Random();
			var transactions = new List<TransactionViewModel>();

			// Only generate sample data in test mode
			if (_configuration.GetValue<bool>("Mpesa:TestMode"))
			{
				// Generate some sample transactions for the demo
				string[] statuses = { "Completed", "Pending", "Failed" };
				string[] phoneNumbers = { "254712345678", "254723456789", "254734567890" };

				for (int i = 0; i < 5; i++)
				{
					transactions.Add(new TransactionViewModel
					{
						TransactionId = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper(),
						PhoneNumber = phoneNumbers[random.Next(phoneNumbers.Length)],
						Amount = random.Next(10, 1000),
						Status = statuses[random.Next(statuses.Length)],
						Timestamp = DateTime.Now.AddMinutes(-random.Next(1, 60))
					});
				}

				// Sort by timestamp (newest first)
				transactions = transactions.OrderByDescending(t => t.Timestamp).ToList();
			}

			return transactions;
		}

		public class TransactionViewModel
		{
			public string TransactionId { get; set; }
			public string PhoneNumber { get; set; }
			public decimal Amount { get; set; }
			public string Status { get; set; }
			public DateTime Timestamp { get; set; }
		}
	}
}
