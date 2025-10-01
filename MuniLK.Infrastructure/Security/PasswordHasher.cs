// MuniLK.Infrastructure/Security/PasswordHasher.cs
using Microsoft.AspNetCore.Identity;
using MuniLK.Domain.Interfaces; // For IPasswordHasher
using System;

namespace MuniLK.Infrastructure.Security
{
    /// <summary>
    /// Implementation of IPasswordHasher using ASP.NET Core Identity's PasswordHasher.
    /// </summary>
    public class PasswordHasher : IPasswordHasher
    {
        private readonly IPasswordHasher<IdentityUser> _identityPasswordHasher;

        public PasswordHasher(IPasswordHasher<IdentityUser> identityPasswordHasher)
        {
            _identityPasswordHasher = identityPasswordHasher;
        }

        public string HashPassword(string password)
        {
            // IdentityUser is a dummy here, as the hashing doesn't depend on the user instance
            return _identityPasswordHasher.HashPassword(new IdentityUser(), password);
        }

        public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            // IdentityUser is a dummy here
            var result = _identityPasswordHasher.VerifyHashedPassword(new IdentityUser(), hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
