using System.Security.Cryptography;
using System.Text;
using GameUniverse.Data;
using GameUniverse.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameUniverse.Controllers
{
    public class AccountController : Controller
    {
        private readonly GameUniverseContext _context;

        public AccountController(GameUniverseContext context)
        {
            _context = context;
        }

        public IActionResult Login() => View();
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(string username, string email, string password)
        {
            if (_context.Users.Any(u => u.Email == email))
            {
                ViewBag.Error = "Цей email вже зареєстрований";
                return View();
            }

            if (_context.Users.Any(u => u.Username == username))
            {
                ViewBag.Error = "Це ім'я користувача вже зайняте";
                return View();
            }

            if (username.Length < 4)
            {
                ViewBag.Error = "Ім'я користувача повинно містити мінімум 4 символи";
                return View();
            }

            if (password.Length < 8)
            {
                ViewBag.Error = "Пароль повинен містити мінімум 8 символів";
                return View();
            }

            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = HashPassword(password),
                IsAdmin = email == "admin@example.com" // Автоматично робимо цього користувача адміном
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Login");
        }


        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Login(string identifier, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == identifier || u.Username == identifier);
            if (user == null || user.PasswordHash != HashPassword(password))
            {
                ViewBag.Error = "Неправильний email/ім'я користувача або пароль";
                return View();
            }

            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("IsAdmin", user.IsAdmin ? "true" : "false");

            return user.IsAdmin ? RedirectToAction("Index", "Admin") : RedirectToAction("Index", "Home");
        }



        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        public IActionResult Profile()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == userId);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            return View(user);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
