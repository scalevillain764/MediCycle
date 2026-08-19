using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using ITokenService = Application.Interfaces.ITokenService;
using Infrastructure;
namespace Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IHttpContextAccessor _accessor;
        private AppDbContext _context;
        public TokenService(IHttpContextAccessor accessor, AppDbContext context)
        {
            _accessor = accessor;
            _context = context;
        }
        public string CreateAccessToken(Ulid Id, string name, string userType, string? userRole)
        {
            Claim[] claims = {
                new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.)
            };            
        }
    }
}