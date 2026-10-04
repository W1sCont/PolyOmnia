using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PolyOmnia.Models;
using PolyOmnia.Repository;
using System.Security.Claims;

namespace PolyOmnia.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVideoRepository _videoRepository;
        private readonly IGenreRepository _genreRepository;

        public HomeController(IVideoRepository videoRepository, IGenreRepository genreRepository)
        {
            _videoRepository = videoRepository;
            _genreRepository = genreRepository;
        }

        public async Task<IActionResult> Index()
        {
            var videos = await _videoRepository.GetApprovedVideosAsync();

            var model = new HomeViewModel
            {
                HeroVideo = videos.FirstOrDefault(),
                Videos = videos,
                Genres = await _genreRepository.GetAllAsync()
            };

            return View(model);
        }
    }
}