using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Auction_System.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using Auction_System.Models.Auction_System.Models;
using DocumentFormat.OpenXml.Vml.Office;
using Humanizer;

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
		public DbSet<ItemFeedback> ItemFeedbacks {  get; set; }
		public DbSet<Feedback> Feedbacks { get; set; }
		public DbSet<Purchase> Purchases { get; set; }
		//public DbSet<ChatMessage> ChatMessages { get; set; }
		public DbSet<Notification> Notifications { get; set; }
		public DbSet<MpesaTransaction> MpesaTransactions {  get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

		

			// Seed Categories
			modelBuilder.Entity<Category>().HasData(
				new Category { Id = 1,  CategoryName = "Phones" },
				new Category { Id = 2, CategoryName = "Accessories" },
				new Category { Id = 3, CategoryName = "Others" },
				new Category { Id = 4, CategoryName = "Laptops"},
				new Category { Id = 5, CategoryName = "Furnitures"}
			);


			// Configure relationships
			modelBuilder.Entity<AppUser>()
				.HasMany(u => u.Bids)
				.WithOne(b => b.Buyer)
				.HasForeignKey(b => b.BuyerId);

			modelBuilder.Entity<AppUser>()
				.HasMany(u => u.ItemsWon)
				.WithOne(i => i.Winner)
				.HasForeignKey(i => i.WinnerId);

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

			modelBuilder.Entity<Item>()
				   .HasOne(i => i.Winner)
				   .WithMany(u => u.ItemsWon)
				   .HasForeignKey(i => i.WinnerId)
				   .OnDelete(DeleteBehavior.SetNull); // Prevent deletion of winner if items exist

			modelBuilder.Entity<Item>().Property(i => i.IsPaid).HasDefaultValue(false);

			modelBuilder.Entity<WatchList>()
				.HasOne(w => w.Buyer)
				.WithMany(u => u.WatchLists)
				.HasForeignKey(w => w.BuyerId)
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
				 .OnDelete(DeleteBehavior.SetNull); 

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

			// AuctionEvent to Items (One-to-Many)
			modelBuilder.Entity<AuctionEvent>()
				.HasMany(ae => ae.Items)
				.WithOne(i => i.AuctionEvent)
				.HasForeignKey(i => i.AuctionEventId)
				.OnDelete(DeleteBehavior.Cascade); // Cascade delete items when auction is deleted

			// AuctionEvent to Seller(AppUser) (Many - to - One)

		modelBuilder.Entity<AuctionEvent>()
			.HasOne(ae => ae.Seller)
			.WithMany(u => u.AuctionEvents)
			.HasForeignKey(ae => ae.SellerId)
			.OnDelete(DeleteBehavior.SetNull); // Prevent seller deletion if auctions exist

			modelBuilder.Entity<Notification>()
		   .HasOne(n => n.User)
		   .WithMany()
		   .HasForeignKey(n => n.UserId)
		   .OnDelete(DeleteBehavior.SetNull);

			modelBuilder.Entity<Notification>()
				.HasOne(n => n.Item)
				.WithMany()
				.HasForeignKey(n => n.ItemId)
				.OnDelete(DeleteBehavior.SetNull);
			modelBuilder.Entity<Feedback>()
	.HasOne(f => f.Buyer)
	.WithMany()
	.HasForeignKey(f => f.BuyerId)
	.OnDelete(DeleteBehavior.SetNull);


		}
	}
}
