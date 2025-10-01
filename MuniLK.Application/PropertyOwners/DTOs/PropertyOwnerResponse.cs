using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.PropertyOwners.DTOs
{
    public class PropertyOwnerResponse
    {
        public Guid Id { get; set; }              // PropertyOwner Id
        public Guid PropertyId { get; set; }      // Linked Property Id
        public Guid ContactId { get; set; }       // Linked Contact Id

        public string OwnershipType { get; set; } = string.Empty; // "Primary Owner", etc.

        // Contact details (flattened for easy UI binding)
        public string FullName { get; set; } = string.Empty;
        public string? NIC { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        // Metadata
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
    }

}
