using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ABCRMALLWebsite.Data;
using ABCRMALLWebsite.Models;

namespace ABCRMALLWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Show Add Movie form
        [HttpGet]
        public IActionResult AddMovie()
        {
            return View();
        }

        // POST: Handle form submission
        [HttpPost]
        public async Task<IActionResult> AddMovie(Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Movie added successfully!";
                return RedirectToAction("Movies"); // ya "ManageMovies" agar tumhare paas woh action hai
            }
            return View(movie); // agar validation fail ho to wapas form dikhao
        }


        public async Task<IActionResult> Index()
        {
            // Latest shops and movies home page par show karenge
            var latestShops = await _context.Shops.Take(4).ToListAsync();
            var latestMovies = await _context.Movies.Take(3).ToListAsync();

            ViewBag.LatestShops = latestShops;
            ViewBag.LatestMovies = latestMovies;
            ViewBag.WelcomeMessage = "Welcome to ABC Mall - Mumbai's Premier Shopping Destination";

            return View();
        }

        public async Task<IActionResult> Shopping()
        {
            var shops = await _context.Shops.ToListAsync();
            return View(shops);
        }

        public async Task<IActionResult> Movies()
        {
            var movies = await _context.Movies.ToListAsync();
            return View(movies);
        }

        public async Task<IActionResult> FoodCourt()
        {
            var foodCounters = await _context.FoodCourts.ToListAsync();
            return View(foodCounters);
        }

        public async Task<IActionResult> Gallery()
        {
            var galleryImages = await _context.Galleries.ToListAsync();
            return View(galleryImages);
        }

        public IActionResult Contact()
        {
            var contactInfo = _context.Contacts.FirstOrDefault();
            return View(contactInfo);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitFeedback(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thank you for your feedback!";
                return RedirectToAction("Contact");
            }
            return View("Contact", feedback);
        }

        public async Task<IActionResult> Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return View(new SearchViewModel());
            }

            var shops = await _context.Shops
                .Where(s => s.Name.Contains(searchTerm) || s.ItemsList.Contains(searchTerm))
                .ToListAsync();

            var foodItems = await _context.FoodCourts
                .Where(f => f.CounterName.Contains(searchTerm) || f.MenuItems.Contains(searchTerm))
                .ToListAsync();

            var movies = await _context.Movies
                .Where(m => m.Title.Contains(searchTerm) || m.Genre.Contains(searchTerm))
                .ToListAsync();

            var model = new SearchViewModel
            {
                Shops = shops,
                FoodItems = foodItems,
                Movies = movies,
                SearchTerm = searchTerm
            };

            return View(model);
        }
    }

    public class SearchViewModel
    {
        public List<Shop> Shops { get; set; } = new();
        public List<FoodCourt> FoodItems { get; set; } = new();
        public List<Movie> Movies { get; set; } = new();
        public string SearchTerm { get; set; }
    }
}