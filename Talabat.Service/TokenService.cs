using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Core.IService;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace Talabat.Service
{
    public class TokenService : ITokenService
    {
        public IConfiguration Configuration { get; } // because take appsetting
        public TokenService(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public async Task<string> CreateToken(AppUser user, UserManager<AppUser> userManager)
        {
            // private Clamis (User-Defined)
            var authClaims = new List<Claim>()
            {
                new Claim (ClaimTypes.Email,user.Email),
                new Claim (ClaimTypes.GivenName,user.DisplayName)
            };

            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));

            // secret Key
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]));

            // create token

            var token = new JwtSecurityToken(
                // register Claims
                issuer: Configuration["JWT:ValidIssuer"],
                audience: Configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(double.Parse(Configuration["JWT:DurationInDays"])),

                // private Clamis
                claims: authClaims,

                //  secret Key
                signingCredentials: new SigningCredentials(authKey,SecurityAlgorithms.HmacSha256Signature)

                );
            // to convert token to Encoded
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
