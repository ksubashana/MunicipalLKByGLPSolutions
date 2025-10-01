using Microsoft.AspNetCore.Identity;
using MuniLK.Application.Contact.DTOs;
using MuniLK.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace MuniLK.Application.Generic.Interfaces
{
    public interface IRoleService
    {
        Task CreateRoleAsync(string roleName);
        Task DeleteRoleAsync(string roleName);
        Task AddUserToRoleAsync(string userId, string roleName);
        Task RemoveUserFromRoleAsync(string userId, string roleName);
        Task<List<string>> GetAllRolesAsync();
        Task<List<string>> GetUserRolesAsync(string userId);
        Task<List<ContactResponse>> GetContactsByTenantAndRoleAsync(string roleName);

    }
}