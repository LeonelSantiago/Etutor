using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Etutor.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Etutor.Core.Models.Configurations;

namespace Etutor.Services.Implementations
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        private readonly JwtConfig _config;

        public TokenGeneratorService(IOptions<JwtConfig> config)
        {
            _config = config.Value;
        }

        public object GenerateJwtToken(string givenName, IdentityUser<int> user, List<Claim> userClaims = null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, givenName),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, Guid.NewGuid().ToString())
            };

            claims.AddRange(userClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Key));
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_config.ExpireDays));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config.Issuer,
                _config.Issuer,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
