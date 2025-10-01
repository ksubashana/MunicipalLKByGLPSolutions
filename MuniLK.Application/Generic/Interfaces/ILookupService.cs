// MuniLK.Application/Services/ILookupService.cs
using MuniLK.Application.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MuniLK.Application.Services
{
    /// <summary>
    /// Defines a service for retrieving lookup values, handling global and tenant-specific data.
    /// </summary>
    public interface ILookupService
    {
        /// <summary>
        /// Retrieves a list of active lookup values for a given category (by its programmatic name),
        /// combining global and tenant-specific values.
        /// Tenant-specific values for the same 'Value' will override global ones.
        /// </summary>
        /// <param name="categoryName">The programmatic name of the lookup category (e.g., "PropertyType").</param>
        /// <returns>A list of unique LookupValueDto objects for the current tenant.</returns>
        Task<List<LookupDto>> GetLookupValuesByCategoryNameAsync(string categoryName); // Updated method name and parameter

        /// <summary>
        /// Retrieves a list of active lookup values for a given category (by its ID),
        /// combining global and tenant-specific values.
        /// Tenant-specific values for the same 'Value' will override global ones.
        /// </summary>
        /// <param name="lookupCategoryId">The GUID of the lookup category.</param>
        /// <returns>A list of unique LookupValueDto objects for the current tenant.</returns>
        Task<List<LookupDto>> GetLookupValuesByCategoryIdAsync(Guid lookupCategoryId); // New method to query by ID

        /// <summary>
        /// Adds a new lookup value. Can be global or tenant-specific.
        /// </summary>
        /// <param name="request">The request DTO containing lookup details.</param>
        /// <returns>The Id of the newly created lookup value.</returns>
        Task<Guid> AddLookupValueAsync(AddLookupRequest request); // Changed parameter
        Task<List<Guid>> AddLookupValuesBatchAsync(IEnumerable<AddLookupRequest> requests); 


        /// <summary>
        /// Retrieves all active lookup categories, combining global and tenant-specific ones.
        /// </summary>
        /// <returns>A list of LookupCategoryDto objects.</returns>
        Task<List<LookupCategoryDto>> GetLookupCategoriesAsync();

        Task<Guid?> GetLookupCategoryIdByNameAsync(string name);
        /// <summary>
        /// Retrieves the ID of a specific lookup value based on its category ID and value text.
        /// </summary>
        /// <param name="lookupCategoryId">The GUID of the lookup category.</param>
        /// <param name="value">The text value of the lookup item (e.g., "Photos").</param>
        /// <returns>The GUID of the lookup item, or null if not found.</returns>
        Task<Guid?> GetLookupIdByCategoryIdAndValueAsync(Guid lookupCategoryId, string value);
        /// <summary>
        /// Adds a new lookup category. Can be global or tenant-specific.
        /// </summary>
        /// <param name="request">The request DTO for the new category.</param>
        /// <returns>The Id of the newly created lookup category.</returns>
        Task<Guid> AddLookupCategoryAsync(AddLookupCategoryRequest request);

        /// <summary>
        /// Validates if a given Lookup ID is valid for a specific category and tenant context.
        /// </summary>
        /// <param name="lookupId">The GUID of the lookup value to validate.</param>
        /// <param name="categoryName">The programmatic name of the category it should belong to.</param>
        /// <returns>True if the lookup ID is valid, false otherwise.</returns>
        Task<bool> IsValidLookupIdAsync(Guid lookupId, string categoryName);

        /// <summary>
        /// Gets the 'Value' (e.g., "Passport") of a specific Lookup entry by its ID,
        /// ensuring it belongs to a given category.
        /// </summary>
        /// <param name="lookupId">The ID of the specific Lookup item (e.g., DocumentTypeId).</param>
        /// <param name="lookupCategoryName">The programmatic name of the LookupCategory (e.g., "DocumentType").</param>
        /// <returns>The 'Value' string of the lookup, or null if not found/invalid category.</returns>
        Task<string?> GetLookupValueForCategoryAsync(Guid lookupId, string lookupCategoryName);

        // Optional: Get the full Lookup object if more properties are needed
        // Task<Lookup?> GetLookupByIdAsync(Guid lookupId);
    }
}
