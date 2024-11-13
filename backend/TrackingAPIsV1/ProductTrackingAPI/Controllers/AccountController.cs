using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductTrackingAPI.Constants;
using ProductTrackingAPI.DTOs;
using ProductTrackingAPI.Services;

namespace ProductTrackingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService accountService;

        public AccountController(AccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpGet("info")]
        [Authorize]
        public async Task<IActionResult> GetInfo()
        {
            var id = User.Claims.FirstOrDefault(c => c.Type == UserClaimTypes.userId.ToString())!.Value??"";

            var user = await accountService.FindUserInfo(u=>u.Id == id);

            if (user == null) return Unauthorized();

            return Ok(BaseResponse<UserFullInfoView>.Success(user));
        }

        [HttpPost("token")]
        public async Task<IActionResult> Login([FromBody]LoginModel request)
        {
            if(await accountService.CheckUserCredentials(request.Email, request.Password))
            {
                var result = await accountService.ProductUserToken(request.Email);

                return Ok(BaseResponse<UserMinInfoView>.Success(result));
            }

            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Regsit([FromBody] RegisterModel request)
        {
            var result = await accountService.CreateNewUserProfile(request.Email, request.Password, request.FullName);

            if (!result)
            {
                BadRequest(BaseResponse.Fail("Invalid User Data"));
            }

            return NoContent();

            //return Ok(BaseResponse<UserMinInfoView>
            //    .Success(await accountService.ProductUserToken(request.Email)));
            

        }
    }
}
