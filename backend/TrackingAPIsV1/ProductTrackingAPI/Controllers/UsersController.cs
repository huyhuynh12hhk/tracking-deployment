using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductTrackingAPI.Constants;
using ProductTrackingAPI.DTOs;
using ProductTrackingAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductTrackingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SocialService socialService;
        private readonly AccountService accountService;

        public UsersController(SocialService socialService, AccountService accountService)
        {
            this.socialService = socialService;
            this.accountService = accountService;
        }

        [HttpGet()]
        //[Authorize]
        public async Task<IActionResult> GetUsers()
        {

            var user = await accountService.GetUsers();

            if (user == null) return NotFound();

            return Ok(BaseResponse<List<UserMinInfoView>>.Success(user));
        }


        [HttpGet("{id}")]
        //[Authorize]
        public async Task<IActionResult> GetUser([FromRoute]string id)
        {
            
            var currentUserId = User.Claims.FirstOrDefault(c=>c.Type == UserClaimTypes.userId.ToString())?.Value??"";
            var user = await accountService.FindUserInfo(u => u.Id == id, currentUserId);

            if (user == null) return NotFound();

            return Ok(BaseResponse<UserFullInfoView>.Success(user));
        }

        [HttpGet("relationships/{id}")]
        //[Authorize]
        public async Task<IActionResult> GetRelationships([FromRoute]string id)
        {


            var user = await socialService.GetFollowingUsers(id);

            if (user == null) return NotFound();

            return Ok(BaseResponse<List<UserRelationshipView>>.Success(user));
        }


        [HttpPost("relationships/{toId}")]
        [Authorize]
        public async Task<IActionResult> AddRelationships(
            [FromRoute] string toId,
            [FromQuery] string? byUser = null
        )
        {
            var userId = User.Claims.FirstOrDefault(e=>e.Type == "userId")?.Value??"";
            //var role = User.IsInRole(AccountTypes.Admin.ToString());

            //return NotFound();

            var rs = await socialService.FollowUser(userId, toId);

            if (!rs) return BadRequest();

            return NoContent();
        }
    }
}
