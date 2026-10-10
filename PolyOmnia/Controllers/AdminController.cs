using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using PolyOmnia.Models;
using PolyOmnia.Repository;

namespace PolyOmnia.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IVideoRepository _videoRepository;
        private readonly IGenreRepository _genreRepository;

        public AdminController(IUserRepository userRepository, IVideoRepository videoRepository, IGenreRepository genreRepository)
        {
            _userRepository = userRepository;
            _videoRepository = videoRepository;
            _genreRepository = genreRepository;
        }

        public async Task<IActionResult> ApproveUsers()
        {
            var users = await _userRepository.GetUsersByRoleAsync(UserRole.Unconfirmed);
            var model = users.Select(u => new AdminUserItemViewModel
            {
                Id = u.Id,
                Name = u.Name,
                Surname = u.Surname,
                Login = u.Login,
                Email = u.Email,
                CreatedAt = u.CreatedAt
            }).ToList();

            return View(model);
        }

        public async Task<IActionResult> ApproveVideos()
        {
            var videos = await _videoRepository.GetPendingVideosAsync();
            var model = videos.Select(v => new AdminVideoItemViewModel
                {
                    Id = v.Id,
                    Title = v.Title,
                    UserName = v.User != null ? v.User.Name : "Unknown",
                    Description = v.Description,
                    ThumbnailUrlOrPath = v.ThumbnailUrlOrPath,
                    Duration = v.Duration,
                    Genres = v.Genres != null && v.Genres.Any() 
                        ? string.Join(", ", v.Genres.Select(g => g.Name)) 
                        : "No genres",
                    }).ToList();

            return View(model);
        }

    }
}