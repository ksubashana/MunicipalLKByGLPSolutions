using Microsoft.AspNetCore.Mvc;
using MediatR;
using MuniLK.Application.Users.DTOs;
using MuniLK.Application.Users.Queries;
using MuniLK.Application.Generic.DTOs;
using MuniLK.Applications.Interfaces;
using MuniLK.Domain.Entities;
using MuniLK.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MuniLK.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;

        public UsersController(IUserRepository userRepository, IMediator mediator)
        {
            _userRepository = userRepository;
            _mediator = mediator;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<User>> GetUserById(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("by-username/{username}")]
        public async Task<ActionResult<User>> GetUserByUsername(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("by-email/{email}")]
        public async Task<ActionResult<User>> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateUser([FromBody] CreateUserRequest request)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Email = request.Email,
                TenantId = request.TenantId
            };

            await _userRepository.AddUserAsync(user, request.Password);

            return Ok(user.Id);
        }

        [HttpPost("{userId:guid}/link-contact/{contactId:guid}")]
        public async Task<ActionResult> LinkContact(Guid userId, Guid contactId)
        {
            await _userRepository.LinkContactAsync(userId, contactId);
            return Ok();
        }

        // NEW: Get all users in current tenant (no role filter)
        [HttpGet("GetUsersByTenant")]
        public async Task<ActionResult<List<UserResponse>>> GetUsersByTenant()
        {
            var list = await _mediator.Send(new GetUsersByTenantQuery());
            return Ok(list);
        }
    }
}
