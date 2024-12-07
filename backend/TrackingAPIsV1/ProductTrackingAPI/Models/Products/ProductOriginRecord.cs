using ProductTrackingAPI.Models.Products;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProductTrackingAPI.Models.Products
{
    public class ProductOriginRecord
    {
        

        [Required]
        public string FromProductId { get; set; }
        [ForeignKey(nameof(FromProductId))]
        public ProductDetail FromProduct { get; set; }


        [Required]
        public string ToProductId { get; set; }
        [ForeignKey(nameof(ToProductId))]
        public ProductDetail ToProduct { get; set; }

        public string Note { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime LastModified { get; set; } = DateTime.Now;
    }
}
