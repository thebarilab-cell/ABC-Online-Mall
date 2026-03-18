using System.ComponentModel.DataAnnotations;

namespace ABCRMALLWebsite.Models
{
    public class Admin
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}