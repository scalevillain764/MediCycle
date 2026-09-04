using Application.DTO.AuthDTO;
using Application.DTO.AuthDTO.Client;
using Application.DTO.AuthDTO.Worker;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IAuthService = Application.Interfaces.IAuthService;
namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly IAuthService _service;
        public AuthController(IAuthService authService)
        {
            _service = authService;
        }

        [HttpPost("registrate_worker")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateWorkerAsync([FromBody] AuthWorkerRegistrationDTO DTO, CancellationToken token)
            => ProcessResult(await _service.RegistrateWorkerAsync(DTO, token));

        [HttpPost("registrate_client")]
        [AllowAnonymous]
        public async Task<IActionResult> RegistrateWorkerAsync([FromBody] AuthClientRegistrationDTO DTO, CancellationToken token)
            => ProcessResult(await _service.RegistrateClientAsync(DTO, token));

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LogInAsync([FromBody] AuthLoginDTOandRegistrationResponse DTO, CancellationToken token)
            => ProcessResult(await _service.LogInAsync(DTO, token));

        [HttpPatch("edit/password")]
        [Authorize]
        public async Task<IActionResult> EditPasswordAsync([FromBody] ChangePasswordDTO DTO, CancellationToken token)
            => ProcessResult(await _service.EditPasswordAsync(DTO, token));

        [HttpPatch("refresh")]
        [Authorize]
        public async Task<IActionResult> RefreshAsync(Ulid userId, CancellationToken token)
            => ProcessResult(await _service.RefreshAsync(userId, token));
    }
}