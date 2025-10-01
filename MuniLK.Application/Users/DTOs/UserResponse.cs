using System;

namespace MuniLK.Application.Users.DTOs
{
    public class UserResponse
    {
        public string Id { get; set; } = string.Empty;      // Identity/User Id (Guid as string)
        public string UserName { get; set; } = string.Empty; // Username
        public string Email { get; set; } = string.Empty;    // Email
        public string Role { get; set; } = string.Empty;     // One role or a concatenation for UI
    }
}