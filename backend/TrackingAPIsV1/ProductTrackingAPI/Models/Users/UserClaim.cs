using ProductTrackingAPI.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductTrackingAPI.Models.Users
{
    public class UserClaim
    {

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public UserDetail User { get; set; }

        public string ClaimId { get; set; }
        [ForeignKey(nameof(ClaimId))]
        public ClaimDetail Claim { get; set; }

    }
}
