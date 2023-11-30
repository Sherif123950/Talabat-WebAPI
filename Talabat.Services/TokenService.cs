using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(ApplicationUser User, UserManager<ApplicationUser> userManager)
        {
            var Authclaims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,User.DisplayName),
                new Claim(ClaimTypes.Email, User.Email)
            };
            var UserRoles = await userManager.GetRolesAsync(User);
            foreach (var role in UserRoles)
            {
                Authclaims.Add(new Claim(ClaimTypes.Role,role));
            }
            var KeyAuth = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var Token = new JwtSecurityToken(issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires:DateTime.Now.AddDays(double.Parse (_configuration["JWT:DurationInDays"])),
                claims:Authclaims,
                signingCredentials:new SigningCredentials(KeyAuth,SecurityAlgorithms.HmacSha256Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
