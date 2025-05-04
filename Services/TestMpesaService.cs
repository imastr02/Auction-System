using Auction_System.Models;
using System;
using System.Threading.Tasks;

namespace Auction_System.Services
{
	public class TestMpesaService : IMpesaService
	{
		private readonly Random _random = new();
		private readonly MpesaSettings _settings;

		public TestMpesaService(MpesaSettings settings)
		{
			_settings = settings;
		}

		public async Task<MpesaResponse> InitiateSTKPushAsync(
			string phoneNumber,
			decimal amount,
			int itemId,
			string userId)
		{
			await Task.Delay(_settings.SimulatedDelayMs);

			var success = _random.NextDouble() > _settings.TestFailureRate;

			return new MpesaResponse
			{
				IsSuccessful = success,
				TransactionId = success ? $"TEST_{Guid.NewGuid():N}" : null,
				ErrorMessage = success ? null : "Simulated payment failure",
				Simulated = true
			};
		}
	}
}