using System.ComponentModel.DataAnnotations;

namespace ABCRMALLWebsite.Models
{
    public class TicketBooking
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a movie")]
        public int MovieId { get; set; }

        [Range(1, 10, ErrorMessage = "You can book 1 to 10 tickets only")]
        public int NumberOfTickets { get; set; }

        [Required(ErrorMessage = "Please select show time")]
        public DateTime ShowTime { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime BookingDate { get; set; } = DateTime.Now;

        public string BookingStatus { get; set; } = "Pending";

        // Navigation property
        public Movie? Movie { get; set; }
    }
}