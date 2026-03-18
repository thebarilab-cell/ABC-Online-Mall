using System;
using System.ComponentModel.DataAnnotations;

namespace ABCRMALLWebsite.Models
{
    public class Shop
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Shop name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public string ItemsList { get; set; }

        public string Floor { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}