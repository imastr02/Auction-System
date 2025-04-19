using Auction_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Auction_System.Pages
{
    [Authorize]
    public class UserModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
	
        public List<string> UserRoles { get; set; } = new List<string>();
		public UserModel(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public AppUser CurrentUser { get; set; }
		public AppUser? appUser;
		public async Task<IActionResult> OnGetAsync()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null) return RedirectToPage("/Account/Login");

			CurrentUser = user;
			return Page();


			//if (user != null)
			//         {
			//             UserRoles = (await _userManager.GetRolesAsync(user)).ToList();
			//         }
			//         appUser = user;
		}


		public async Task<IActionResult> OnPostDeleteAccountAsync(string password)
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null) return RedirectToPage("/Account/Login");

			if (await _userManager.CheckPasswordAsync(user, password))
			{
				var result = await _userManager.DeleteAsync(user);
				if (result.Succeeded)
				{
					await _signInManager.SignOutAsync();
					return RedirectToPage("/Index");
				}
			}

			ModelState.AddModelError(string.Empty, "Incorrect password");
			return Page();
		}
	}
}

