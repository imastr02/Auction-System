using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Auction_System.Models;
using Auction_System.Services;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace Auction_System.Pages.Shared.Seller
{
    public class EditModel : PageModel
    {
        private readonly Auction_System.Services.ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EditModel(Auction_System.Services.ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

		//public List<SelectListItem> AuctionEvents { get; set; }
		public List<SelectListItem> Categories { get; set; }
		public AuctionEvent AuctionEvent { get; set; }

		[BindProperty]
        public IFormFile ImageFile { get; set; }

		[BindProperty]
        public Item Item { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
               // Response.Redirect("/Shared/Seller/Index");
               // return;
                return NotFound();
            }

            var item =  await _context.Items
                .Include(i => i.Category)
                .Include(i => i.AuctionEvent)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            Item = item;

			

			// Fetch categories from the database
			Categories = await _context.Categories
				.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.CategoryName })
				.ToListAsync();

			return Page();
		}

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
				////Repopulate dropdowns and existing data
				//AuctionEvents = await _context.AuctionEvents
				//.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name })
				//.ToListAsync();

				Categories = await _context.Categories
			   .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.CategoryName })
			   .ToListAsync();

				return Page();
            }

			var existingItem = await _context.Items
	      .AsNoTracking() // Ensure the existing item is not tracked
	      .FirstOrDefaultAsync(i => i.Id == Item.Id);

			if (existingItem == null)
            {
                return NotFound();
            }

            //check if any changes were made
            bool hasChanges = existingItem.Title != Item.Title || existingItem.Description != Item.Description || existingItem.StartingPrice != Item.StartingPrice || existingItem.CategoryId != Item.CategoryId || existingItem.StartingTime != Item.StartingTime || existingItem.EndingTime != Item.EndingTime || (ImageFile != null && ImageFile.Length > 0);

            if (!hasChanges)
            {
				//No changes were made, redirect to the list page
				return RedirectToPage("./Index");
			}

            //update only the necessary properties
            existingItem.Title = Item.Title;
            existingItem.Description = Item.Description;
            existingItem.StartingPrice = Item.StartingPrice;
            existingItem.CategoryId = Item.CategoryId;
            existingItem.StartingTime = Item.StartingTime;
            existingItem.EndingTime = Item.EndingTime;

			//Handle Image upload
			if (ImageFile != null && ImageFile.Length > 0)
            {
                // Delete the old image if it exists
                if (!string.IsNullOrEmpty(existingItem.ImagePath))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, existingItem.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Save the new image
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }

                // Update the image path in the database
                existingItem.ImagePath = $"/images/{uniqueFileName}";

            }
			else
			{
				// Retain the existing image path if no new image is uploaded
				existingItem.ImagePath = existingItem.ImagePath;
			}

			//Attach eexisting item and mark it as modified
			_context.Attach(existingItem).State = EntityState.Modified;
           // _context.Attach(Item).State = EntityState.Modified;
            //_context.Entry(Item).Property(x => x.CreatedAt).IsModified = false;


            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(Item.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return RedirectToPage("./Index");
        }
        

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
    }
}
