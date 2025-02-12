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
        public AppUser? appUser;
        public List<string> UserRoles { get; set; } = new List<string>();
		public UserModel(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
		}
		public async Task OnGetAsync()
        {
         //Get current user
         var user = await _userManager.GetUserAsync(User);

            if(user != null)
            {
                UserRoles = (await _userManager.GetRolesAsync(user)).ToList();
            }
            appUser = user;
        }
    }
}
