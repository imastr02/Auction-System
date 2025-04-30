using Auction_System.Models;
using Auction_System.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Auction_System.Pages.Buyer
{
    public class InboxModel : PageModel
    {
		private readonly ApplicationDbContext _context;
		private readonly UserManager<AppUser> _userManager;

		public List<Notification> Notifications { get; set; } = new();
		public Item SelectedItem { get; set; }

		public InboxModel(ApplicationDbContext context, UserManager<AppUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task OnGetAsync(int? itemId)
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				 RedirectToPage("/Login");
			}

			// Load notifications with related items
			Notifications = await _context.Notifications
				.Where(n => n.UserId == user.Id)
				.Include(n => n.Item)
					.ThenInclude(i => i.Seller)
				.Include(n => n.Item)
					.ThenInclude(i => i.Bids)
				.OrderByDescending(n => n.CreatedAt)
				.ToListAsync();

			// Mark notification as read if viewing details
			if (itemId.HasValue)
			{
				var notification = Notifications.FirstOrDefault(n => n.ItemId == itemId);
				if (notification != null && !notification.IsRead)
				{
					notification.IsRead = true;
					await _context.SaveChangesAsync();
				}

				// In your page model
				SelectedItem = await _context.Items
					.Include(i => i.Seller)
					.Include(i => i.Bids)
					.Include(i => i.AuctionEvent)
					.FirstOrDefaultAsync(i => i.Id == itemId);
			}
		}
	}
}
