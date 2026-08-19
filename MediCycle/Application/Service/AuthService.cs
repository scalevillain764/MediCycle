using Application.DTO.AuthDTO;
using Application.DTO.AuthDTO.Client;
using Application.DTO.AuthDTO.Worker;

using Application.Interfaces;
using Infrastructure.Result;
using Infrastructure;
using Error = Domain.Enums.ErrorType;
using Microsoft.EntityFrameworkCore;

using Worker = Domain.Worker;
using Client = Domain.Client;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        public AuthService(AppDbContext context)
        {
            _context = context;
        }
        private Worker CreateWorker(AuthWorkerRegistrationDTO DTO, string hashedPassword) 
            => new Worker(DTO.login, hashedPassword, DTO.name, DTO.surname, DTO.birthday, DTO.role, DTO.driverLicenseNumber);

        private Client CreateClient(AuthClientRegistrationDTO DTO, string hashedPassword)
            => new Client(DTO.login, hashedPassword);

        private async Task<Result<AuthLoginDTOandRegistrationResponse>> RegistrateAsync<TRequest, TEntity>(TRequest DTO, Func<TRequest, string, TEntity> create, CancellationToken token) 
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
        public Task<Result<AuthLoginDTOandRegistrationResponse>> RegistrateWorkerAsync(AuthWorkerRegistrationDTO DTO, CancellationToken token)
            => RegistrateAsync(DTO, CreateWorker, token);

        public Task<Result<AuthLoginDTOandRegistrationResponse>> RegistrateClientAsync(AuthClientRegistrationDTO DTO, CancellationToken token)
     => RegistrateAsync(DTO, CreateClient, token);

        public Task<AuthLoginResponse> LogInAsync(AuthLoginDTOandRegistrationResponse DTO, CancellationToken token)
        {

        }
    }
}