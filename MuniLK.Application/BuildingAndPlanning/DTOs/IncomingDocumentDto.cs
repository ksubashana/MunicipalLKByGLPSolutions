using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.BuildingAndPlanning.DTOs
{
    public class IncomingDocumentDto
    {
        public string FileName { get; set; } = default!;
        public string BlobPath { get; set; } = default!; // you said blob is already handled by API, so we assume you pass it here
        public string DocumentType { get; set; } = default!;
    }
}
