using MuniLK.Application.PropertyOwners.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.PropertiesLK.DTOs
{
    public class CreatePropertyRequest
    {
        public ICollection<CreatePropertyOwnerRequest> PropertyOwners { get; set; } = new List<CreatePropertyOwnerRequest>();

        [Required]
        [MaxLength(500)]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// The GUID of the PropertyType from Lookup table (Category: "PropertyType").
        /// </summary>
        [Required(ErrorMessage = "Property Type ID is required.")]
        public Guid PropertyTypeId { get; set; }

        [Required]
        public decimal AssessmentValue { get; set; }

        public int? WardNumber { get; set; }

        /// <summary>
        /// The GUID of the Zone from Lookup table (Category: "Zone").
        /// </summary>
        public Guid? ZoneId { get; set; }

        /// <summary>
        /// The GUID of the LandExtent from Lookup table (Category: "LandExtentUnit").
        /// </summary>
        public Guid? LandExtentId { get; set; }

        public decimal? LandAreaInSqMeters { get; set; }

        [MaxLength(100)]
        public string? TitleDeedNumber { get; set; }

        /// <summary>
        /// The GUID of the Property Ownership Type from Lookup table (Category: "PropertyOwnershipType").
        /// </summary>
        public Guid? OwnershipTypeId { get; set; }

        public bool IsCommercialUse { get; set; }

        public int? NumberOfBuildings { get; set; }

        /// <summary>
        /// The GUID of the Construction Type from Lookup table (Category: "ConstructionType").
        /// </summary>
        public Guid? ConstructionTypeId { get; set; }

        /// <summary>
        /// The GUID of the Road Access Type from Lookup table (Category: "RoadAccessType").
        /// </summary>
        public Guid? RoadAccessTypeId { get; set; }

        // Location
        /// <summary>
        /// The GUID of the Grama Niladhari (GS) Division from Lookup table (Category: "GSDivision").
        /// </summary>
        public Guid? GSDivisionId { get; set; }

        /// <summary>
        /// The GUID of the Electoral Division from Lookup table (Category: "ElectoralDivision").
        /// </summary>
        public Guid? ElectoralDivisionId { get; set; }

        public bool? WaterConnection { get; set; }
        public bool? ElectricityConnection { get; set; }

        public int? LastAssessmentYear { get; set; }
        public bool IsDisputed { get; set; }

        [MaxLength(1000)]
        public string? PhotoUrl { get; set; }

        [MaxLength(100)]
        public string? LocationCoordinates { get; set; }

    }

}
