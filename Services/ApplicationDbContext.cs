using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Auction_System.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Auction_System.Services
{
	public class ApplicationDbContext : IdentityDbContext<AppUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
		{
		}

		public DbSet<Item> Items { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<WatchList> WatchLists { get; set; }
		public DbSet<Bid> Bids { get; set; }
		public DbSet<Rating> Ratings { get; set; }
		public DbSet<AuctionEvent> AuctionEvents { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Seed Auction Events
			modelBuilder.Entity<AuctionEvent>().HasData(
				new AuctionEvent { Id = 1, Name = "9 AM - 10 AM" },
				new AuctionEvent { Id = 2, Name = "12 PM - 1 PM" },
				new AuctionEvent { Id = 3, Name = "3 PM - 4 PM" }
			);

			// Seed Categories
			modelBuilder.Entity<Category>().HasData(
				new Category { Id = 1,  CategoryName = "Phones" },
				new Category { Id = 2, CategoryName = "Accessories" },
				new Category { Id = 3, CategoryName = "Others" }
			);


			// Configure relationships
			modelBuilder.Entity<Item>()
				.HasOne(i => i.Seller)
				.WithMany(u => u.Items)
				.HasForeignKey(i => i.SellerId)
				.HasPrincipalKey(u => u.Id)
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<Item>()
				.HasOne(i => i.Category)
				.WithMany(c => c.Items)
				.HasForeignKey(i => i.CategoryId);

			modelBuilder.Entity<Item>()
				.HasOne(i => i.AuctionEvent)
				.WithMany(e => e.Items)
				.HasForeignKey(i => i.AuctionEventId);

			modelBuilder.Entity<WatchList>()
				.HasOne(w => w.User)
				.WithMany(u => u.WatchLists)
				.HasForeignKey(w => w.UserId)
				.HasPrincipalKey(u => u.Id)
				 .OnDelete(DeleteBehavior.NoAction); ;

			modelBuilder.Entity<WatchList>()
				.HasOne(w => w.Item)
				.WithMany(i => i.WatchLists)
				.HasForeignKey(w => w.ItemId);

			modelBuilder.Entity<Bid>()
				.HasOne(b => b.Buyer)
				.WithMany(u => u.Bids)
				.HasForeignKey(b => b.BuyerId)
				.HasPrincipalKey(u => u.Id)
				 .OnDelete(DeleteBehavior.NoAction); 

			modelBuilder.Entity<Bid>()
				.HasOne(b => b.Item)
				.WithMany(i => i.Bids)
				.HasForeignKey(b => b.ItemId);

			modelBuilder.Entity<Rating>()
				.HasOne(r => r.Seller)
				.WithMany(u => u.RatingsReceived)
				.HasForeignKey(r => r.SellerId)
				.HasPrincipalKey(u => u.Id)
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<Rating>()
				.HasOne(r => r.Buyer)
				.WithMany(u => u.RatingsGiven)
				.HasForeignKey(r => r.BuyerId)
				.HasPrincipalKey(u => u.Id)
				.OnDelete(DeleteBehavior.NoAction);

				modelBuilder.Entity<Rating>()
				.HasOne(r => r.Item)
				.WithMany(i => i.Ratings)
				.HasForeignKey(r => r.ItemId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
