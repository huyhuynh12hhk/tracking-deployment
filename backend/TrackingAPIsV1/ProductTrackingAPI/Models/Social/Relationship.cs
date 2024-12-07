using ProductTrackingAPI.Constants;
using ProductTrackingAPI.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductTrackingAPI.Models.Social
{
    public class Relationship
    {
        [Required]
        public string FromUserId { get; set; }
        [ForeignKey(nameof(FromUserId))]
        public UserDetail FromUser {  get; set; }

        [Required]
        public string ToUserId { get; set; }
        [ForeignKey(nameof(ToUserId))]
        public UserDetail ToUser {  get; set; }

        public bool IsActive { get; set; } = true;
        public string Type { get; set; } = RelationshipTypes.follow;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime LastModified { get; set; } = DateTime.Now;

    }
}
