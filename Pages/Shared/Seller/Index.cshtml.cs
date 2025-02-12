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
    public class IndexModel : PageModel
    {
        private readonly Auction_System.Services.ApplicationDbContext _context;

        public IndexModel(Auction_System.Services.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Item> Item { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Item = await _context.Items
                .Include(i => i.AuctionEvent)
                .Include(i => i.Category)
                .Include(i => i.Seller).ToListAsync();
        }
    }
}
