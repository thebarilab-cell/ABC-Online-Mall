using System;
using System.ComponentModel.DataAnnotations;

namespace ABCRMALLWebsite.Models
{
    public class Gallery
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public string ImagePath { get; set; }

        public string Category { get; set; } // "Shops", "Food", "Events", etc.

        public DateTime UploadDate { get; set; } = DateTime.Now;

        public string UploadedBy { get; set; } = "Admin";
    }
}