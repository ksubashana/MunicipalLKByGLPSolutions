using MuniLK.Domain.Entities;
using MuniLK.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Property : IHasTenant
{
    [Key]
    public Guid Id { get; set; }
    public Guid? TenantId { get; set; }

    // Auto-generated short PropertyId for user-friendliness
    [Required]
    [MaxLength(10)]
    public string PropertyId { get; set; } = $"P{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";

    // Navigation property for multiple owners (join entity)
    public ICollection<PropertyOwner> PropertyOwners { get; set; } = new List<PropertyOwner>();

    // Property details
    [Required]
    [MaxLength(500)]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Foreign key to the Lookup table for PropertyType (Category: "PropertyType").
    /// </summary>
    public Guid? PropertyTypeId { get; set; } // Changed from string PropertyType
    public Lookup? PropertyType { get; set; } // Navigation property

    [Required]
    public decimal AssessmentValue { get; set; }

    public int? WardNumber { get; set; }

    /// <summary>
    /// Foreign key to the Lookup table for Zone classification (Category: "Zone").
    /// </summary>
    public Guid? ZoneId { get; set; } // Changed from string Zone
    public Lookup? Zone { get; set; } // Navigation property

    /// <summary>
    /// Foreign key to the Lookup table for Land extent description (Category: "LandExtentUnit").
    /// </summary>
    public Guid? LandExtentId { get; set; } // Changed from string LandExtent
    public Lookup? LandExtent { get; set; } // Navigation property

    public decimal? LandAreaInSqMeters { get; set; }

    [MaxLength(100)]
    public string? TitleDeedNumber { get; set; }

    /// <summary>
    /// Foreign key to the Lookup table for Property Ownership Type (Category: "PropertyOwnershipType").
    /// </summary>
    public Guid? OwnershipTypeId { get; set; } // Changed from string OwnershipType
    public Lookup? OwnershipType { get; set; } // Navigation property (for property's legal ownership type)

    public bool IsCommercialUse { get; set; }

    public int? NumberOfBuildings { get; set; }

    /// <summary>
    /// Foreign key to the Lookup table for Construction Type (Category: "ConstructionType").
    /// </summary>
    public Guid? ConstructionTypeId { get; set; } // Changed from string ConstructionType
    public Lookup? ConstructionType { get; set; } // Navigation property

    /// <summary>
    /// Foreign key to the Lookup table for Road Access Type (Category: "RoadAccessType").
    /// </summary>
    public Guid? RoadAccessTypeId { get; set; } // Changed from string RoadAccessType
    public Lookup? RoadAccessType { get; set; } // Navigation property

    // Location
    /// <summary>
    /// Foreign key to the Lookup table for Grama Niladhari (GS) Division (Category: "GSDivision").
    /// </summary>
    public Guid? GSDivisionId { get; set; } // Changed from string GSDivision
    public Lookup? GSDivision { get; set; } // Navigation property

    /// <summary>
    /// Foreign key to the Lookup table for Electoral Division (Category: "ElectoralDivision").
    /// </summary>
    public Guid? ElectoralDivisionId { get; set; } // Changed from string ElectoralDivision
    public Lookup? ElectoralDivision { get; set; } // Navigation property

    public bool? WaterConnection { get; set; }
    public bool? ElectricityConnection { get; set; }

    public int? LastAssessmentYear { get; set; }
    public bool IsDisputed { get; set; }

    // Optional metadata
    [MaxLength(1000)]
    public string? PhotoUrl { get; set; }

    [MaxLength(100)]
    public string? LocationCoordinates { get; set; } // Format: "latitude,longitude"

    [MaxLength(256)]
    public string? CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? LastModifiedDate { get; set; }
    public string? LastModifiedBy { get; set; }
}

