// MuniLK.Application.Documents.Commands.DeleteDocument/DeleteDocumentCommand.cs
using MediatR;
using System;

namespace MuniLK.Application.Documents.Commands.DeleteDocument
{
    /// <summary>
    /// Command to delete a specific document by its ID.
    /// </summary>
    public class DeleteDocumentCommand : IRequest<Unit> // Unit indicates a void return type from MediatR
    {
        public Guid Id { get; set; }

        public DeleteDocumentCommand(Guid id)
        {
            Id = id;
        }
    }
}