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
	}
}
