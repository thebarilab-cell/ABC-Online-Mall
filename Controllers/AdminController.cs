using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ABCRMALLWebsite.Data;
using ABCRMALLWebsite.Models;

namespace ABCRMALLWebsite.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ ADMIN LOGIN PAGE
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var admin = await _context.Admins
                .FirstOrDefaultAsync(a => a.Username == username && a.Password == password);

            if (admin != null)
            {
                // Simple session management
                HttpContext.Session.SetString("AdminLoggedIn", "true");
                HttpContext.Session.SetString("AdminUsername", admin.Username);

                TempData["SuccessMessage"] = "Login successful!";
                return RedirectToAction("Dashboard");
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid username or password!";
                return View();
            }
        }

        // ✅ ADMIN DASHBOARD
        public IActionResult Dashboard()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login");
            }

            var dashboardStats = new AdminDashboardViewModel
            {
                TotalShops = _context.Shops.Count(),
                TotalMovies = _context.Movies.Count(),
                TotalBookings = _context.TicketBookings.Count(),
                TotalFeedbacks = _context.Feedbacks.Count()
            };

            return View(dashboardStats);
        }

        // ✅ SHOPS MANAGEMENT
        public async Task<IActionResult> ManageShops()
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");

            var shops = await _context.Shops.ToListAsync();
            return View(shops);
        }

        public IActionResult AddShop()
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");
            return View();
        }

        // ✅ VIEW GALLERY IMAGES (Admin can manage gallery)
        public async Task<IActionResult> ManageGallery()
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");

            var galleryImages = await _context.Galleries.ToListAsync();
            return View(galleryImages);
        }

        public IActionResult AddGalleryImage()
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddGalleryImage(Gallery gallery)
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");

            if (ModelState.IsValid)
            {
                _context.Galleries.Add(gallery);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Gallery image added successfully!";
                return RedirectToAction("ManageGallery");
            }
            return View(gallery);
        }

        // ✅ MANAGE FOOD COURTS
        public async Task<IActionResult> ManageFoodCourts()
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");

            var foodCourts = await _context.FoodCourts.ToListAsync();
            return View(foodCourts);
        }

        public IActionResult AddFoodCourt()
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFoodCourt(FoodCourt foodCourt)
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");

            if (ModelState.IsValid)
            {
                _context.FoodCourts.Add(foodCourt);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Food court added successfully!";
                return RedirectToAction("ManageFoodCourts");
            }
            return View(foodCourt);
        }

        [HttpPost]
        public async Task<IActionResult> AddShop(Shop shop)
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");

            if (ModelState.IsValid)
            {
                _context.Shops.Add(shop);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Shop added successfully!";
                return RedirectToAction("ManageShops");
            }
            return View(shop);
        }

        public async Task<IActionResult> EditShop(int id)
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");

            var shop = await _context.Shops.FindAsync(id);
            if (shop == null) return NotFound();

            return View(shop);
        }

        [HttpPost]
        public async Task<IActionResult> EditShop(Shop shop)
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");

            if (ModelState.IsValid)
            {
                _context.Shops.Update(shop);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Shop updated successfully!";
                return RedirectToAction("ManageShops");
            }
            return View(shop);
        }

        public async Task<IActionResult> DeleteShop(int id)
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");

            var shop = await _context.Shops.FindAsync(id);
            if (shop == null) return NotFound();

            _context.Shops.Remove(shop);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Shop deleted successfully!";
            return RedirectToAction("ManageShops");
        }

        // ✅ MOVIES MANAGEMENT
        public async Task<IActionResult> ManageMovies()
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");

            var movies = await _context.Movies.ToListAsync();
            return View(movies);
        }

        public IActionResult AddMovie()
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddMovie(Movie movie)
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");

            if (ModelState.IsValid)
            {
                movie.AvailableSeats = movie.TotalSeats; // Initially all seats available
                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Movie added successfully!";
                return RedirectToAction("ManageMovies");
            }
            return View(movie);
        }

        public async Task<IActionResult> EditMovie(int id)
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return NotFound();

            return View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> EditMovie(Movie movie)
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");

            if (ModelState.IsValid)
            {
                _context.Movies.Update(movie);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Movie updated successfully!";
                return RedirectToAction("ManageMovies");
            }
            return View(movie);
        }

        public async Task<IActionResult> DeleteMovie(int id)
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return NotFound();

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Movie deleted successfully!";
            return RedirectToAction("ManageMovies");
        }

        // ✅ VIEW FEEDBACKS
        public async Task<IActionResult> ViewFeedbacks()
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");

            var feedbacks = await _context.Feedbacks.OrderByDescending(f => f.SubmittedDate).ToListAsync();
            return View(feedbacks);
        }

        // ✅ FOOD COURTS CRUD METHODS
        public async Task<IActionResult> EditFoodCourt(int id)
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");

            var foodCourt = await _context.FoodCourts.FindAsync(id);
            if (foodCourt == null) return NotFound();

            return View(foodCourt);
        }

        [HttpPost]
        public async Task<IActionResult> EditFoodCourt(FoodCourt foodCourt)
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");

            if (ModelState.IsValid)
            {
                _context.FoodCourts.Update(foodCourt);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Food court updated successfully!";
                return RedirectToAction("ManageFoodCourts");
            }
            return View(foodCourt);
        }

        public async Task<IActionResult> DeleteFoodCourt(int id)
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");

            var foodCourt = await _context.FoodCourts.FindAsync(id);
            if (foodCourt == null) return NotFound();

            _context.FoodCourts.Remove(foodCourt);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Food court deleted successfully!";
            return RedirectToAction("ManageFoodCourts");
        }

        // ✅ GALLERY CRUD METHODS
        public async Task<IActionResult> EditGalleryImage(int id)
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");

            var galleryImage = await _context.Galleries.FindAsync(id);
            if (galleryImage == null) return NotFound();

            return View(galleryImage);
        }

        [HttpPost]
        public async Task<IActionResult> EditGalleryImage(Gallery gallery)
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");

            if (ModelState.IsValid)
            {
                _context.Galleries.Update(gallery);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Gallery image updated successfully!";
                return RedirectToAction("ManageGallery");
            }
            return View(gallery);
        }

        public async Task<IActionResult> DeleteGalleryImage(int id)
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");

            var galleryImage = await _context.Galleries.FindAsync(id);
            if (galleryImage == null) return NotFound();

            _context.Galleries.Remove(galleryImage);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Gallery image deleted successfully!";
            return RedirectToAction("ManageGallery");
        }

        // ✅ VIEW BOOKINGS
        public async Task<IActionResult> ViewBookings()
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login");

            var bookings = await _context.TicketBookings
                .Include(b => b.Movie)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            return View(bookings);
        }


        // ✅ LOGOUT
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Logged out successfully!";
            return RedirectToAction("Login");
        }

        // ✅ HELPER METHOD FOR ADMIN CHECK
        private bool IsAdminLoggedIn()
        {
            return HttpContext.Session.GetString("AdminLoggedIn") == "true";
        }
    }

    public class AdminDashboardViewModel
    {
        public int TotalShops { get; set; }
        public int TotalMovies { get; set; }
        public int TotalBookings { get; set; }
        public int TotalFeedbacks { get; set; }
    }
}