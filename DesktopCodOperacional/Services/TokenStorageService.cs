namespace DesktopCodOperacional.Services
{
    public class TokenStorageService
    {
        private string _accessToken = string.Empty;

        private string _refreshToken = string.Empty;

        public void SetTokens(
        string accessToken,
        string refreshToken)
        {
            _accessToken = accessToken;
            _refreshToken = refreshToken;
        }

        public string GetAccessToken()
        {
            return _accessToken;
        }

        public string GetRefreshToken()
        {
            return _refreshToken;
        }

        public bool HasToken()
        {
            return !string.IsNullOrWhiteSpace(_accessToken);
        }

        public void Clear()
        {
            _accessToken = string.Empty;
            _refreshToken = string.Empty;
        }
    }
}
