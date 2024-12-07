using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PostTrackingAPI.Services;
using ProductTrackingAPI.Constants;
using ProductTrackingAPI.DTOs;

namespace ProductTrackingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly PostService postService;

        public PostsController(PostService postService)
        {
            this.postService = postService;
        }

        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetAllPost(string? userId = null)
        {
            var id = User.Claims.FirstOrDefault(c=>c.Type == UserClaimTypes.userId.ToString())?.Value??"";

            var posts = await postService.GetPosts(id);

            return Ok(BaseResponse<List<PostView>>.Success(posts));
        }
    }
}
