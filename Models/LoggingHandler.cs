namespace Auction_System.Models
{
	public class LoggingHandler : DelegatingHandler
	{
		private readonly ILogger<LoggingHandler> _logger;

		public LoggingHandler(ILogger<LoggingHandler> logger)
		{
			_logger = logger;
		}

		protected override async Task<HttpResponseMessage> SendAsync(
			HttpRequestMessage request,
			CancellationToken cancellationToken)
		{
			_logger.LogInformation("Request: {Method} {Url}",
				request.Method,
				request.RequestUri);

			var response = await base.SendAsync(request, cancellationToken);

			_logger.LogInformation("Response: {StatusCode} {Content}",
				response.StatusCode,
				await response.Content.ReadAsStringAsync());

			return response;
		}
	}
}
