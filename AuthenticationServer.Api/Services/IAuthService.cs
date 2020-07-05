using AuthenticationServer.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthenticationServer.Api.Services
{
    public interface IAuthService
    {
        string GetToken(ClaimsIdentity identity);
        Task<ClaimsIdentity> GetIdentityAsync(Login request);
    }
}