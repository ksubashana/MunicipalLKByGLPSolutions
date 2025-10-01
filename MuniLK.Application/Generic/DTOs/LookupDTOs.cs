// MuniLK.Application/Services/DTOs/LookupValueDto.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace MuniLK.Application.Services.DTOs
{
    /// <summary>
    /// DTO for representing a detailed lookup value.
    /// </summary>
    public class LookupDto
    {
        public Guid Id { get; set; }
        public Guid LookupCategoryId { get; set; } // The ID of the category this lookup belongs to
        public string CategoryName { get; set; } = string.Empty; // The programmatic name of the category (e.g., "PropertyType")
        public string CategoryDisplayName { get; set; } = string.Empty; // The human-readable name of the category
        public string Value { get; set; } = string.Empty;
        public int Order { get; set; }
        public Guid? TenantId { get; set; } // Null for global values
        public bool IsActive { get; set; }
    }

    public class LookupCategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty; // Programmatic name
        public string DisplayName { get; set; } = string.Empty; // Human-readable name
        public string? Description { get; set; }
        public int Order { get; set; }
        public Guid? TenantId { get; set; } // Null for global categories
        public bool IsActive { get; set; }
    }

    public class AddLookupRequest
    {
        /// <summary>
        /// The GUID of the LookupCategory this value belongs to.
        /// </summary>
        [Required(ErrorMessage = "LookupCategoryId is required.")]
        public Guid LookupCategoryId { get; set; }

        /// <summary>
        /// The actual value of the lookup item (e.g., "Primary Owner", "Residential").
        /// </summary>
        [Required(ErrorMessage = "Value is required.")]
        [MaxLength(256)]
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// Indicates if this lookup value is global (true) or tenant-specific (false).
        /// </summary>
        public bool IsGlobal { get; set; } = false;

        /// <summary>
        /// Optional: Display order for the value within its category.
        /// </summary>
        public int Order { get; set; } = 0;
    }

    public class AddLookupCategoryRequest
    {
        /// <summary>
        /// The unique programmatic name of the category (e.g., "PropertyType").
        /// </summary>
        [Required(ErrorMessage = "Category Name is required.")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// A human-readable display name for the category (e.g., "Property Types").
        /// </summary>
        [Required(ErrorMessage = "Display Name is required.")]
        [MaxLength(256)]
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Optional: A description for the category.
        /// </summary>
        [MaxLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Optional: Display order for the category in a list.
        /// </summary>
        public int Order { get; set; } = 0;

        /// <summary>
        /// Indicates if this category is global (true) or tenant-specific (false).
        /// </summary>
        public bool IsGlobal { get; set; } = false;
    }
}
