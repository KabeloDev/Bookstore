using BookShopCart.Constants;
using BookShopCart.DTOs;
using BookShopCart.Models;
using BookShopCart.Repositories;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopCart.Controllers
{
	[Authorize(Roles = nameof(Roles.Admin))]
	public class GenreController : Controller
	{
		private readonly IGenreRepository _genreRepository;

		public GenreController(IGenreRepository genreRepository)
		{
			_genreRepository = genreRepository;
		}

		public async Task<IActionResult> Index()
		{
			return View(await _genreRepository.GetAllGenres());
		}

		public IActionResult AddGenre()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> AddGenre(GenreDTO genre)
		{
			if (!ModelState.IsValid)
			{
				return View(genre);
			}

			try
			{
				var genreToAdd = new Genre
				{
					GenreName = genre.GenreName,
					Id = genre.Id,
				};
				await _genreRepository.AddGenre(genreToAdd);
				TempData["successMessage"] = "Genre added successfully!";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception)
			{

				TempData["errorMessage"] = "Genre has not been added";
				return View(genre);
			}
		}

		public async Task<IActionResult> UpdateGenre (int id)
		{
			var genre = await _genreRepository.GetGenreById(id);
			if (genre == null)
				throw new InvalidOperationException($"Genre with id: {id} was not found");

			var genreToUpdate = new GenreDTO
			{
				Id = genre.Id,
				GenreName = genre.GenreName,
			};

			return View(genreToUpdate);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateGenre(GenreDTO genreToUpdate)
		{
			if (!ModelState.IsValid)
			{
				return View(genreToUpdate);
			}

			try
			{
				var genre = new Genre
				{
					GenreName = genreToUpdate.GenreName,
					Id = genreToUpdate.Id,
				};

				await _genreRepository.UpdateGenre(genre);
				TempData["successMessage"] = "Genre updated successfully!";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception)
			{

				TempData["errorMessage"] = "Genre has not been updated";
				return View(genreToUpdate);
			}
		}

		public async Task<IActionResult> DeleteGenre(int id)
		{
			var genre = await _genreRepository.GetGenreById(id);
			if (genre == null)
				throw new InvalidOperationException($"Gemre with id: {id} was not found");
			await _genreRepository.DeleteGenre(genre);
			return RedirectToAction(nameof(Index));
		}
	}
}
