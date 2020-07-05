using AuthenticationServer.Api.Models;
using AuthenticationServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using AuthenticationServer.Api.Common.CustomException;

namespace AuthenticationServer.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthenticationContext authenticationContext;

        public AuthService(AuthenticationContext context)
        {
            authenticationContext = context;
        }

        public async Task<ClaimsIdentity> GetIdentityAsync(Login request)
        {
            var user = await authenticationContext.Users
                .FirstOrDefaultAsync(user => user.Email == request.Email);

            if (user == null)
            {
                throw new LoginException($"{request.Email} not found");
            }

            if(!CompareHash(user.PasswordHash, user.Salt, request.Password))
            {
                throw new LoginException($"{request.Email} invalid password");
            }

            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email)
                };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim("role", role.ToString()));
            }

            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;

        }

        public AuthOptions GetOptions() => new AuthOptions();

        public string GetToken(ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;
            var options = new AuthOptions();

            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: options.Issuer,
                    audience: options.Audience,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(options.Lifetime)),
                    signingCredentials: new SigningCredentials(options.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        private bool CompareHash(string Hash, string Salt, string Password)
        {
           var generate = Convert.ToBase64String(KeyDerivation.Pbkdf2(
               password: Password,
               salt: Convert.FromBase64String(Salt),
               prf: KeyDerivationPrf.HMACSHA1,
               iterationCount: 10000,
               numBytesRequested: 256 / 8));

            return Hash.Contains(generate);
        }
    }
}
