namespace ProductTrackingAPI.DTOs
{
    public class ProductView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public UserMinInfoView Supplier { get; set; }
        public string AvatarImage {  get; set; }
        public List<string> GalleryPaths { get; set; } = new List<string>();
    }
}
