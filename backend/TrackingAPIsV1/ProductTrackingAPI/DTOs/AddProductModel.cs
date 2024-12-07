using ProductTrackingAPI.Models.Users;

namespace ProductTrackingAPI.DTOs
{
    public class AddProductModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public List<string> GalleryPaths { get; set; }
        public string SupplierId { get; set; }
        public Dictionary<string, string> OriginKeys { get; set; }
    }
}
