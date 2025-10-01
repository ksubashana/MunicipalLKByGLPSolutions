// MuniLK.Application/Contracts/Contact/UpdateContactRequest.cs
using System;

namespace MuniLK.Application.Contact.DTOs
{
    public class UpdateContactRequest
    {
        // No Id here, as the Id will be passed in the command itself or URL
        public string FullName { get; set; }
        public string NIC { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public bool IsActive { get; set; } // Allow updating active status
    }
}