using MiniProjectManagerAPI.DTOs;
using System.Security.Claims;


namespace MiniProjectManagerAPI.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        int GetUserIdFromClaims(ClaimsPrincipal user);
    }
}
