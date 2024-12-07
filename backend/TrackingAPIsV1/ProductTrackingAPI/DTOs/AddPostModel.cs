using System.ComponentModel.DataAnnotations;

namespace ProductTrackingAPI.DTOs
{
    public class AddPostModel
    {
        public string Content { get; set; }

        public string AuthorId { get; set; }
        public List<IFormFile> AttachItems { get; set; }
    }
}
