using Microsoft.AspNetCore.Mvc;
using GameUniverse.Data;
using GameUniverse.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GameUniverse.Controllers
{

    public class WishlistController : Controller
    {
        private readonly GameUniverseContext _context;
     
        public WishlistController(GameUniverseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var wishlist = _context.Wishlist
                .Include(w => w.Game)
                .Where(w => w.UserId == int.Parse(userId))
                .ToList();
            return View(wishlist);
        }

        public async Task<IActionResult> Add(int gameId)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var existingItem = _context.Wishlist.FirstOrDefault(w => w.UserId == int.Parse(userId) && w.GameId == gameId);
            if (existingItem == null)
            {
                _context.Wishlist.Add(new Wishlist { UserId = int.Parse(userId), GameId = gameId });
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Catalog", new { id = gameId });
        }

        public async Task<IActionResult> Remove(int gameId)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var wishlistItem = _context.Wishlist.FirstOrDefault(w => w.UserId == int.Parse(userId) && w.GameId == gameId);
            if (wishlistItem != null)
            {
                _context.Wishlist.Remove(wishlistItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Catalog", new { id = gameId });
        }

        public IActionResult AdminIndex()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Login", "Account");
            }

            var wishlists = _context.Wishlist
                .Include(w => w.User)
                .Include(w => w.Game)
                .ToList();
            return View(wishlists);
        }


    }
}

