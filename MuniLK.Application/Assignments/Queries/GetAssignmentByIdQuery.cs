using MediatR;
using MuniLK.Application.Assignments.DTOs;
using System;

namespace MuniLK.Application.Assignments.Queries
{
    /// <summary>
    /// Query to fetch a single assignment by Id. Returns null when not found.
    /// </summary>
    public sealed record GetAssignmentByIdQuery(Guid AssignmentId) : IRequest<AssignmentResponse?>;
}
