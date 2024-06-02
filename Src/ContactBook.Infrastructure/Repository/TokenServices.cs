using ContactBook.Core.Entities;
using ContactBook.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Infrastructure.Repository
{
    public class TokenServices : ITokenServices
    {
        private readonly IConfiguration configuration;
        private readonly SymmetricSecurityKey key;

        public TokenServices(IConfiguration configuration)
        {
            this.configuration = configuration;
            key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Key"]));
        }

        /// <summary>
        /// Create Token
        /// </summary>
        /// <param name="appUser">Object of user</param>
        /// <returns></returns>
        public string CreateToken(AppUser appUser)
        {
            var claims = new List<Claim>()
            {
                new Claim("FirstName", appUser.FirstName),
                new Claim("LastName", appUser.LastName),
                new Claim(JwtRegisteredClaimNames.Email, appUser.Email),
            };
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(10),
                Issuer = configuration["Token:Issuer"],
                SigningCredentials = creds,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
