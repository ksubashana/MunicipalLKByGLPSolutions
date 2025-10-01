using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.BuildingAndPlanning.DTOs
{
    public class ApplicationDocumentResponse
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = default!;
        public string BlobPath { get; set; } = default!;
        public string DocumentType { get; set; } = default!;
        public DateTime UploadedAt { get; set; }
    }
}
