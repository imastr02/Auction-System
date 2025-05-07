using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Auction_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Auction_System.Pages.Admin
{
	public class ViewBuyersModel : PageModel
	{
		private readonly UserManager<AppUser> _userManager;

		public ViewBuyersModel(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
		}

		public List<AppUser> Buyers { get; set; }

		public async Task OnGetAsync()
		{
			var users = await _userManager.GetUsersInRoleAsync("Buyer");
			Buyers = users.ToList();
		}

		public async Task<IActionResult> OnPostDeleteAsync(string id)
		{
			var buyer = await _userManager.FindByIdAsync(id);
			if (buyer == null)
			{
				return NotFound();
			}

			var result = await _userManager.DeleteAsync(buyer);
			if (!result.Succeeded)
			{
				ModelState.AddModelError(string.Empty, "Failed to delete buyer.");
			}

			return RedirectToPage(); // Refresh the page
		}

	}
}
