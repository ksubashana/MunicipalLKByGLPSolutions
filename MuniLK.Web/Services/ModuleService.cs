using MuniLK.Application.Services.DTOs;
using MuniLK.Web.Interfaces;

namespace MuniLK.Web.Services
{
    public class ModuleService : IModuleService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ModuleService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Guid> GetModuleIdByCodeAsync(string Code)
        {
            var client = _httpClientFactory.CreateClient("AuthorizedClient");


                Console.WriteLine($"Fetching category ID for: {Code}");
                var result = await client.GetFromJsonAsync<Guid>($"api/Modules/id-by-code/{Code}");
                return result;
            

        }
    }
}
