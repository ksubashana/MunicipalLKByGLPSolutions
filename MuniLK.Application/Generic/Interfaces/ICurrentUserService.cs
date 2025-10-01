// MuniLK.Application.Generic.Interfaces/ICurrentUserService.cs
using System;
using System.Security.Claims;

namespace MuniLK.Application.Generic.Interfaces
{
    /// <summary>
    /// Provides information about the current authenticated user.
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// Gets the unique identifier (ID) of the current user.
        /// Returns null if the user is not authenticated or the ID claim is not found.
        /// </summary>
        string? UserId { get; }

        /// <summary>
        /// Gets the username or email of the current user.
        /// Returns null if the user is not authenticated or the name claim is not found.
        /// </summary>
        string? UserName { get; }

        /// <summary>
        /// Gets the current user's principal (all claims).
        /// Returns null if the user is not authenticated.
        /// </summary>
        ClaimsPrincipal? User { get; }

        /// <summary>
        /// Checks if the current user is authenticated.
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Checks if the current user belongs to a specific role.
        /// </summary>
        /// <param name="roleName">The name of the role to check.</param>
        /// <returns>True if the user is in the role, false otherwise.</returns>
        bool IsInRole(string roleName);

        /// <summary>
        /// Gets a specific claim value for the current user.
        /// </summary>
        /// <param name="claimType">The type of the claim (e.g., ClaimTypes.Email, "sub").</param>
        /// <returns>The claim value, or null if the claim is not found.</returns>
        string? GetClaim(string claimType);
        IEnumerable<string>? GetRoles();
    }
}