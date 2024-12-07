using ProductTrackingAPI.Constants;
using System.ComponentModel.DataAnnotations;

namespace ProductTrackingAPI.Models.Users
{
    public class ClaimDetail
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Key { get; set; } = UserClaimTypes.role.ToString();
        [Required]
        public string Value { get; set; }
        public string Provider { get; set; } = ProviderCatalogs.None.ToString();

    }
}
