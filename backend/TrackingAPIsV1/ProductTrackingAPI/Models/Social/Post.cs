using ProductTrackingAPI.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductTrackingAPI.Models.Social
{
    public class Post
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Content { get; set; }



        [Required]
        public string AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public UserDetail Author { get; set; }


        private string _attachmentString = "";
        public string AttachmentString { get => _attachmentString; set => _attachmentString = value; }
        [NotMapped]
        public List<string> AttachmentPaths
        {
            get
            {
                return _attachmentString.Split(",").ToList();
            }
            set
            {
                _attachmentString = string.Join(",", value);
            }
        }




        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime LastModified { get; set; } = DateTime.Now;
        public DateTime? DeletedDate { get; set; } = null;
        public ICollection<Reaction> Reactions { get; set; }
    }
}
