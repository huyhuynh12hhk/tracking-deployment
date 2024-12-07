using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace MediaAPI.Data.Models
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class MediaObject
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string AuthorId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Path { get; set; } =string.Empty;
        [Required]
        public string MediaType { get; set; }
        [Required]
        public byte[] Data { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
