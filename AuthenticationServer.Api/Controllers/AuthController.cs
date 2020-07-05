using AuthenticationServer.Api.Common.CustomException;
using AuthenticationServer.Api.Models;
using AuthenticationServer.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AuthenticationServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Login request)
        {
            try
            {
                var identity = await _authService.GetIdentityAsync(request);
                var encodedJwt = _authService.GetToken(identity);

                var response = new
                {
                    access_token = encodedJwt,
                    username = request.Email
                };

                return Json(response);
            }
            catch (LoginException ex)
            {
                return Json(new { Error = true, ex.Message });
            }
            catch(Exception)
            {
                return Json(new { Error = true, Message = "An error occurred on the server" });
            }
        }
    }
}
