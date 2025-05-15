using Auction_System.Models;
using Auction_System.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;

namespace Auction_System.Pages.Buyer
{
	public class InboxModel : PageModel
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<AppUser> _userManager;

		public List<Notification> Notifications { get; set; } = new();
		public Item SelectedItem { get; set; }
		
		public string? CurrentUserId { get; set; }


		public InboxModel(ApplicationDbContext context, UserManager<AppUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task<IActionResult> OnGetAsync(int? itemId)
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				RedirectToPage("/Login");
			}

			CurrentUserId = user.Id;

			// Always load notifications
			Notifications = await _context.Notifications
				.Where(n => n.UserId == user.Id)
				.Include(n => n.Item)
					.ThenInclude(i => i.Seller)
				.OrderByDescending(n => n.CreatedAt)
				.AsNoTracking()
				.ToListAsync();


		

			// Only load item details if ID is provided
			if (itemId.HasValue)
			{
				SelectedItem = await _context.Items
					.Include(i => i.Seller)
					.Include(i => i.Bids)
					.FirstOrDefaultAsync(i => i.Id == itemId);



				if (SelectedItem == null)
				{
					ModelState.AddModelError("", "Item not found");
				}
				else
				{
					// Mark notification as read
					var notification = Notifications.FirstOrDefault(n => n.ItemId == itemId);
					if (notification != null && !notification.IsRead)
					{
						notification.IsRead = true;
						await _context.SaveChangesAsync();
					}
				}
			}

			return Page();
		}

		public async Task<IActionResult> OnPostPayAsync(int itemId)
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null) return RedirectToPage("/Login");

			var item = await _context.Items
				.Include(i => i.AuctionEvent)
				.FirstOrDefaultAsync(i => i.Id == itemId);

			if (item == null || item.IsSold || item.AuctionEvent.EndTime < DateTime.UtcNow)
			{
				TempData["Error"] = "Item is no longer available";
				return RedirectToPage("./Inbox");
			}

			// Verify notification ownership
			var notification = await _context.Notifications
				.FirstOrDefaultAsync(n => n.ItemId == itemId
					&& n.UserId == user.Id && !n.IsRead);

			if (notification != null)
			{
				notification.IsRead = true;
				await _context.SaveChangesAsync();
			}

			return RedirectToPage("/Payment/Checkout", new { itemId = itemId });
		}
	}
}
