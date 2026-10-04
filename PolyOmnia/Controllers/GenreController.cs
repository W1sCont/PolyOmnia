using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolyOmnia.Models;
using PolyOmnia.Repository;

namespace PolyOmnia.Controllers
{
    // Обмеження доступу: лише для Admin та Moderator
    [Authorize(Roles = "Admin,Moderator")]
    public class GenreController : Controller
    {
        private readonly IGenreRepository _genreRepository;

        public GenreController(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        // Список усіх жанрів
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var genres = await _genreRepository.GetAllAsync();
            return View(genres);
        }

        // Форма створення жанру
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGenreViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var genre = new Genre
            {
                Name = model.Name.Trim()
            };

            await _genreRepository.AddAsync(genre);
            TempData["SuccessMessage"] = "Жанр успішно додано!";

            return RedirectToAction(nameof(Index));
        }

        // Форма редагування
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var genre = await _genreRepository.GetByIdAsync(id);
            if (genre == null) return NotFound();

            var model = new EditGenreViewModel
            {
                Id = genre.Id,
                Name = genre.Name
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditGenreViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var genre = await _genreRepository.GetByIdAsync(model.Id);
            if (genre == null) return NotFound();

            genre.Name = model.Name.Trim();
            await _genreRepository.UpdateAsync(genre);

            TempData["SuccessMessage"] = "Жанр успішно оновлено!";
            return RedirectToAction(nameof(Index));
        }

        // Видалення жанру
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var genre = await _genreRepository.GetByIdAsync(id);
            if (genre != null)
            {
                await _genreRepository.DeleteAsync(genre);
                TempData["SuccessMessage"] = "Жанр успішно видалено!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}