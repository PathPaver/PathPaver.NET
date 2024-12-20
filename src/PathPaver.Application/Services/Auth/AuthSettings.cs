﻿using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace PathPaver.Application.Services.Auth {
    public class AuthSettings
    {
        public static string? PrivateKey { get; set; }

        public static byte[] GetKeyBytes() =>
            Encoding.ASCII.GetBytes(PrivateKey!);
        
        public static TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                // Key used in signature
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(PrivateKey!)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        }
    }
}
