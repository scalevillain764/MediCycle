using Domain;
using Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using ITokenService = Application.Interfaces.ITokenService;
namespace Application.Services
{
    public class TokenService : ITokenService
    {
        private AppDbContext _context;
        public TokenService(AppDbContext context)
        {
            _context = context;
        }
        public string CreateAccessToken(Ulid Id, string name, string userType, string? userRole)
        {
            List<Claim> claims = [
                new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
                new Claim(ClaimTypes.Name, name),
                new Claim("UserType", userType),
            ];

            if (userType == "Worker" && !string.IsNullOrEmpty(userRole))
                claims.Add(new Claim(ClaimTypes.Role, userRole));

            string? decoded_key = Environment.GetEnvironmentVariable("SECRET_KEY");

            if (string.IsNullOrEmpty(decoded_key))
                throw new KeyNotFoundException("Secret key not found");

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(decoded_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                   issuer: "MediCycleServer",
                   audience: "MediCycleAudience",
                   claims: claims,
                   expires: DateTime.UtcNow.AddMinutes(15),
                   signingCredentials: creds
                   );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string CreateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }       
    }
}