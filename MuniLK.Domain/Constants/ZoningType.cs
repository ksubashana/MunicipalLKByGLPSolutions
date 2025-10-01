using System.ComponentModel.DataAnnotations;

namespace MuniLK.Domain.Constants
{
    /// <summary>
    /// Zoning types for property classification
    /// </summary>
    public enum ZoningType
    {
        [Display(Name = "Residential")]
        Residential = 1,

        [Display(Name = "Commercial")]
        Commercial = 2,

        [Display(Name = "Industrial")]
        Industrial = 3,

        [Display(Name = "Mixed")]
        Mixed = 4
    }
}