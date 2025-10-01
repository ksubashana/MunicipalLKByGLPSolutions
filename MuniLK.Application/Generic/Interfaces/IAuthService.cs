// MuniLK.Application/Interfaces/IAuthService.cs
using MuniLK.Application.Generic.DTOs;
using System.Threading.Tasks;

namespace MuniLK.Application.Generic.Interfaces
{
    /// <summary>
    /// Defines the application service for authentication operations.
    /// </summary>
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> RefreshTokenAsync(string refreshToken);
        /// <param name="request">The request containing the user's ID to delete.</param>
        /// <returns>An AuthResponse indicating success or failure.</returns>
        Task<AuthResponse> RemoveUserAsync(DeleteUserRequest request); // Added method
    }
}
