using BaseCore.Application.Contracts;
using BaseCore.Application.Contracts.Identity;
using BaseCore.Application.Models.Authentication;
using BaseCore.Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;


namespace BaseCore.Api.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILoggedInUserService _loggedInUserService;
        private readonly AuthCookie _authCookie;
        public AccountController(IOptions<AuthCookie> authCookie, 
            IAuthenticationService authenticationService,
            ILoggedInUserService loggedInUserService)
        {
            _authCookie = authCookie.Value;
            _authenticationService = authenticationService;
            _loggedInUserService = loggedInUserService;
        }

        [HttpPost("Login")]
        [SwaggerResponse(StatusCodes.Status200OK, "با موفقیت وارد شدید")]
        public async Task<ActionResult<BaseApiResponse<AuthenticationResponse>>> Login(AuthenticationRequest authenticationRequest)
        {
            var result = await _authenticationService.AuthenticateAsync(authenticationRequest);

            Response.Cookies.Append("AuthToken", result.Token, new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(_authCookie.DurationInMinutes)
            });

            return Ok(new BaseApiResponse<AuthenticationResponse>(result, "با موفقیت وارد شدید"));

        }


        [HttpGet("LogOut")]
        public async Task<ActionResult<BaseApiResponse<object>>> LogOut()
        {
            Response.Cookies.Delete("AuthToken", new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(_authCookie.DurationInMinutes)
            });

            return Ok(new BaseApiResponse<object>("با موفقیت خارج شدید"));

        }


        [HttpGet("ChangeRole")]
        [Authorize]
        public async Task<IActionResult> ChangeRole()
        {

            await _authenticationService.ChangeRole(_loggedInUserService.UserId);
            return NoContent();
        }


        [HttpGet("SuperAdmin")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<string>> SuperAdminText()
        {
            return Ok("Hello SuperAdmin");
        }
        
        [HttpGet("User")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<string>> UserText()
        {
            return Ok("Hello User");
        }


    }
}
