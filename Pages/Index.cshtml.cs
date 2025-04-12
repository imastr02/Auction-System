using Auction_System.Models;
using Auction_System.Services;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Auction_System.Pages
{
    public class IndexModel : PageModel
	{
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;


		public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

		// Properties for search, filters, and pagination
		[BindProperty(SupportsGet = true)]
		public string? Search { get; set; }

		[BindProperty(SupportsGet = true)]
		public DateTime? AuctionDate { get; set; } // Auction date filter

		[BindProperty(SupportsGet = true)]
		public string? SortBy { get; set; } // Sorting criteria
		public Dictionary<int, bool> IsInWatchlist { get; set; } = new Dictionary<int, bool>();

		public int PageIndex { get; set; } = 1;
		public int PageSize { get; set; } = 6; // Number of items per page
		public int TotalItems { get; set; }
		public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);

		// List of items to display
		public List<Item> Items { get; set; } = new List<Item>();

		public async Task OnGetAsync(string search, DateTime? auctionDate, string sortBy, int pageIndex = 1)
		{
			PageIndex = pageIndex;
			Search = search; // Store the search term
			AuctionDate = auctionDate; // Store the auction date filter
			SortBy = sortBy; // Store the sorting criteria

			// Start with a base query that includes related data
			var query = _context.Items
				.Include(i => i.Category)
				.Include(i => i.AuctionEvent)
				.AsQueryable();

			// Apply search filter if the search parameter is not empty
			if (!string.IsNullOrEmpty(Search))
			{
				query = query.Where(i =>
					i.Title!.Contains(Search) ||
					i.Description!.Contains(Search) ||
					i.Category!.CategoryName!.Contains(Search) ||
					i.AuctionEvent!.Name!.Contains(Search)
				);
			}

			// Apply Auction Date filter if provided
			//if (AuctionDate.HasValue)
			//{
			//	query = query.Where(i => i.AuctionDate!.Value.Date == AuctionDate.Value.Date);
			//}

			// Apply sorting based on the SortBy parameter
			switch (SortBy)
			{
				case "AuctionDate":
					query = query.OrderBy(i => i.AuctionDate);
					break;
				case "AuctionEvent":
					query = query.OrderBy(i => i.AuctionEvent!.Name);
					break;
				case "CategoryName":
					query = query.OrderBy(i => i.Category!.CategoryName);
					break;
				default:
					query = query.OrderBy(i => i.AuctionDate); // Default sorting by AuctionDate
					break;
			}

			// Get the total number of items for pagination
			TotalItems = await query.CountAsync();

			// Paginate the results
			Items = await query
				.Skip((PageIndex - 1) * PageSize)
				.Take(PageSize)
				.ToListAsync();

			// Check if each item is in the buyer's watchlist
			var buyer = await _userManager.GetUserAsync(User);
			if (buyer != null)
			{
				var watchlistItemIds = await _context.WatchLists
					.Where(w => w.BuyerId == buyer.Id)
					.Select(w => w.ItemId)
					.ToListAsync();

				foreach (var item in Items)
				{
					IsInWatchlist[item.Id] = watchlistItemIds.Contains(item.Id);
				}
			}
		}
	}
  }
