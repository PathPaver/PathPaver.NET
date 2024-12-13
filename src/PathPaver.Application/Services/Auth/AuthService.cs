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
            var creds = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature); // Create signing hash with secret key

            var tokenSettings = new SecurityTokenDescriptor
            {
                Subject = GenerateTokenSettings(user),
                SigningCredentials = creds
            };

            var token = tokenHandler.CreateToken(tokenSettings);
            return tokenHandler.WriteToken(token);
        }

        private static ClaimsIdentity GenerateTokenSettings(User user) // Generate payload of token
        {
            var claims = new ClaimsIdentity();

            claims.AddClaim(new Claim(ClaimTypes.Name, user.Username));
            claims.AddClaim(new Claim(ClaimTypes.Email, user.Email));

            return claims;
        }
    }
}
