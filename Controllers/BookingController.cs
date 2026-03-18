using ABCRMALLWebsite.Data;
using ABCRMALLWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ABCRMALLWebsite.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ BOOK TICKET PAGE
        public async Task<IActionResult> BookTicket(int movieId)
        {
            var movie = await _context.Movies.FindAsync(movieId);
            if (movie == null)
            {
                TempData["ErrorMessage"] = "Movie not found!";
                return RedirectToAction("Movies", "Home");
            }

            if (movie.AvailableSeats <= 0)
            {
                TempData["ErrorMessage"] = "Sorry, this show is house full!";
                return RedirectToAction("Movies", "Home");
            }

            var viewModel = new BookingViewModel
            {
                MovieId = movie.Id,
                MovieTitle = movie.Title,
                ShowTimings = movie.ShowTimings ?? "",
                TicketPrice = movie.TicketPrice,
                AvailableSeats = movie.AvailableSeats,
                TotalSeats = movie.TotalSeats
            };

            return View(viewModel);
        }

        // ✅ CONFIRM BOOKING
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmBooking(BookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("BookTicket", model);
            }

            var movie = await _context.Movies.FindAsync(model.MovieId);
            if (movie == null)
            {
                TempData["ErrorMessage"] = "Movie not found!";
                return RedirectToAction("Movies", "Home");
            }

            // Check seat availability
            if (movie.AvailableSeats < model.NumberOfTickets)
            {
                ModelState.AddModelError("NumberOfTickets", $"Only {movie.AvailableSeats} seats available!");
                return View("BookTicket", model);
            }

            // Calculate total amount
            var totalAmount = model.NumberOfTickets * movie.TicketPrice;

            // Create booking
            var booking = new TicketBooking
            {
                CustomerName = model.CustomerName,
                Email = model.Email,
                Phone = model.Phone,
                MovieId = model.MovieId,
                NumberOfTickets = model.NumberOfTickets,
                ShowTime = model.ShowTime,
                TotalAmount = totalAmount,
                //PaymentMethod = model.PaymentMethod,
                //CardNumber = model.PaymentMethod == "Card" ? model.CardNumber : null,
                BookingDate = DateTime.Now,
                BookingStatus = "Confirmed",
                
            };

            // Update available seats
            movie.AvailableSeats -= model.NumberOfTickets;

            try
            {
                _context.TicketBookings.Add(booking);
                await _context.SaveChangesAsync();

                // Store booking ID for success page
                HttpContext.Session.SetInt32("LastBookingId", booking.Id);
                TempData["BookingSuccess"] = "true";

                return RedirectToAction("BookingSuccess", new { bookingId = booking.Id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Booking failed! Please try again.";
                return View("BookTicket", model);
            }
        }

        // ✅ BOOKING SUCCESS PAGE
        public async Task<IActionResult> BookingSuccess(int bookingId)
        {
            var booking = await _context.TicketBookings
                .Include(b => b.Movie)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
            {
                TempData["ErrorMessage"] = "Booking not found!";
                return RedirectToAction("Movies", "Home");
            }

            return View(booking);
        }

        // ✅ VIEW MY BOOKINGS
        public async Task<IActionResult> MyBookings(string email, string phone)
        {
            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(phone))
            {
                return View(new List<TicketBooking>());
            }

            var bookings = await _context.TicketBookings
                .Include(b => b.Movie)
                .Where(b => b.Email == email || b.Phone == phone)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            ViewBag.SearchEmail = email;
            ViewBag.SearchPhone = phone;

            return View(bookings);
        }

        // ✅ CANCEL BOOKING
        [HttpPost]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            var booking = await _context.TicketBookings
                .Include(b => b.Movie)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
            {
                return Json(new { success = false, message = "Booking not found!" });
            }

            if (booking.BookingStatus == "Cancelled")
            {
                return Json(new { success = false, message = "Booking already cancelled!" });
            }

            // Check if show time is more than 2 hours away
            if (booking.ShowTime <= DateTime.Now.AddHours(2))
            {
                return Json(new { success = false, message = "Cannot cancel within 2 hours of show time!" });
            }

            // Refund seats
            booking.Movie.AvailableSeats += booking.NumberOfTickets;
            booking.BookingStatus = "Cancelled";
            //booking.CardNumber = "REFUNDED"; // Mark for refund

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Booking cancelled successfully!" });
        }
    }

    // ✅ VIEW MODEL FOR BOOKING
    public class BookingViewModel
    {
        public int MovieId { get; set; }

        [Required(ErrorMessage = "Please enter your name")]
        [Display(Name = "Full Name")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your phone number")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; }



        [Required(ErrorMessage = "Please select number of tickets")]
        [Range(1, 10, ErrorMessage = "You can book 1 to 10 tickets only")]
        [Display(Name = "Number of Tickets")]
        public int NumberOfTickets { get; set; }

        [Required(ErrorMessage = "Please select show time")]
        [Display(Name = "Show Time")]
        public DateTime ShowTime { get; set; }

        // Movie details for display
        public string MovieTitle { get; set; }
        public string ShowTimings { get; set; } = "";
        public decimal TicketPrice { get; set; }
        public int AvailableSeats { get; set; }
        public int TotalSeats { get; set; }
    }
}