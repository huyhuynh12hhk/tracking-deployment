using System.ComponentModel.DataAnnotations;

namespace ProductTrackingAPI.DTOs
{
    public class RegisterModel
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
