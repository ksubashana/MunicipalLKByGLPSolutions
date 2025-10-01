using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.Documents.DTOs
{
    /// <summary>
    /// Represents the result of a document download query.
    /// </summary>
    public class DocumentDownloadResult
    {
        public Stream Content { get; set; } = Stream.Null;
        public string ContentType { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
    }
}
