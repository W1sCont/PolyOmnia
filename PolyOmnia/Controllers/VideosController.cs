using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PolyOmnia.Helpers;
using PolyOmnia.Models;
using PolyOmnia.Repository;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace PolyOmnia.Controllers
{
    // [Authorize] // Доступ тільки для авторизованих користувачів
    public class VideoController : Controller
    {
        private readonly IVideoRepository _videoRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IWebHostEnvironment _environment;

        public VideoController(
            IVideoRepository videoRepository,
            IGenreRepository genreRepository,
            IWebHostEnvironment environment)
        {
            _videoRepository = videoRepository;
            _genreRepository = genreRepository;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View(new CreateVideoViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVideoViewModel model)
        {
            // Валідація залежно від обраного типу
            if (model.SourceType == VideoSourceType.LocalFile && model.VideoFile == null)
            {
                ModelState.AddModelError("VideoFile", "Будь ласка, завантажте відеофайл.");
            }
            else if (model.SourceType == VideoSourceType.ExternalUrl && string.IsNullOrWhiteSpace(model.ExternalUrl))
            {
                ModelState.AddModelError("ExternalUrl", "Будь ласка, вкажіть посилання на відео.");
            }

            if (!ModelState.IsValid)
            {
                var genres = await _genreRepository.GetAllAsync();
                ViewBag.Genres = new SelectList(genres, "Id", "Name");
                return View(model);
            }

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out int userId)) return Unauthorized();

            string finalVideoPath = string.Empty;

            // Визначаємо шлях/URL залежно від типу джерела
            if (model.SourceType == VideoSourceType.LocalFile && model.VideoFile != null)
            {
                finalVideoPath = await SaveFileAsync(model.VideoFile, "uploads/videos");
            }
            else if (model.SourceType == VideoSourceType.ExternalUrl && !string.IsNullOrWhiteSpace(model.ExternalUrl))
            {
                finalVideoPath = VideoUrlHelper.ConvertToEmbedUrl(model.ExternalUrl.Trim());
            }

            string? thumbnailPath = null;
            if (model.ThumbnailFile != null)
            {
                thumbnailPath = await SaveFileAsync(model.ThumbnailFile, "uploads/thumbnails");
            }
            else
            {
                thumbnailPath = VideoUrlHelper.GetThumbnailUrl(finalVideoPath);
            }

            var video = new Video
            {
                Title = model.Title,
                Description = model.Description,
                GenreId = model.GenreId,
                UserId = userId,
                SourceType = model.SourceType,
                UrlOrPath = finalVideoPath,
                ThumbnailUrlOrPath = thumbnailPath,
                AllowDownload = model.AllowDownload,
                CreatedAt = DateTime.UtcNow,
                IsPublished = true,
                IsApproved = true // Відео потребує модерації false
            };

            await _videoRepository.AddAsync(video);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Watch(int id)
        {
            var video = await _videoRepository.GetByIdWithDetailsAsync(id);
            if (video == null)
            {
                return NotFound();
            }

            // Збільшуємо лічильник переглядів при відкритті сторінки
            video.ViewCount++;
            await _videoRepository.UpdateAsync(video);

            return View(video);
        }

        // Вспоміжний метод для збереження файлів у wwwroot
        private async Task<string> SaveFileAsync(IFormFile file, string folderPath)
        {
            string uploadsFolder = Path.Combine(_environment.WebRootPath, folderPath);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Генеруємо унікальне ім'я файлу, щоб уникнути перезапису
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return "/" + folderPath + "/" + uniqueFileName;
        }
    }
}