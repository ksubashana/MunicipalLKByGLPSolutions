// MuniLK.Infrastructure/Security/AuthService.cs
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MuniLK.Application.Generic.DTOs;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Applications.Interfaces;
using MuniLK.Domain.Entities; // For Domain.Entities.User
using MuniLK.Domain.Interfaces; // For ITokenService, IPasswordHasher
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MuniLK.Infrastructure.Security
{
    /// <summary>
    /// Implementation of IAuthService, orchestrating user management and token generation.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher; // Your custom IPasswordHasher interface
        private readonly IContactRepository _contactRepository; // Injected interface from Application layer
        private readonly ICurrentTenantService _currentTenantService;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ITokenService tokenService,
            IPasswordHasher passwordHasher,
            ICurrentTenantService currentTenantService,IContactRepository contactRepository, IUserRepository userRepository,IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager; 
            _tokenService = tokenService;
            _passwordHasher = passwordHasher; // Inject your custom password hasher
            _currentTenantService = currentTenantService;
            _contactRepository = contactRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Handles user login.
        /// </summary>
        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                return new AuthResponse { Succeeded = false, Errors = new[] { "Invalid credentials." } };
            }

            // Check password using SignInManager
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return new AuthResponse { Succeeded = false, Errors = new[] { "Invalid credentials." } };
            }
            Guid? tenantId = null;
            var tenantIdClaim = (await _userManager.GetClaimsAsync(user))
                                    .FirstOrDefault(c => c.Type == "TenantId"); 

            if (tenantIdClaim != null && Guid.TryParse(tenantIdClaim.Value, out var parsedTenantId))
            {
                tenantId = parsedTenantId;
            }
            // Map IdentityUser to your Domain.Entities.User for token generation
            var domainUser = new Domain.Entities.User
            {
                Id = Guid.Parse(user.Id),
                Email = user.Email ?? string.Empty,
                Username = user.UserName ?? string.Empty,
                // Assuming TenantId is stored as a claim or custom property in IdentityUser
                // You might need to add a custom claim type for TenantId during registration/user creation
                TenantId = tenantId
            };

            var accessToken = await _tokenService.GenerateAccessTokenAsync(domainUser);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(domainUser);

            return new AuthResponse { Succeeded = true, AccessToken = accessToken, RefreshToken = refreshToken };
        }

        /// <summary>
        /// Handles new user registration.
        /// </summary>
        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            // Start domain transaction (covers domain entities only)
            using var domainTx = await _unitOfWork.BeginTransactionAsync();

            IdentityUser? identityUser = null;
            try
            {
                // 1. Create Identity user (separate DbContext)
                identityUser = new IdentityUser { UserName = request.Username, Email = request.Email };
                var identityResult = await _userManager.CreateAsync(identityUser, request.Password);

                if (!identityResult.Succeeded)
                {
                    return new AuthResponse
                    {
                        Succeeded = false,
                        Errors = identityResult.Errors.Select(e => e.Description).ToArray()
                    };
                }

                // 2. Add tenant claim (still on Identity context)
                var claimResult = await _userManager.AddClaimAsync(identityUser, new Claim("TenantId", request.TenantId.ToString()));
                if (!claimResult.Succeeded)
                {
                    // Rollback by deleting Identity user
                    await _userManager.DeleteAsync(identityUser);
                    return new AuthResponse
                    {
                        Succeeded = false,
                        Errors = claimResult.Errors.Select(e => e.Description).ToArray()
                    };
                }

                // 3. Prepare domain entities (same Guid for user/contact linkage)
                var userIdGuid = Guid.Parse(identityUser.Id);
                var contact = new MuniLK.Domain.Entities.ContactEntities.Contact
                {
                    Id = userIdGuid,
                    FullName = request.FullName,
                    NIC = request.NIC,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    AddressLine1 = request.AddressLine1,
                    AddressLine2 = request.AddressLine2,
                    City = request.City,
                    District = request.District,
                    Province = request.Province,
                    PostalCode = request.PostalCode,
                    TenantId = request.TenantId,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };
                await _contactRepository.AddAsync(contact);

                var domainUser = new Domain.Entities.User
                {
                    Id = userIdGuid,
                    Email = request.Email,
                    Username = request.Username,
                    ContactId = userIdGuid,
                    TenantId = request.TenantId,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                // IMPORTANT: Avoid storing password twice – if AddUserAsync hashes/stores it again, reconsider design.
                await _userRepository.AddUserAsync(domainUser, request.Password);

                // 4. Commit domain changes
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                // 5. Generate tokens (uses domain model)
                var accessToken = await _tokenService.GenerateAccessTokenAsync(domainUser);
                var refreshToken = await _tokenService.GenerateRefreshTokenAsync(domainUser);

                return new AuthResponse
                {
                    Succeeded = true,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
            }
            catch (Exception ex)
            {
                // Roll back domain transaction
                await _unitOfWork.RollbackTransactionAsync();

                // Compensation: remove Identity user if it was created
                if (identityUser != null)
                {
                    await _userManager.DeleteAsync(identityUser);
                }

                return new AuthResponse
                {
                    Succeeded = false,
                    Errors = new[] { $"Registration failed: {ex.Message}" }
                };
            }
        }

        /// <summary>
        /// Handles refreshing an access token using a refresh token.
        /// </summary>
        public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var principal = _tokenService.GetPrincipalFromExpiredToken(refreshToken);

                // Validate this is actually a refresh token
                var tokenType = principal.Claims.FirstOrDefault(c => c.Type == "token_type")?.Value;
                if (tokenType != "refresh")
                {
                    return new AuthResponse { Succeeded = false, Errors = new[] { "Invalid token type. Expected refresh token." } };
                }

                var userId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return new AuthResponse { Succeeded = false, Errors = new[] { "Invalid refresh token." } };
                }

                var identityUser = await _userManager.FindByIdAsync(userId);
                if (identityUser == null)
                {
                    return new AuthResponse { Succeeded = false, Errors = new[] { "User not found for refresh token." } };
                }

                // Map IdentityUser to your Domain.Entities.User for token generation
                var domainUser = new Domain.Entities.User
                {
                    Id = Guid.Parse(identityUser.Id),
                    Email = identityUser.Email ?? string.Empty,
                    Username = identityUser.UserName ?? string.Empty,
                    // Retrieve TenantId from claims if present in the refresh token
                    TenantId = Guid.TryParse(principal.Claims.FirstOrDefault(c => c.Type == "tenantid")?.Value, out Guid tenantId) ? tenantId : Guid.Empty
                };

                // Generate new access token and refresh token
                var newAccessToken = await _tokenService.GenerateAccessTokenAsync(domainUser);
                var newRefreshToken = await _tokenService.GenerateRefreshTokenAsync(domainUser); // Generate a new refresh token

                return new AuthResponse { Succeeded = true, AccessToken = newAccessToken, RefreshToken = newRefreshToken };
            }
            catch (Exception)
            {
                return new AuthResponse { Succeeded = false, Errors = new[] { "Invalid or expired refresh token." } };
            }
        }

        public async Task<AuthResponse> RemoveUserAsync(DeleteUserRequest request)
        {
            if (!Guid.TryParse(request.UserId, out Guid userIdGuid))
            {
                return new AuthResponse { Succeeded = false, Errors = new[] { "Invalid User ID format." } };
            }

            var user = await _userManager.FindByIdAsync(request.UserId); // Find user by ID
            if (user == null)
            {
                return new AuthResponse { Succeeded = false, Errors = new[] { "User not found." } };
            }

            var result = await _userManager.DeleteAsync(user); // Delete the user

            if (!result.Succeeded)
            {
                return new AuthResponse { Succeeded = false, Errors = result.Errors.Select(e => e.Description).ToArray() };
            }

            return new AuthResponse { Succeeded = true, Message = "User deleted successfully." };
        }
    }
}
