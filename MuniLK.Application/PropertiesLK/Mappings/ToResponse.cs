using MuniLK.Application.PropertiesLK.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.PropertiesLK.Mappings
{
    public static class PropertyMappingExtensions
    {
        public static PropertyResponse ToResponse(this Property property)
        {
            return new PropertyResponse
            {
                Id = property.Id,
                PropertyId = property.PropertyId,
                Address = property.Address,
                TitleDeedNumber = property.TitleDeedNumber,
                AssessmentValue = property.AssessmentValue,
                IsCommercialUse = property.IsCommercialUse,
                // Map other fields as needed
            };
        }
    }

}
