// MuniLK.Application.PropertiesLK.DTOs/CreatePropertyOwnerRequest.cs
using System.ComponentModel.DataAnnotations;

namespace MuniLK.Application.PropertyOwners.DTOs
{
    /// <summary>
    /// DTO for creating a new PropertyOwner.
    /// </summary>
    public class CreatePropertyOwnerRequest
    {
        public Guid PropertyId { get; set; }
        public Guid ContactId { get; set; }
        public string OwnershipType { get; set; } = string.Empty;
    }
}
