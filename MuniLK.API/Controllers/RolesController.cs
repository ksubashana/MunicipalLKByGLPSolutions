using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MuniLK.Application.Generic.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            try
            {
                await _roleService.CreateRoleAsync(roleName);
                return Ok($"Role {roleName} created successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{roleName}")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            try
            {
                await _roleService.DeleteRoleAsync(roleName);
                return Ok($"Role {roleName} deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AddUserToRole([FromBody] UserRoleDto dto)
        {
            try
            {
                await _roleService.AddUserToRoleAsync(dto.UserId, dto.RoleName);
                return Ok($"User {dto.UserId} assigned to role {dto.RoleName}.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveUserFromRole([FromBody] UserRoleDto dto)
        {
            try
            {
                await _roleService.RemoveUserFromRoleAsync(dto.UserId, dto.RoleName);
                return Ok($"User {dto.UserId} removed from role {dto.RoleName}.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = await _roleService.GetAllRolesAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            try
            {
                var roles = await _roleService.GetUserRolesAsync(userId);
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUsersByTenantAndRole")]
        public async Task<ActionResult<List<IdentityUser>>> GetUsersByTenantAndRole( string roleName)
        {
            var users = await _roleService.GetContactsByTenantAndRoleAsync( roleName);
            return Ok(users);
        }
    }

    public class UserRoleDto
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }
    }
}