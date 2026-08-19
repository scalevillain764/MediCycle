using Application.DTO.AuthDTO;
using Application.DTO.AuthDTO.Client;
using Application.DTO.AuthDTO.Worker;
using Infrastructure.Result;
namespace Application.Interfaces
{
    public interface IAuthService
    {
        public Task<Result<AuthLoginDTOandRegistrationResponse>> RegistrateWorkerAsync(AuthWorkerRegistrationDTO DTO, CancellationToken token);
        public Task<Result<AuthLoginDTOandRegistrationResponse>> RegistrateClientAsync(AuthClientRegistrationDTO DTO, CancellationToken token);
        public Task<AuthLoginResponse> LogInAsync(AuthLoginDTO DTO, CancellationToken token);
    }
}