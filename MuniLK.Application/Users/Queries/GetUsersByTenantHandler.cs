using MediatR;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Application.Users.DTOs;
using MuniLK.Application.Users.Queries;
using MuniLK.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MuniLK.Application.Users.Queries
{
    public class GetUsersByTenantHandler : IRequestHandler<GetUsersByTenantQuery, List<UserResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentTenantService _currentTenantService;
        private readonly IRoleService _roleService;

        public GetUsersByTenantHandler(
            IUserRepository userRepository,
            ICurrentTenantService currentTenantService,
            IRoleService roleService)
        {
            _userRepository = userRepository;
            _currentTenantService = currentTenantService;
            _roleService = roleService;
        }

        public async Task<List<UserResponse>> Handle(GetUsersByTenantQuery request, CancellationToken ct)
        {
            var tenantId = _currentTenantService.GetTenantId();
            if (!tenantId.HasValue)
                return new List<UserResponse>();

            var users = await _userRepository.GetUsersByTenantAsync(tenantId.Value);

            var result = new List<UserResponse>(users.Count);
            foreach (var u in users)
            {
                // Identity user id matches domain User.Id (Guid) as string
                var roles = await _roleService.GetUserRolesAsync(u.Id.ToString());
                var roleStr = roles?.Any() == true ? string.Join(",", roles) : string.Empty;

                result.Add(new UserResponse
                {
                    Id = u.Id.ToString(),
                    UserName = u.Username,
                    Email = u.Email,
                    Role = roleStr
                });
            }

            return result;
        }
    }
}