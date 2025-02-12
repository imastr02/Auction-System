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

namespace Auction_System.Pages.Shared.Seller
{
    public class CreateModel : PageModel
    {
        private readonly Auction_System.Services.ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CreateModel(Auction_System.Services.ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


		public List<SelectListItem> AuctionEvents { get; set; }
		public List<SelectListItem> Categories { get; set; }
        //public ItemDto ItemDto { get; set; } = new ItemDto();

        [BindProperty]
        public IFormFile? ImageFile { get; set; }


		public async Task<IActionResult> OnGet()
        {
			// Fetch auction events from the database
			AuctionEvents = await _context.AuctionEvents
				.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name })
				.ToListAsync();

			// Fetch categories from the database
			Categories = await _context.Categories
				.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.CategoryName })
				.ToListAsync();
			return Page();
        }

        [BindProperty]
        public Item Item { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {


            if (!ModelState.IsValid)
            {
                return Page();
            }

			////save image file
			//string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
			//newFileName += Path.GetExtension(ItemDto.ImageFile!.FileName);

			//string imageFullPath = _webHostEnvironment.WebRootPath +"/images/" + newFileName;
			//using (var stream = System.IO.File.Create(imageFullPath))
			//{
			//    ItemDto.ImageFile.CopyTo(stream);
			//}

			//Item.ImagePath = newFileName; 

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
           
           
            _context.Items.Add(Item);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
