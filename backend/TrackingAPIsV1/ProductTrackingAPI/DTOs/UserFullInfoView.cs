using ProductTrackingAPI.Constants;

namespace ProductTrackingAPI.DTOs
{
    public class UserFullInfoView
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string BackgroundImage { get; set; }

        //[StringLength(100), Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        //[StringLength(100)]
        public string Address { get; set; } = string.Empty;
        public string Gender { get; set; } = UserGenders.NotSet;
        //public Dictionary<string, string> 

    }
}
