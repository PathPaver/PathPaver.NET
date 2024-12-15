using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using PathPaver.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using PathPaver.Application.Repository.Entities;

namespace PathPaver.Application.Services.Auth
{
    public class AuthService(IUserRepository userRepository)
    {
        #region Password Related

        public static string HashString(string toHash) => 
            BCrypt.Net.BCrypt.EnhancedHashPassword(toHash);

        public static bool CompareHash(string hashed, string toHash) => 
            BCrypt.Net.BCrypt.EnhancedVerify(toHash, hashed);
        
        #endregion

        #region JWT Related

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AuthSettings.PrivateKey);
            
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature); // Create signing hash with secret key

            var tokenSettings = new SecurityTokenDescriptor
            {
                Subject = GenerateTokenSettings(user),
                Expires = DateTime.UtcNow.AddHours(1), // Expire in 1 hour after created
                SigningCredentials = credentials
            };
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenSettings));
        }

        private static ClaimsIdentity GenerateTokenSettings(User user) // Generate payload of token
        {
            return new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Email, user.Email)
            ]);
        }

        public bool IsTokenValid(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(token, AuthSettings.GetTokenValidationParameters(), out var validatedToken);
                var email = ((JwtSecurityToken)validatedToken).Claims.First(x => x.Type == "email").Value;
                var user = userRepository.GetByEmail(email);
                return user != null;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
