using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Auction_System.Models;
using Auction_System.Services;

namespace Auction_System.Pages.Shared.Seller
{
    public class DetailsModel : PageModel
    {
        private readonly Auction_System.Services.ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DetailsModel(Auction_System.Services.ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

		 public Item Item { get; set; } = default!;
		//public List<Item> Items { get; set; } = new List<Item>();

		public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Category) // Include related data (optional)
				.Include(i => i.AuctionEvent) // Include related data (optional)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (item == null)
            {
                return NotFound();
            }
            else
            {
                Item = item;
            }
            return Page();
        }
    }
}
