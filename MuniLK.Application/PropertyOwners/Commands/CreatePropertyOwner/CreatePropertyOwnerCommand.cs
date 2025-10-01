using MediatR;
using MuniLK.Application.PropertyOwners.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.PropertyOwners.Commands.CreatePropertyOwner
{
    public record CreatePropertyOwnerCommand(CreatePropertyOwnerRequest Request)
        : IRequest<PropertyOwnerResponse>;
}
