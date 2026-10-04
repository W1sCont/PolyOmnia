using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PolyOmnia.Models;
using PolyOmnia.Repository;
using System.Security.Claims;

namespace PolyOmnia.Controllers
{
    public class AdminController(IUserRepository userRepository, IPasswordHasher passwordHasher) : Controller
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

            if (!_userRepository.IsLoginUniqueAsync(model.Login).Result)
            {
                ModelState.AddModelError("Login", "Login already exists.");
                return View(model);
            }

            if (_userRepository.GetByEmailAsync(model.Email).Result != null)
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
    }
}
