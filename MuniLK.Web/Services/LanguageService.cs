using Microsoft.JSInterop;
using MuniLK.Web.Interfaces;

namespace MuniLK.Web.Services
{
    public class LanguageService : ILanguageService
    {
        private string currentCulture = "en";
        private readonly IJSRuntime _jsRuntime;
        public LanguageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }
        public string CurrentCulture => currentCulture;

        public event Action OnChange;
        public async Task InitializeAsync()
        {
            var savedCulture = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "appCulture");
            if (!string.IsNullOrEmpty(savedCulture))
            {
                currentCulture = savedCulture;
            }
            else
            {
                currentCulture = "en";
            }
        }
        public async Task SetCultureAsync(string culture)
        {
            if (culture != currentCulture)
            {
                currentCulture = culture;
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "appCulture", culture);
                OnChange?.Invoke();
                await Task.CompletedTask;
            }
        }
    }
}
