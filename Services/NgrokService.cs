using Newtonsoft.Json.Linq;

namespace Auction_System.Services
{
	// Services/INgrokService.cs
	public interface INgrokService
	{
		Task<string> GetPublicUrlAsync();
	}

	// Services/NgrokService.cs
	public class NgrokService : INgrokService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ILogger<NgrokService> _logger;
		private string _cachedUrl;

		public NgrokService(IHttpClientFactory httpClientFactory, ILogger<NgrokService> logger)
		{
			_httpClientFactory = httpClientFactory;
			_logger = logger;
		}

		public async Task<string> GetPublicUrlAsync()
		{
			if (!string.IsNullOrEmpty(_cachedUrl)) return _cachedUrl;

			try
			{
				var client = _httpClientFactory.CreateClient();
				var response = await client.GetAsync("http://localhost:4040/api/tunnels");
				response.EnsureSuccessStatusCode();

				var content = await response.Content.ReadAsStringAsync();
				var tunnels = JObject.Parse(content)["tunnels"] as JArray;

				var httpsTunnel = tunnels?.FirstOrDefault(t =>
					t["proto"]?.ToString() == "https" &&
					t["config"]?["addr"]?.ToString().EndsWith(":80") == true);

				_cachedUrl = httpsTunnel?["public_url"]?.ToString();

				if (string.IsNullOrEmpty(_cachedUrl))
					throw new ApplicationException("No active HTTPS tunnel found");

				_logger.LogInformation("Ngrok URL: {Url}", _cachedUrl);
				return _cachedUrl;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to get Ngrok URL");
				throw new ApplicationException("Start Ngrok with: ngrok http 80 --host-header=localhost", ex);
			}
		}
	}
}
