using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
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

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true,
                    SameSite = SameSiteMode.Lax
                }
            );

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}