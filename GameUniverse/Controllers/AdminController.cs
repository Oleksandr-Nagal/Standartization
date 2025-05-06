using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using GameUniverse.Data;
using GameUniverse.Models;
using System.Linq;
using System.Threading.Tasks;

namespace GameUniverse.Controllers
{
    public class AdminController : Controller
    {
        private readonly GameUniverseContext _context;

        public AdminController(GameUniverseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        public IActionResult Users()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Login", "Account");
            }

            var currentUserId = HttpContext.Session.GetString("UserId");
            var users = _context.Users.ToList();
            ViewBag.CurrentUserId = currentUserId; // Передаємо currentUserId у ViewBag
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var currentUserId = HttpContext.Session.GetString("UserId");
            if (userId.ToString() == currentUserId || HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Users");
            }

            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                var wishlistItems = _context.Wishlist.Where(w => w.UserId == userId).ToList();
                _context.Wishlist.RemoveRange(wishlistItems);

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Users");
        }
    }
}
