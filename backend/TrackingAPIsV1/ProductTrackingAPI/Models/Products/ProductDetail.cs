using ProductTrackingAPI.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ProductTrackingAPI.Models.Products
{
    public class ProductDetail
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; } = 0;
        public double Discount { get; set; } = 0;
        public string Description { get; set; } = string.Empty;
        private string _galleryString = "";
        public string GalleryString { get => _galleryString; set => _galleryString = value; }
        [NotMapped]
        public List<string> GalleryPaths
        {
            get {
                return _galleryString.Split(",").ToList();
            }
            set {
                _galleryString = string.Join(",", value);
            }
        }

        
        [Required]
        public string SupplierId { get; set; }
        [ForeignKey(nameof(SupplierId))]
        public UserDetail Supplier { get; set; }

        public IEnumerable<ProductOriginRecord> Origins { get; set; }
        public IEnumerable<ProductOriginRecord> Products { get; set; }

    }
}
