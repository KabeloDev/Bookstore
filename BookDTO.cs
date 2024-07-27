using BookShopCart.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BookShopCart.DTOs
{
	public class BookDTO
	{
		public int Id { get; set; }
		[Required]
		[MaxLength(40)]
		public string BookName { get; set; }
		public int GenreId { get; set; }
		[Required]
		[MaxLength(40)]
		public string AuthorName { get; set; }
		public string? BookImage { get; set; }
		[Required]
		public double Price { get; set; }
		public IFormFile? ImageFile { get; set; }
		public IEnumerable<SelectListItem> GenreList { get; set; }
	}
}
