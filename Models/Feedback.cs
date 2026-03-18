using System;
using System.ComponentModel.DataAnnotations;

namespace ABCRMALLWebsite.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string CustomerName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [MinLength(10, ErrorMessage = "Message must be at least 10 characters")]
        public string Message { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        public DateTime SubmittedDate { get; set; } = DateTime.Now;
    }
}