using System.ComponentModel.DataAnnotations;

namespace ProductTrackingAPI.DTOs
{
    public class UserMinInfoView
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string AvatarImage { get; set; }
        public string BackgroundImage { get; set; }

    }
}
