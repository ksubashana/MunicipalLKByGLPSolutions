namespace MuniLK.Web.Interfaces
{
    public interface ILanguageService
    {
        event Action OnChange;
        string CurrentCulture { get; }
        Task SetCultureAsync(string culture);
        Task InitializeAsync();
    }
}
