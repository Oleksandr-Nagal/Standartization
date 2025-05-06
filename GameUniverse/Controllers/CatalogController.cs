using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using GameUniverse.Data;
using GameUniverse.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameUniverse.Controllers
{
    public class CatalogController : Controller
    {
        private readonly GameUniverseContext _context;

        public CatalogController(GameUniverseContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchString, string genre, string platform)
        {
            var games = _context.Games.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                games = games.Where(g => g.Title.Contains(searchString) || g.Description.Contains(searchString));
            }
 
            if (!string.IsNullOrEmpty(genre))
            {
                games = games.Where(g => (g.Genre ?? "").Equals(genre));
            }

            if (!string.IsNullOrEmpty(platform))
            {
                games = games.Where(g => (g.Platform ?? "").Equals(platform));
            }

            ViewBag.Genres = new SelectList(_context.Genres.OrderBy(g => g.Name), "Name", "Name");
            ViewBag.Platforms = new SelectList(_context.Platforms.OrderBy(p => p.Name), "Name", "Name");

            ViewData["searchString"] = searchString;
            ViewData["selectedGenre"] = genre;
            ViewData["selectedPlatform"] = platform;

            return View(games.ToList());
        }





        public IActionResult Details(int id)
        {
            var game = _context.Games.FirstOrDefault(g => g.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                var isInWishlist = _context.Wishlist.Any(w => w.UserId == int.Parse(userId) && w.GameId == id);
                ViewBag.IsInWishlist = isInWishlist;
            }

            return View(game);
        }

        [HttpGet]
        public IActionResult AddGame()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Index");
            }

            ViewBag.Platforms = _context.Platforms.Select(p => p.Name).ToList();
            ViewBag.Genres = _context.Genres.Select(g => g.Name).ToList();
            ViewBag.Publishers = _context.Games.Select(g => g.Publisher).Distinct().ToList();
            ViewBag.Developers = _context.Games.Select(g => g.Developer).Distinct().ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddGame(Game game)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Index");
            }

            if (!string.IsNullOrEmpty(game.ImageUrl))
            {
                game.ImageUrl = "/images/" + game.ImageUrl;
            }

            if (ModelState.IsValid)
            {
                _context.Games.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Platforms = _context.Platforms.Select(p => p.Name).ToList();
            ViewBag.Genres = _context.Genres.Select(g => g.Name).ToList();
            ViewBag.Publishers = _context.Games.Select(g => g.Publisher).Distinct().ToList();
            ViewBag.Developers = _context.Games.Select(g => g.Developer).Distinct().ToList();
            return View(game);
        }

        [HttpGet]
        public async Task<IActionResult> EditGame(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Index");
            }

            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            ViewBag.Platforms = _context.Platforms.Select(p => p.Name).ToList();
            ViewBag.Genres = _context.Genres.Select(g => g.Name).ToList();
            ViewBag.Publishers = _context.Games.Select(g => g.Publisher).Distinct().ToList();
            ViewBag.Developers = _context.Games.Select(g => g.Developer).Distinct().ToList();
            return View(game);
        }

        [HttpPost]
        public async Task<IActionResult> EditGame(Game game)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Index");
            }

            if (!string.IsNullOrEmpty(game.ImageUrl))
            {
                game.ImageUrl = "/images/" + game.ImageUrl;
            }

            if (ModelState.IsValid)
            {
                _context.Update(game);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Platforms = _context.Platforms.Select(p => p.Name).ToList();
            ViewBag.Genres = _context.Genres.Select(g => g.Name).ToList();
            ViewBag.Publishers = _context.Games.Select(g => g.Publisher).Distinct().ToList();
            ViewBag.Developers = _context.Games.Select(g => g.Developer).Distinct().ToList();
            return View(game);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGame(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Index");
            }

            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
