using System.ComponentModel.DataAnnotations;

namespace ABCRMALLWebsite.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required]
        public string MallName { get; set; }

        [Required]
        public string Address { get; set; }

        [Phone]
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string OperatingHours { get; set; }

        public string GoogleMapLink { get; set; }
    }
}