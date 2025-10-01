
    using MediatR;
using MuniLK.Application.Documents.DTOs;
using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

namespace MuniLK.Application.Documents.Commands.UploadDocument
{
    public record UploadDocumentCommand(UploadDocumentRequest Request) : IRequest<Guid?>;

}