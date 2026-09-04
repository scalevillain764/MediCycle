using Application.DTO.AuthDTO;
using Application.DTO.AuthDTO.Client;
using Application.DTO.AuthDTO.Worker;
using Application.Interfaces;
using Domain;
using Domain.Enums;
using Infrastructure;
using Infrastructure.Responding;
using Microsoft.EntityFrameworkCore;
using Client = Domain.Client;
using Error = Domain.Enums.ErrorType;
using ITokenService = Application.Interfaces.ITokenService;
using ICurrnetUser = Application.Abstractions.ICurrentUser;
using Worker = Domain.Worker;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly ICurrnetUser _currentUser;

        public AuthService(AppDbContext context, ITokenService tokenService, IHttpContextAccessor httpAccessor, ICurrnetUser currnetUser)
        {
            _context = context;
            _tokenService = tokenService;
            _httpAccessor = httpAccessor;
            _currentUser = currnetUser;
        }

        private string AppendCookiesAndGetAccessToken(User user)
        {
            string userType = user switch
            {
                Worker => "Worker",
                Client => "Client",
                _ => throw new InvalidDataException("Такого типа нет")
            };

            string? userRole = null;

            if (user is Worker worker)
                userRole = worker.Role.ToString();

            string accessToken = _tokenService.CreateAccessToken(user.Id, user.Login, userType, userRole);
            string refreshToken = _tokenService.CreateRefreshToken();

            string refreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken);

            user.RefreshTokenHash = refreshTokenHash;
            user.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
                SameSite = SameSiteMode.Strict,
                Secure = true
            };

            _httpAccessor.HttpContext?.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

            return accessToken;
        }

        private Worker CreateWorker(AuthWorkerRegistrationDTO DTO, string hashedPassword) 
            => new Worker(DTO.login, hashedPassword, DTO.name, DTO.surname, DTO.birthday, DTO.role, DTO.driverLicenseNumber);

        private Client CreateClient(AuthClientRegistrationDTO DTO, string hashedPassword)
            => new Client(DTO.organization_name, DTO.login, hashedPassword);

        private async Task<Result<AuthLoginDTOandRegistrationResponse>> RegisterAsync<TRequest, TEntity>(TRequest DTO, Func<TRequest, string, TEntity> create, CancellationToken token) 
            where TEntity : Domain.User
            where TRequest : UniversalRegistrationDTO
        {
            bool loginExists = await _context.AllUsers
                .AnyAsync(u => u.Login == DTO.login);

            if (loginExists)
                return Result<AuthLoginDTOandRegistrationResponse>.Error("Такой логин уже существует", Error.Validation);

            string hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(DTO.password, workFactor: 11);

            var user = create(DTO, hashedPassword);

            _context.AllUsers.Add(user);

            await _context.SaveChangesAsync(token);

            return Result<AuthLoginDTOandRegistrationResponse>.Success(new AuthLoginDTOandRegistrationResponse(user.Login, DTO.password));
        }
        public Task<Result<AuthLoginDTOandRegistrationResponse>> RegisterWorkerAsync(AuthWorkerRegistrationDTO DTO, CancellationToken token)
            => RegisterAsync(DTO, CreateWorker, token);

        public Task<Result<AuthLoginDTOandRegistrationResponse>> RegisterClientAsync(AuthClientRegistrationDTO DTO, CancellationToken token)
            => RegisterAsync(DTO, CreateClient, token);
        public async Task<Result<AuthLoginResponse>> EditPasswordAsync(ChangePasswordDTO DTO, CancellationToken token)
        {
            var user = await _context.AllUsers
                .FindAsync(_currentUser.UserId, token);

            if (user == null)
                return Result<AuthLoginResponse>.Error("Пользователь не найден", Error.Forbidden);

            bool oldPasswordIsOk = BCrypt.Net.BCrypt.EnhancedVerify(user.PasswordHash, DTO.oldPassword);

            if (!oldPasswordIsOk)
                return Result<AuthLoginResponse>.Error("Старый пароль неправильный", Error.Validation);

            user.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(DTO.newPassword);

            await _context.SaveChangesAsync(token);

            string accessToken = AppendCookiesAndGetAccessToken(user);

            return Result<AuthLoginResponse>.Success(new AuthLoginResponse(user.Id, accessToken));
        }

        public async Task<Result<AuthLoginResponse>> LogInAsync(AuthLoginDTOandRegistrationResponse DTO, CancellationToken token)
        {
            var user = await _context.AllUsers
                .FirstOrDefaultAsync(x => x.Login == DTO.login, token);

            if (user == null)
                return Result<AuthLoginResponse>.Error("Пользователь не найден, провертье логин", Error.NotFound);

            if (!BCrypt.Net.BCrypt.EnhancedVerify(user.PasswordHash, DTO.password))
                return Result<AuthLoginResponse>.Error("Невепрный пароль", Error.Validation);

            string accessToken = AppendCookiesAndGetAccessToken(user);

            await _context.SaveChangesAsync(token);

            return Result<AuthLoginResponse>.Success(new AuthLoginResponse(user.Id, accessToken));
        }

        public async Task<Result<AuthLoginResponse>> RefreshAsync(Ulid userId, CancellationToken token)
        {
            string? existingRefreshToken = _httpAccessor.HttpContext?.Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(existingRefreshToken))
                return Result<AuthLoginResponse>.Error("Куки пусты", ErrorType.Unauthorized);

            var user = await _context.AllUsers
                .FirstOrDefaultAsync(x => x.Id == userId, token);

            if (user == null)
                return Result<AuthLoginResponse>.Error("Пользователь не найден", ErrorType.Unauthorized);

            if (user.RefreshTokenExpiresAt == null || user.RefreshTokenExpiresAt < DateTime.UtcNow)
                return Result<AuthLoginResponse>.Error("Сессия истекла", ErrorType.Unauthorized);

            if (!BCrypt.Net.BCrypt.Verify(existingRefreshToken, user.RefreshTokenHash))
                return Result<AuthLoginResponse>.Error("Невалидный токен сессии", ErrorType.Unauthorized);

            string AccessToken = AppendCookiesAndGetAccessToken(user);

            await _context.SaveChangesAsync(token);
            return Result<AuthLoginResponse>.Success(new AuthLoginResponse(user.Id, AccessToken));
        }
    }
}