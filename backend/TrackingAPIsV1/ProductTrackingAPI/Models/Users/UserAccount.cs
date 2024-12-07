using ProductTrackingAPI.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductTrackingAPI.Models.Users
{
    public class UserAccount
    {
        [Key]
        [StringLength(100)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [StringLength(100)]
        public string Key { get; set; }
        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        public bool IsConfirmed { get; set; } = false;
        [Required]
        [StringLength(20)]
        public string AccountType { get; set; } = AccountTypes.Member.ToString();
        public string Provider { get; set; } = ProviderCatalogs.None.ToString();
        [Required]
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public UserDetail UserDetail { get; set; }


    }
}
