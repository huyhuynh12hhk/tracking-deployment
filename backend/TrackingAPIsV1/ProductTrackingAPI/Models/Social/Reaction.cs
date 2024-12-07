using ProductTrackingAPI.Constants;
using ProductTrackingAPI.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductTrackingAPI.Models.Social
{
    public class Reaction
    {
        [Key] 
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Type { get; set; } = ReactionTypes.like.ToString();

        public bool IsActive { get; set; } = true;

        [Required]
        public string PostId { get; set; }
        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; }

        [Required]
        public string FromUserId { get; set; }
        [ForeignKey(nameof(FromUserId))]
        public UserDetail FromUser { get; set; }

        public DateTime CreatedDate { get; set; } =DateTime.Now;
        public DateTime LastModified { get; set; } = DateTime.Now;
    }
}
