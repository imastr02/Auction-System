using Auction_System.Services;
using Microsoft.EntityFrameworkCore;
using static Auction_System.Models.AuctionEvent;
public class AuctionStatusService : BackgroundService
{
	private readonly IServiceProvider _services;

	public AuctionStatusService(IServiceProvider services)
	{
		_services = services;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			using (var scope = _services.CreateScope())
			{
				var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
				var now = DateTime.Now;

				// Update events to "Active"
				var eventsToActivate = await dbContext.AuctionEvents
					.Where(ae => ae.Status == AuctionEventStatus.Scheduled && ae.StartTime <= now)
					.ToListAsync();

				foreach (var auction in eventsToActivate)
				{
					auction.Status = AuctionEventStatus.Active;
				}

				// Update events to "Completed"
				var eventsToComplete = await dbContext.AuctionEvents
					.Where(ae => ae.Status == AuctionEventStatus.Active && ae.EndTime <= now)
					.ToListAsync();

				foreach (var auction in eventsToComplete)
				{
					auction.Status = AuctionEventStatus.Completed;
				}

				await dbContext.SaveChangesAsync();
			}

			// Run every minute
			await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
		}
	}
}