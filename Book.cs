using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace BookShopCart.Models
{
	public class Book
	{
        public int Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string BookName { get; set; }
        [Required]
        [MaxLength(40)]
        public string AuthorName { get; set; }
        public string? BookImage { get; set; }
		[Required]
		public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public double Price { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }
        public List<CartDetail> CartDetail { get; set; }
        public Stock Stock { get; set; }

        [NotMapped]
        public string GenreName { get; set; }
    }
}
