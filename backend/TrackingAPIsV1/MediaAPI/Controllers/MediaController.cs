using MediaAPI.Data;
using MediaAPI.Data.Models;
using MediaAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        
        private readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "MediaStorage");
        private readonly MediaStorageContext context;

        public MediaController(MediaStorageContext context)
        {
            if (!Directory.Exists(_storagePath))
                Directory.CreateDirectory(_storagePath);
            this.context = context;
        }



        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm]UploadFileRequest request)
        {
            if (request.File.Length == 0)
                return BadRequest("File cannot empty.");

            Console.WriteLine("Name: "+request.File.FileName);
            Console.WriteLine("Type: "+request.File.ContentType);

            var item = new MediaObject
            {
                MediaType = request.File.ContentType,
                AuthorId = request.AuthorId,
                Name = request.File.FileName,
                Path = request.FilePath??""
            };

            using(var ms = new MemoryStream())
            {
                request.File.CopyTo(ms);
                item.Data = ms.ToArray();
            }

            context.MediaItems.Add(item);

            await context.SaveChangesAsync();
            

            return Ok(new { Key = item.Id, Path = $"/api/media/{item.AuthorId}/{item.Id}" });
        }

        

        [HttpGet("{fileId}")]
        public async Task<IActionResult> Get(
            //[FromRoute]string authorId, 
            [FromRoute]string fileId)
        {
            
            var src = await context.MediaItems.FirstOrDefaultAsync(e=>e.Id == fileId);

            if (src == null)
                return NotFound();

            //var contentType = "application/octet-stream";

            return File(src.Data, src.MediaType, src.Name);
        }

        [HttpDelete("{authorId}/{fileName}")]
        public IActionResult Delete([FromRoute] string authorId, [FromRoute] string fileName)
        {
            
            return NotFound();
        }

        [HttpGet("list")]
        public IActionResult List()
        {
            var files = Directory.GetFiles(_storagePath)
                                 .Select(Path.GetFileName)
                                 .ToList();

            return Ok(files);
        }

        /*

        [HttpGet("{authorId}/{fileName}")]
        public IActionResult Download(string authorId, string fileName)
        {
            var filePath = Path.Combine(_storagePath, fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found.");

            var contentType = "application/octet-stream";
            var fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, contentType, fileName);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is missing or empty.");

            var filePath = Path.Combine(_storagePath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { FileName = file.FileName, Path = $"/api/media/download/{file.FileName}" });
        }
        */
    }
}
