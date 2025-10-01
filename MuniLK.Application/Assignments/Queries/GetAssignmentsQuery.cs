using MediatR;
using MuniLK.Application.Assignments.DTOs;
using MuniLK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.Assignments.Queries
{
    public record GetAssignmentsQuery(Guid? ModuleId, Guid? EntityId,Guid? TenantId = null)
        : IRequest<List<AssignmentResponse>>;
}
