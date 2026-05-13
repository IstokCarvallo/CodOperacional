using CommunityToolkit.Mvvm.ComponentModel;

namespace DesktopCodOperacional.ViewModels.Base
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [ObservableProperty]
        private bool hasError;

        protected void SetError(string message)
        {
            ErrorMessage = message;
            HasError = true;
        }

        protected void ClearError()
        {
            ErrorMessage = string.Empty;
            HasError = false;
        }
    }
}
