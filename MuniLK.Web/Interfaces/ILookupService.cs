using MuniLK.Application.Services.DTOs;

namespace MuniLK.Web.Interfaces
{
 public interface ILookupService
    {
        Task<List<LookupDto>> LoadLookupAsync(string category);
        Task<Guid> GetLookupCategoryIdAsync(string categoryName);
        /// <summary>
        /// Retrieves the ID of a lookup value by its category ID and the value string.
        /// </summary>
        /// <param name="lookupCategoryId">The GUID of the lookup category.</param>
        /// <param name="value">The text value of the lookup item.</param>
        /// <returns>The GUID of the lookup item.</returns>
        Task<Guid> GetLookupIdAsync(Guid lookupCategoryId, string value);
        Task<List<LookupDto>> LoadOptionTypesAsync(string CategoryType); // new convenience method
    }
}
