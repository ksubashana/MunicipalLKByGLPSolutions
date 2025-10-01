using MediatR;
using MuniLK.Application.Assignments.DTOs;

namespace MuniLK.Application.Assignments.Commands
{
    public record CompleteAssignmentCommand(CompleteAssignmentRequest Request) : IRequest<bool>;
}