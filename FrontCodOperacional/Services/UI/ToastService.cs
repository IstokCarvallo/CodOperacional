namespace FrontCodOperacional.Services.UI
{
    public class ToastService
    {
        public event Action<string, string>? OnShow;

        public void IsSuccess(string message) => OnShow?.Invoke(message, "IsSuccess");
        public void Error(string message) => OnShow?.Invoke(message, "error");
        public void Info(string message) => OnShow?.Invoke(message, "info");
    }
}
