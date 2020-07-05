using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace AuthenticationServer.Api.Services
{
    public class AuthOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Lifetime { get; set; }

        public SymmetricSecurityKey SymmetricSecurityKey { get; set; }

        public AuthOptions()
        {

            Issuer = Environment.GetEnvironmentVariable("ISSUER");
            Audience = Environment.GetEnvironmentVariable("AUDIENCE");
            if(int.TryParse(Environment.GetEnvironmentVariable("LIFETIME"), out var lifetime))
            {
                Lifetime = lifetime;
            }
            else
            {
                Lifetime = 60;
            }

            SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("KEY")));
        }
    }
}
