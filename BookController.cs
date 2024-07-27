using BookShopCart.Constants;
using BookShopCart.DTOs;
using BookShopCart.Models;
using BookShopCart.Repositories;
using BookShopCart.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShopCart.Controllers
{
	[Authorize(Roles = nameof(Roles.Admin))]
	public class BookController : Controller
	{
		private readonly IBookRepository _bookRepository;
		private readonly IGenreRepository _genreRepository;
		private readonly IFileService _fileService;

		public BookController (IBookRepository bookRepository, IFileService fileService, IGenreRepository genreRepository)
		{
			_bookRepository = bookRepository;
			_fileService = fileService;
			_genreRepository = genreRepository;
		}


		public async Task<IActionResult> Index()
		{
			return View(await _bookRepository.GetBooks());
		}

		public async Task<IActionResult> AddBook()
		{
			var genreList = (await _genreRepository.GetAllGenres()).Select(genre
			=> new SelectListItem
			{
				Text = genre.GenreName,
				Value = genre.Id.ToString()
			});

			BookDTO bookDTO = new BookDTO()
			{
				GenreList = genreList
			};

			return View(bookDTO);
		}

		[HttpPost]
        public async Task<IActionResult> AddBook(BookDTO bookDTO)
		{
            var genreList = (await _genreRepository.GetAllGenres()).Select(genre
            => new SelectListItem
            {
                Text = genre.GenreName,
                Value = genre.Id.ToString()
            });
			bookDTO.GenreList = genreList;

			if (ModelState.IsValid) 
				return View(bookDTO);

			try
			{
				if (bookDTO.ImageFile != null)
				{
					if (bookDTO.ImageFile.Length > 1 * 1024 * 1024)
					{
						throw new InvalidOperationException("Image file cannot exceed 1MB");
					}
					string[] allowedExtensions = [".jpeg", ".jpg", ".png"];
					string imageName = await _fileService.SaveFile(bookDTO.ImageFile, allowedExtensions);
					bookDTO.BookImage = imageName;
				}

				Book book = new Book()
				{
					Id = bookDTO.Id,
					BookName = bookDTO.BookName,
					AuthorName = bookDTO.AuthorName,
					BookImage = bookDTO.BookImage,
					GenreId = bookDTO.GenreId,
					Price = bookDTO.Price,
				};

				await _bookRepository.AddBook(book);
				TempData["successMessage"] = "Book is added successfully!";
				return RedirectToAction(nameof(Index));
			}
			catch (InvalidOperationException)
			{

                TempData["errorMessage"] = "Book has not been added";
				return View(bookDTO);
            }
			catch (Exception)
			{
                TempData["errorMessage"] = "Error on saving data";
                return View(bookDTO);
            }
        }

		public async Task<IActionResult> UpdateBook (int id)
		{
			var book = await _bookRepository.GetBookById(id);
			if (book == null)
			{
				TempData["successMessage"] = $"Book with the id: {id} was not found";
				RedirectToAction(nameof(Index));
			}

			var genreList = (await _genreRepository.GetAllGenres()).Select(genre =>
			new SelectListItem
			{
				Text = genre.GenreName,
				Value = genre.Id.ToString(),
				Selected = genre.Id == book.GenreId
			});

			BookDTO bookDTO = new BookDTO()
			{
				GenreList = genreList,
				BookName = book.BookName,
				AuthorName = book.AuthorName,
				GenreId = book.GenreId,
				Price = book.Price,
			};

			return View(bookDTO);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateBook(BookDTO bookDTO)
		{
			var genreList = (await _genreRepository.GetAllGenres()).Select(genre =>
			new SelectListItem
			{
				Text = genre.GenreName,
				Value = genre.Id.ToString(),
				Selected = genre.Id == bookDTO.GenreId
			});

			bookDTO.GenreList = genreList;

			if (ModelState.IsValid)
			{
				return View(bookDTO);
			}

			try
			{
				string oldImage = "";
				if (bookDTO.ImageFile != null)
				{
					if (bookDTO.ImageFile.Length > 1 * 1024 * 1024)
					{
						throw new InvalidOperationException($"Image file cannot exceed 1MB");
					}
					string[] allowedExtensions = [".jpeg", ".jpg", ".png"];
					string imageName = await _fileService.SaveFile(bookDTO.ImageFile, allowedExtensions);

					oldImage = bookDTO.BookImage;
					bookDTO.BookImage = imageName;
				}

				Book book = new Book()
				{
					Id = bookDTO.Id,
					BookName = bookDTO.BookName,
					AuthorName = bookDTO.AuthorName,
					GenreId = bookDTO.GenreId,
					Price = bookDTO.Price,
					BookImage = bookDTO.BookImage,
				};

				await _bookRepository.UpdateBook(book);

				if (!string.IsNullOrWhiteSpace(oldImage))
				{
					_fileService.DeleteFile(oldImage);
				}
				TempData["successMessage"] = "Book is updated successfully!";
				return RedirectToAction("Index");
			}


			catch (InvalidOperationException ex)
			{

				TempData["errorMessage"] = ex.Message;
				return View(bookDTO);
			}

		}

		public async Task<IActionResult> DeleteBook (int id)
		{
			try
			{
				var book = await _bookRepository.GetBookById(id);
				if (book == null)
				{
					TempData["errormessage"] = $"Book with id: {id} was not found";
					return View(book);
				}
				else
				{
					await _bookRepository.DeleteBook(book);
					if (!string.IsNullOrEmpty(book.BookImage))
					{
						_fileService.DeleteFile(book.BookImage);
					}
					TempData["successMessage"] ="Book successfully deleted";
					return RedirectToAction("Index");	

				}
			}
			catch (InvalidOperationException ex)
			{

				throw;
			}
		}

	}
}
