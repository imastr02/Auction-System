using Microsoft.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Auction_System.Models;

namespace Auction_System.Services
{
	public static class SeedData
	{
		public static async Task Initialize (IServiceProvider serviceProvider)
		{
			var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
			var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

			//Seed roles
			string[] roleNames = { "Admin", "Buyer", "Seller" };

			foreach (var roleName in roleNames) 
			{ 
				var roleExists = await roleManager.RoleExistsAsync (roleName);
				if( !roleExists )
				{
					await roleManager.CreateAsync(new IdentityRole(roleName));
				}
			}

			//Create admin user
			// Create admin user
			var adminEmail = "admin@gmail.com";
			var adminUser = await userManager.FindByEmailAsync(adminEmail);
			if (adminUser == null)
			{
				adminUser = new AppUser
				{
					UserName = adminEmail,
					Email = adminEmail,
					EmailConfirmed = true // Mark the email as confirmed
				};

				// Create the admin user with a password
				var createUserResult = await userManager.CreateAsync(adminUser, "Admin@123"); // Use a strong password
				if (createUserResult.Succeeded)
				{
					// Assign the "Admin" role to the admin user
					await userManager.AddToRoleAsync(adminUser, "Admin");
				}
			}


		}
	}
}
