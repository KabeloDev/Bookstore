using System.ComponentModel.DataAnnotations;

namespace BookShopCart.Models
{
	public class CartDetail
	{
        public int Id { get; set; }
		[Required]
		public int ShoppingCartId { get; set; }
		public ShoppingCart ShoppingCart { get; set; }
		public int BookId { get; set; }
		public Book Book { get; set; }
		public int Quantity { get; set; }
		[Required]
		public double UnitPrice { get; set; }
	}
}
