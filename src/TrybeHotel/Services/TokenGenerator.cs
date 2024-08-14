using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Services
{
    public class TokenGenerator
    {
        private readonly TokenOptions _tokenOptions;
        public TokenGenerator()
        {
           _tokenOptions = new TokenOptions {
                Secret = "4d82a63bbdc67c1e4784ed6587f3730c",
                ExpiresDay = 1
           };

        }
        public string Generate(UserDto user)
        {
            var handler = new JwtSecurityTokenHandler();
            var encoder = Encoding.ASCII.GetBytes(_tokenOptions.Secret);
            var claims = AddClaims(user);

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = System.DateTime.UtcNow.AddDays(_tokenOptions.ExpiresDay),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(encoder), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = handler.CreateToken(descriptor);

            return handler.WriteToken(token);
        }

        private ClaimsIdentity AddClaims(UserDto user)
        {
            ClaimsIdentity claims = new();
            claims.AddClaim(new Claim(ClaimTypes.Email, user.Email!));
            if (user.UserType == "admin") {
                claims.AddClaim(new Claim(ClaimTypes.Role, user.UserType!));
            }
            return claims;
        }
    }
}