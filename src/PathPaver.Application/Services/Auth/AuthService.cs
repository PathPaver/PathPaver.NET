using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using PathPaver.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace PathPaver.Application.Services.Auth
{
    public class AuthService
    {
        public static string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AuthSettings.PrivateKey);
            
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature); // Create signing hash with secret key

            var tokenSettings = new SecurityTokenDescriptor
            {
                Subject = GenerateTokenSettings(user),
                Expires = DateTime.UtcNow.AddHours(2), // Expire in 2 hours after connection
                SigningCredentials = credentials
            };
            
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenSettings));
        }

        private static ClaimsIdentity GenerateTokenSettings(User user) // Generate payload of token
        {
            return new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            ]);
        }
    }
}
