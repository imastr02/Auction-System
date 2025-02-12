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

		public List<SelectListItem> AuctionEvents { get; set; }
		public List<SelectListItem> Categories { get; set; }

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

            var item =  await _context.Items.FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            Item = item;
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }




            if (ImageFile != null && ImageFile.Length > 0)
            {
                // Delete the old image if it exists
                if (!string.IsNullOrEmpty(Item.ImagePath))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, Item.ImagePath.TrimStart('/'));
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
                Item.ImagePath = $"/images/{uniqueFileName}";

            }
            else
            {
                //// Retain the existing image path if no new image is uploaded
                Item.ImagePath = Item.ImagePath;
            }

            _context.Attach(Item).State = EntityState.Modified;
            _context.Entry(Item).Property(x => x.CreatedAt).IsModified = false;


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
