using FilmStudioSFF.Models;
using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace FilmStudioSFF.Services
{
    public class AuthenticationService 
    {
        private const string SecretKey = "YourSuperSecretKey1234567890abcdef";    


        public string GenerateJwtToken(string username, string role, int id)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim(ClaimTypes.Role, role) // Ändra till ClaimTypes.Role
            };

            var token = new JwtSecurityToken(
                issuer: "FilmStudioSFF",
                audience: "FilmStudioSFF",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}