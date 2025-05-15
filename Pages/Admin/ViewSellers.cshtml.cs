
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Auction_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Auction_System.Pages.Admin
{
	public class ViewSellersModel : PageModel
	{
		private readonly UserManager<AppUser> _userManager;

		public ViewSellersModel(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
		}

		public List<AppUser> Sellers { get; set; }

		public async Task OnGetAsync()
		{
			var users = await _userManager.GetUsersInRoleAsync("Seller");
			Sellers = users.ToList();
		}

		public async Task<IActionResult> OnPostDeleteAsync(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
			{
				return NotFound();
			}

			// Optional: Check if user is a seller
			if (!await _userManager.IsInRoleAsync(user, "Seller"))
			{
				return Forbid(); // Only allow deleting sellers
			}

			await _userManager.DeleteAsync(user);

			return RedirectToPage(); // Refresh the list
		}

	}
}
