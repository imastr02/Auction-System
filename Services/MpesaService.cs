using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Auction_System.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Auction_System.Services
{
	public class MpesaService : IMpesaService
	{
		private readonly IConfiguration _config;
		private readonly HttpClient _httpClient;
		private readonly ApplicationDbContext _context;

		public MpesaService(
			IConfiguration config,
			HttpClient httpClient, 			ApplicationDbContext context)
		{
			_config = config;
			_httpClient = httpClient;
			_context = context;
			
		}

		public async Task<MpesaResponse> InitiateSTKPushAsync(
			string phoneNumber,
			decimal amount,
			int itemId,
			string userId)
		{
			try
			{
				var token = await GetAccessTokenAsync();
				if (string.IsNullOrEmpty(token))
				{
					return new MpesaResponse
					{
						IsSuccessful = false,
						ErrorMessage = "Failed to authenticate with M-Pesa"
					};
				}

				var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
				var password = Convert.ToBase64String(
					Encoding.UTF8.GetBytes(
						$"{_config["Mpesa:BusinessShortCode"]}" +
						$"{_config["Mpesa:PassKey"]}" +
						$"{timestamp}"));

				var payload = new
				{
					BusinessShortCode = _config["Mpesa:BusinessShortCode"],
					Password = password,
					Timestamp = timestamp,
					TransactionType = "CustomerPayBillOnline",
					Amount = amount,
					PartyA = phoneNumber,
					PartyB = _config["Mpesa:BusinessShortCode"],
					PhoneNumber = phoneNumber,
					CallBackURL = _config["Mpesa:CallbackUrl"],
					AccountReference = $"Item_{itemId}",
					TransactionDesc = $"Payment for item {itemId}"
				};

				var request = new HttpRequestMessage(HttpMethod.Post, _config["Mpesa:STKPushUrl"])
				{
					Content = new StringContent(
						JsonConvert.SerializeObject(payload),
						Encoding.UTF8,
						"application/json")
				};

				request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

				var response = await _httpClient.SendAsync(request);
				var content = await response.Content.ReadAsStringAsync();

				if (!response.IsSuccessStatusCode)
				{
					return new MpesaResponse
					{
						IsSuccessful = false,
						ErrorMessage = $"M-Pesa API Error: {content}"
					};
				}

				var result = JsonConvert.DeserializeObject<dynamic>(content);
				return new MpesaResponse
				{
					IsSuccessful = result.ResponseCode == "0",
					TransactionId = result.CheckoutRequestID,
					Simulated = false
				};

				if (response.IsSuccessStatusCode)
				{
					await CloseItemPayment(itemId);
				}
				else
				{
					return new MpesaResponse
					{
						IsSuccessful = false,
						ErrorMessage = $"API Error: {response.StatusCode} - {content}"
					};
				}

			}
			catch (Exception ex)
			{
				return new MpesaResponse
				{
					IsSuccessful = false,
					ErrorMessage = ex.Message
				};
			}
		}

		private async Task CloseItemPayment(int itemId)
		{
			await using var transaction = await _context.Database.BeginTransactionAsync();
			try
			{
				var item = await _context.Items.FindAsync(itemId);
				if (item != null)
				{
					item.IsPaid = true;
					item.PaidAt = DateTime.UtcNow;
					await _context.SaveChangesAsync();
					await transaction.CommitAsync();
				}
			}
			catch
			{
				await transaction.RollbackAsync();
				throw;
			}
		}


		private async Task<string> GetAccessTokenAsync()
		{
			try
			{
				var authString = $"{_config["Mpesa:ConsumerKey"]}:{_config["Mpesa:ConsumerSecret"]}";
				var encodedAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes(authString));

				var request = new HttpRequestMessage(HttpMethod.Get, _config["Mpesa:AuthUrl"])
				{
					Headers = {
				{"Authorization", $"Basic {encodedAuth}"}
			}
				};

				var response = await _httpClient.SendAsync(request);
				response.EnsureSuccessStatusCode();

				var content = await response.Content.ReadAsStringAsync();
				var tokenResponse = JsonConvert.DeserializeObject<dynamic>(content);

				return tokenResponse?.access_token ?? throw new ApplicationException("Invalid token response");
			}
			catch (Exception ex)
			{
				//_logger.LogError(ex, "Authentication failed");
				throw new ApplicationException("M-Pesa authentication failed. Check credentials.");
			}
		}
	}
}