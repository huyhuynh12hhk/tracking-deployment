using ProductTrackingAPI.Constants;

namespace ProductTrackingAPI.DTOs
{
    public class UserFullInfoView
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string AvatarImage { get; set; }
        public string BackgroundImage { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string Relationship { get; set; } = string.Empty;

    }
}
