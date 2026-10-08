using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolyOmnia.Models;
using PolyOmnia.Repository;
using System.Security.Claims;

namespace PolyOmnia.Controllers
{
    public class AccountController(IUserRepository userRepository, IPasswordHasher passwordHasher) : Controller
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
                return View(model);
            }

            if (!await _userRepository.IsLoginUniqueAsync(model.Login))
            {
                ModelState.AddModelError("Login", "Login already exists.");
                return View(model);
            }

            if (await _userRepository.GetByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError("Email", "Користувач з таким Email вже існує.");
                return View(model);
            }

            User? newUser = await _userRepository.CreateUserAsync(model);
            if (newUser == null)
            {
                return BadRequest("Дані користувача некоректні.");
            }
            await _userRepository.AddAsync(newUser);

            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            User? user = await _userRepository.GetByLoginOrEmailAsync(model.EmailOrLogin);
            if (user == null)
            {
                ModelState.AddModelError("EmailOrLogin", "Невірний логін/Email або пароль");
                return View(model);
            }

            bool isPasswordValid = _passwordHasher.VerifyPassword(
                model.Password,
                user.PasswordHash,
                user.PasswordSalt
            );

            if (!isPasswordValid)
            {
                ModelState.AddModelError("EmailOrLogin", "Невірний логін/Email або пароль");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true // Зберігати сесію після закриття браузера
            };

            // Записуємо авторизаційну куку
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            // 1. Отримуємо Claim з ID залогіненого користувача
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return RedirectToAction("Login");
            }

            // 2. Отримуємо юзера з БД
            var user = await _userRepository.GetUserWithVideosAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // 3. Формуємо ViewModel з маппінгом відео
            var model = new UserProfileViewModel
            {
                UserId = user.Id,
                Username = user.Login,
                Email = user.Email,
                // RegisteredAt = user.CreatedAt, // Переконайся, що це поле є в сутності User

                UserVideos = user.Videos.Select(v => new UserVideoItemViewModel
                {
                    Id = v.Id,
                    Title = v.Title,
                    Description = v.Description,
                    ThumbnailUrlOrPath = v.ThumbnailUrlOrPath,
                    ViewCount = v.ViewCount,
                    CreatedAt = v.CreatedAt,
                    AllowDownload = v.AllowDownload
                    //PrimaryGenreName = v.Genres?.Name // Якщо є зв'язок із Genre
                }).ToList()
            };

            return View(model);
        }
    }
}
