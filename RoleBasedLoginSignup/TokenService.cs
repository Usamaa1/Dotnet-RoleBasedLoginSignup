using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RoleBasedLoginSignup
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public string GenerateToken(Models.User user)
        {
            var jwt = _configuration.GetSection("jwt");
            var key = Encoding.UTF8.GetBytes(jwt["key"]);

            var claims = new[]
            {
                new Claim (ClaimTypes.Name, user.Username),
                new Claim (ClaimTypes.Role, user.Role),
            };

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwt["ExpiresInMinutes"])),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256
                    )


                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
