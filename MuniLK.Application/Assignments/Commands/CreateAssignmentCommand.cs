using MediatR;
using MuniLK.Application.Assignments.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.Assignments.Commands
{
    public class CreateAssignmentCommand : IRequest<Guid>
    {
        public CreateAssignmentRequest Request { get; }

        public CreateAssignmentCommand(CreateAssignmentRequest request)
        {
            Request = request;
        }
    }
}
