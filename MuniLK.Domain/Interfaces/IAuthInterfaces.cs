// MuniLK.Domain/Interfaces/IAuthInterfaces.cs
using MuniLK.Domain.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MuniLK.Domain.Interfaces
{
    /// <summary>
    /// Defines an interface for hashing and verifying passwords.
    /// </summary>
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyHashedPassword(string hashedPassword, string providedPassword);
    }

    /// <summary>
    /// Defines an interface for generating and validating authentication tokens (e.g., JWTs).
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Generates an access token for the given domain user.
        /// </summary>
        /// <param name="user">The domain user entity.</param>
        /// <returns>The generated access token string.</returns>
        Task<string> GenerateAccessTokenAsync(User user);

        /// <summary>
        /// Generates a refresh token for the given domain user.
        /// </summary>
        /// <param name="user">The domain user entity.</param>
        /// <returns>The generated refresh token string.</returns>
        Task<string> GenerateRefreshTokenAsync(User user);

        /// <summary>
        /// Retrieves claims principal from an expired token for refresh token validation.
        /// </summary>
        /// <param name="token">The expired token string.</param>
        /// <returns>ClaimsPrincipal if token is valid, otherwise throws SecurityTokenException.</returns>
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }

    /// <summary>
    /// Defines an interface for user repository operations.
    /// In a strict Clean Arch, this would abstract UserManager.
    /// For simplicity, in this example, UserManager is used directly in Infrastructure.AuthService.
    /// </summary>
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(Guid userId);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user, string password);
        Task LinkContactAsync(Guid userId, Guid contactId); // new method to link contact
        Task<List<User>> GetUsersByTenantAsync(Guid tenantId); // new method to link contact

    }

    /// <summary>
    /// Defines an interface for role repository operations.
    /// </summary>
    //public interface IRoleRepository
    //{
    //    Task<Role?> GetRoleByNameAsync(string roleName);
    //    Task AddRoleAsync(Role role);
    //    // Add other role-related CRUD operations as needed
    //}
}
