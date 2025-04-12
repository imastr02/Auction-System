using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Auction_System.Models;
using Auction_System.Services;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Auction_System.Pages.Shared.Seller
{
    public class CreateModel : PageModel
    {
        private readonly Auction_System.Services.ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly UserManager<AppUser> _userManager;

        public CreateModel(Auction_System.Services.ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<AppUser> userManager)
        {
            _context = context;
			_webHostEnvironment = webHostEnvironment;
			_userManager = userManager;
        }


		[BindProperty]
		//public int SelectedAuctionEventId {  get; set; }
		//public List<SelectListItem> AuctionEvents { get; set; } = new List<SelectListItem>();
		public AuctionEvent AuctionEvent { get; set; }
		
		public List<SelectListItem> Categories { get; set; }
    

        [BindProperty]
        public IFormFile? ImageFile { get; set; }


		public async Task OnGetAsync()
        {
	

			// Fetch categories from the database
			Categories = await _context.Categories
				.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.CategoryName })
				.ToListAsync();
			

			//return Page();
        }

        [BindProperty]
        public Item Item { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
			if (!ModelState.IsValid)
			{
				// Log validation errors
				foreach (var modelState in ModelState.Values)
				{
					foreach (var error in modelState.Errors)
					{
						Console.WriteLine($"Validation error: {error.ErrorMessage}");
					}
				}

				// Repopulate dropdowns to avoid rendering issues
				Categories = await _context.Categories
					.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.CategoryName })
					.ToListAsync();

				return Page();
			}


			// Get the current user (seller)
			var seller = await _userManager.GetUserAsync(User);
			if (seller == null)
			{
				return RedirectToPage("/Account/Login");
			}

			Item.SellerId = seller.Id; // Set the SellerId
									   //Item.Seller = seller;		

			// Print ModelState errors for debugging
		

			if (ImageFile != null)
			{
				// Generate unique file name
				string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);

				// Define upload path
				string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
				if (!Directory.Exists(uploadsFolder))
				{
					Directory.CreateDirectory(uploadsFolder);
				}

				// Full file path
				string filePath = Path.Combine(uploadsFolder, fileName);

				// Save the file
				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					await ImageFile.CopyToAsync(fileStream);
				}

				// Save the file path in the database
				Item.ImagePath = "/images/" + fileName;
			}

				Item.CreatedAt = DateTime.Now;

			var auctionEvent = new AuctionEvent
			{
				Name = Item.Title,
				StartTime = (DateTime)Item.StartingTime!,
				EndTime = (DateTime)Item.EndingTime!,
				AuctionDate = Item.AuctionDate,
				SellerId = seller.Id,

			};


			_context.AuctionEvents.Add(auctionEvent);
			await _context.SaveChangesAsync(); // Save to get the ID

			// Now set the AuctionEventId on the Item
			Item.AuctionEventId = auctionEvent.Id;

			// Add and save the Item
			_context.Items.Add(Item);
			await _context.SaveChangesAsync();

			return RedirectToPage("./Index");
        }


    }
}
