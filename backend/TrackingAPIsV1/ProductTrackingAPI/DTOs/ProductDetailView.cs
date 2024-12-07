using ProductTrackingAPI.Models.Products;
using ProductTrackingAPI.Models.Users;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProductTrackingAPI.DTOs
{
    public class ProductDetailView
    {
        public string Id { get; set; }
        
        public string Name { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public string Description { get; set; } 
        
        public List<string> GalleryPaths
        {
            get;
            set;
        }

        public UserMinInfoView Supplier { get; set; }

        public List<ProductView> OriginProducts { get; set; }
        //public List<ProductView> Products { get; set; }
    }
}
