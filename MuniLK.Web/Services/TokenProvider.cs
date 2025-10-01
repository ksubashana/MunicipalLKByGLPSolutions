namespace MuniLK.Web.Services
{
    public class TokenProvider
    {
        private string? _token;
        private DateTime? _tokenExpiry;

        public string? GetToken() => _token;

        public void SetToken(string token) 
        {
            _token = token;
            // Estimate token expiry (access tokens are 15 minutes)
            _tokenExpiry = DateTime.UtcNow.AddMinutes(14); // Slight buffer
        }

        public void ClearToken() 
        {
            _token = null;
            _tokenExpiry = null;
        }

        public bool IsTokenExpired()
        {
            return _tokenExpiry.HasValue && DateTime.UtcNow >= _tokenExpiry.Value;
        }
    }
}
