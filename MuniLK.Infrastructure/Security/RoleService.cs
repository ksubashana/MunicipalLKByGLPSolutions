using Microsoft.AspNetCore.Identity;
using MuniLK.Application.Contact.DTOs;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Applications.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICurrentTenantService _currentTenantService;
        private readonly IContactRepository _contactRepository;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, ICurrentTenantService currentTenantService, IContactRepository contactRepository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _currentTenantService = currentTenantService;
            _contactRepository = contactRepository;
        }

        public async Task CreateRoleAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Role name cannot be empty.");

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                if (!result.Succeeded)
                    throw new Exception("Failed to create role: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        public async Task DeleteRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                throw new ArgumentException("Role not found.");

            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
                throw new Exception("Failed to delete role: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task AddUserToRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found.");

            if (!await _roleManager.RoleExistsAsync(roleName))
                throw new ArgumentException("Role not found.");

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
                throw new Exception("Failed to add user to role: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task RemoveUserFromRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found.");

            if (!await _roleManager.RoleExistsAsync(roleName))
                throw new ArgumentException("Role not found.");

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (!result.Succeeded)
                throw new Exception("Failed to remove user from role: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<List<string>> GetAllRolesAsync()
        {
            return _roleManager.Roles.Select(r => r.Name).ToList();
        }

        public async Task<List<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found.");

            return (await _userManager.GetRolesAsync(user)).ToList();
        }

       public async Task<List<ContactResponse>> GetContactsByTenantAndRoleAsync(string roleName)
{
    var contactsInRole = new List<ContactResponse>();

    // Get all users
    var allUsers = _userManager.Users.ToList();

    foreach (var user in allUsers)
    {
        // Check if the user has the tenant claim
        var claims = await _userManager.GetClaimsAsync(user);
        var tenantClaim = claims.FirstOrDefault(c => c.Type == "TenantId");
        if (tenantClaim?.Value == _currentTenantService.GetTenantId().ToString())
        {
            // Check if user is in role
            if (await _userManager.IsInRoleAsync(user, roleName))
            {
                // Map to ContactResponse
                var contact = await _contactRepository.GetByIdAsync(Guid.Parse(user.Id));
                if (contact != null)
                {
                    contactsInRole.Add(new ContactResponse
                    {
                        Id = contact.Id,
                        NationalId = contact.NIC,
                        FullName = contact.FullName,
                        Address = $"{contact.AddressLine1} {contact.AddressLine2}, {contact.City}, {contact.District}",
                        Email = contact.Email,
                        PhoneNumber = contact.PhoneNumber
                    });
                }
            }
        }
    }

    return contactsInRole;
}




    }
}