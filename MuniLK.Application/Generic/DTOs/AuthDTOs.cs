// MuniLK.Application/DTOs/AuthDTOs.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace MuniLK.Application.Generic.DTOs
{
    /// <summary>
    /// Request DTO for user login.
    /// </summary>
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request DTO for new user registration.
    /// </summary>
    public class RegisterRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Guid TenantId { get; set; } // Required for multi-tenancy registration

        // Contact info
        public string FullName { get; set; } = string.Empty;
        public string NIC { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string AddressLine2 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
    }

    /// <summary>
    /// Response DTO for authentication operations (login, register, refresh token).
    /// </summary>
    public class AuthResponse
    {
        public bool Succeeded { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string Message { get; set; }
        public string[] Errors { get; set; } = Array.Empty<string>();
        // Added: Identity/User Id returned on successful registration/login
        public string? UserId { get; set; }
    }

    /// <summary>
    /// Request DTO for refreshing an access token.
    /// </summary>
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
    public class DeleteUserRequest
    {
        [Required]
        // You can choose to identify the user by Username or Id.
        // Using Id is generally more robust as usernames can change or be less unique.
        // If using Id, ensure it's a valid GUID string.
        public string UserId { get; set; } = string.Empty;
    }
}
