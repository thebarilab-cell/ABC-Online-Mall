using System;
using System.ComponentModel.DataAnnotations;

namespace ABCRMALLWebsite.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Movie title is required")]
        public string Title { get; set; }

        public string Genre { get; set; }

        public string Duration { get; set; }

        public string ShowTimings { get; set; }

        [Range(1, 500, ErrorMessage = "Seats must be between 1 and 500")]
        public int TotalSeats { get; set; }

        public int AvailableSeats { get; set; }

        public string ImagePath { get; set; }

        [Range(100, 5000, ErrorMessage = "Ticket price must be between 100 and 5000")]
        public decimal TicketPrice { get; set; }

        // Add this new property for movie poster URL
        [Url(ErrorMessage = "Please enter a valid URL for the movie poster")]
        [Display(Name = "Movie Poster URL")]
        public string PosterUrl { get; set; }
    }
}