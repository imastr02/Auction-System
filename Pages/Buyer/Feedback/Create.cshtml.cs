// Pages/Buyer/Feedback/Create.cshtml.cs
using Auction_System.Models;
using Auction_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Auction_System.Pages.Buyer.Feedback
{
	[Authorize]
	public class CreateModel : PageModel
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<AppUser> _userManager;

		public CreateModel(ApplicationDbContext context,
						 UserManager<AppUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		[BindProperty(SupportsGet = true)]
		public int ItemId { get; set; }

		[BindProperty]
		public Auction_System.Models.Feedback Feedback { get; set; } = new();

		public async Task<IActionResult> OnGetAsync()
		{
			var item = await _context.Items
				.Include(i => i.Seller)
				.Include(i => i.Winner)
				.FirstOrDefaultAsync(i => i.Id == ItemId);

			if (item == null || item.Winner == null)
			{
				return NotFound();
			}

			var currentUser = await _userManager.GetUserAsync(User);
			if (currentUser == null || currentUser.Id != item.Winner.Id)
			{
				return Forbid();
			}

			// Pre-populate related IDs
			Feedback.ItemId = item.Id;
			Feedback.BuyerId = currentUser.Id;
			Feedback.SellerId = item.Seller?.Id;
			Feedback.FeedbackDate = DateTime.Now;

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			// Verify the user is still the winner
			var item = await _context.Items.FindAsync(Feedback.ItemId);
			var currentUser = await _userManager.GetUserAsync(User);

			if (item == null || currentUser == null || item.WinnerId != currentUser.Id)
			{
				return Forbid();
			}

			_context.Feedbacks.Add(Feedback);
			await _context.SaveChangesAsync();

			TempData["SuccessMessage"] = "Feedback submitted successfully!";
			return RedirectToPage("../Details", new { id = Feedback.ItemId });
		}
	}
}