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
        #region Class Fields

        private readonly JwtSecurityTokenHandler _tokenHandler = new();
        
        private readonly TokenValidationParameters _validationParameters = AuthSettings.GetTokenValidationParameters();

        private readonly SigningCredentials _credentials = new(
            new SymmetricSecurityKey(AuthSettings.GetKeyBytes()),
            SecurityAlgorithms.HmacSha256Signature
        );
        #endregion

        #region Password Related

        public static string HashString(string toHash) =>
            BCrypt.Net.BCrypt.EnhancedHashPassword(toHash);

        public static bool CompareHash(string hashed, string toHash) =>
            BCrypt.Net.BCrypt.EnhancedVerify(toHash, hashed);

        #endregion

        #region JWT Related

        public string GenerateToken(User user)
        {
            var tokenSettings = new SecurityTokenDescriptor
            {
                Subject = GenerateTokenSettings(user),
                Expires = DateTime.UtcNow.AddHours(1), // Expire in 1 hour after created
                SigningCredentials = _credentials
            };
            return _tokenHandler.WriteToken(_tokenHandler.CreateToken(tokenSettings));
        }

        private static ClaimsIdentity GenerateTokenSettings(User user) // Generate payload of token
        {
            return new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, user.Email)
            }.Concat(user.Roles.Select(r => new Claim(ClaimTypes.Role, r))));
        }

        public bool IsTokenValid(string token)
        {
            try
            {
                _tokenHandler.ValidateToken(token, _validationParameters, out var validatedToken);
                var email = ((JwtSecurityToken)validatedToken).Claims.First(c => c.Type == "email").Value;
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