using ProductTrackingAPI.Models.Users;

namespace ProductTrackingAPI.DTOs
{
    public class EditProductInfoModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public List<string> GalleryPaths { get; set; }
        public UserDetail Supplier {  get; set; }
    }
}
