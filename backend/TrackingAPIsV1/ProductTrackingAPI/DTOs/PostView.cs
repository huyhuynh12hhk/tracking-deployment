using ProductTrackingAPI.Models.Users;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProductTrackingAPI.DTOs
{
    public class PostView
    {

        public string Id { get; set; }
        public string Content { get; set; }
        public UserMinInfoView Author { get; set; }
        public List<string> AttachmentPaths
        {
            get;
            set;
        }
    }
}
