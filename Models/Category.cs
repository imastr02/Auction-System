namespace Auction_System.Models
{
	public class Category
	{
		public int Id { get; set; }
		public string? CategoryName { get; set; }

		public ICollection<Item>? Items { get; set; }
	}
}
