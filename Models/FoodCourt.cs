using System;
using System.ComponentModel.DataAnnotations;

namespace ABCRMALLWebsite.Models
{
    public class FoodCourt
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Counter name is required")]
        public string CounterName { get; set; }

        public string CuisineType { get; set; }

        public string MenuItems { get; set; }

        public string ImagePath { get; set; }

        public string PriceRange { get; set; }

        public string Floor { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}