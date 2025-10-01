using MediatR;
using MuniLK.Application.Users.DTOs;
using System.Collections.Generic;

namespace MuniLK.Application.Users.Queries
{
    public record GetUsersByTenantQuery() : IRequest<List<UserResponse>>;
}