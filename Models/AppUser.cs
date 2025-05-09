﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Auction_System.Models
{
	public class AppUser : IdentityUser
	{
		[Required]
		[StringLength(10)]
		[RegularExpression(@"^[A-Z]{3}/\d{3}/\d{2}$",
		 ErrorMessage = "Admission number must be in format COM/001/21")]
		public string AdmissionNumber { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }	
		public string? City { get; set; }
		public string? Role { get; set; }
		public DateTime CreatedAt { get; set; }


		//Navigation properties
		public virtual ICollection<Item>? Items { get; set; } = new List<Item>();//Items listed by seller
		public virtual ICollection<Bid>? Bids { get; set; } = new List<Bid>(); //Bids made by buyer
		public List<Item> ItemsWon { get; set; } = new List<Item>();
		public virtual ICollection<Rating>? RatingsReceived { get; set; } = new List<Rating>(); //Rating received as a seller
		public virtual ICollection<Rating>? RatingsGiven { get; set; } = new List<Rating>(); // Rating given as a buyer
		public virtual ICollection<Rating>? Ratings { get; set; } = new List<Rating>();
		public virtual ICollection<WatchList>? WatchLists { get; set; } = new List<WatchList>(); //Item user is watching

		public ICollection<AuctionEvent> AuctionEvents { get; set; } = new List<AuctionEvent>();
																								 // Navigation properties for messages
		

	}
}
