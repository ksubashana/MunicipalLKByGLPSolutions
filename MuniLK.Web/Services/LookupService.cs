using MuniLK.Application.Services.DTOs;
using MuniLK.Web.Interfaces;

namespace MuniLK.Web.Services
{
    public class LookupService : ILookupService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LookupService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<LookupDto>> LoadLookupAsync(string category)
        {
            var client = _httpClientFactory.CreateClient("AuthorizedClient");

            try
            {
                Console.WriteLine($"Fetching lookups for: {category}");
                var result = await client.GetFromJsonAsync<List<LookupDto>>($"api/lookups/values/byname/{category}");
                return result ?? new List<LookupDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading {category}: {ex.Message}");
                return new List<LookupDto>();
            }
        }

        public async Task<Guid> GetLookupCategoryIdAsync(string categoryName)
        {
            var client = _httpClientFactory.CreateClient("AuthorizedClient");


                Console.WriteLine($"Fetching category ID for: {categoryName}");
                var result = await client.GetFromJsonAsync<Guid>($"api/lookups/categories/id/byname/{categoryName}");
                return result;
            

        }
        public async Task<Guid> GetLookupIdAsync(Guid lookupCategoryId, string value)
        {
            var client = _httpClientFactory.CreateClient("AuthorizedClient");

            try
            {
                Console.WriteLine($"Fetching lookup ID for CategoryId: {lookupCategoryId} and Value: {value}");
                var result = await client.GetFromJsonAsync<Guid>($"api/lookups/id/bycategoryandvalue/{lookupCategoryId}/{value}");
                return result;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching lookup ID: {ex.Message}");
                // You can re-throw or handle the exception based on your application's needs
                throw;
            }
        }

        // New convenience wrapper for ClearanceTypes category
        public async Task<List<LookupDto>> LoadOptionTypesAsync(string CategoryType)
            => await LoadLookupAsync(CategoryType);
    }
}
