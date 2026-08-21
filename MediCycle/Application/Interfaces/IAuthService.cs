using Application.DTO.AuthDTO;
using Application.DTO.AuthDTO.Client;
using Application.DTO.AuthDTO.Worker;
using Infrastructure.Result;
namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<AuthLoginDTOandRegistrationResponse>> RegistrateWorkerAsync(AuthWorkerRegistrationDTO DTO, CancellationToken token); // only for admin
        Task<Result<AuthLoginDTOandRegistrationResponse>> RegistrateClientAsync(AuthClientRegistrationDTO DTO, CancellationToken token);
        Task<Result<AuthLoginResponse>> LogInAsync(AuthLoginDTOandRegistrationResponse DTO, CancellationToken token);
        Task<Result<AuthLoginResponse>> RefreshAsync(Ulid userId, CancellationToken token);
    }
}