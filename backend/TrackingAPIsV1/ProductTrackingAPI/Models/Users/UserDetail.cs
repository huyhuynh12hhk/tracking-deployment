using ProductTrackingAPI.Constants;
using System.ComponentModel.DataAnnotations;

namespace ProductTrackingAPI.Models.Users
{
    public class UserDetail
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }
        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        public string AvatarImage { get; set; } = string.Empty;
        public string BackgroundImage { get; set; } = string.Empty;

        [StringLength(20), Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [StringLength(100)]
        public string Address { get; set; } = string.Empty;
        [StringLength(10)]
        public string Gender { get; set; } = UserGenders.NotSet;
        [StringLength(5000)]
        public string Description { get; set; } = string.Empty;


    }
}
