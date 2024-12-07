using System.ComponentModel.DataAnnotations;

namespace MediaAPI.DTOs
{
    public class UploadFileRequest
    {
        [Required]
        public string AuthorId { get; set; }
        public string? FilePath {  get; set; } = string.Empty;
        [Required]
        public IFormFile File { get; set; }
    }
}
