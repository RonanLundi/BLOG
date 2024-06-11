using Blog.Models;
using BlogAPI.Extension;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogAPI.Services
{
    public class TokenService
    {
        public TokenService() { }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
            var claims = user.GetClaims();
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(
                    new Claim[] 
                    {
                        new Claim(ClaimTypes.Name, "ronan"),
                        new Claim(ClaimTypes.Role, "admin"),
                        new Claim("Exemplo", "Teste")
                    }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token); 
        }
    }
}
