using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Auction_System.Models;
using Auction_System.Services;
using Microsoft.AspNetCore.Identity;

namespace Auction_System.Pages.Shared.Seller
{
    public class IndexModel : PageModel
    {
        private readonly Auction_System.Services.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public IndexModel(Auction_System.Services.ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Item> Item { get;set; } = default!;

        public async Task OnGetAsync()
        {
            //Get current User (seller)
            var seller = await _userManager.GetUserAsync(User);
            if (seller == null)
            {
                //Handle the case where user is not logged into
                RedirectToPage("/Account/Login");
            }
           
            Item = await _context.Items
                .Include(i => i.AuctionEvent)
                .Include(i => i.Category)
                //.Include(i => i.Seller)
                .Where(i => i.SellerId == seller!.Id)
                .ToListAsync();
        }
    }
}
