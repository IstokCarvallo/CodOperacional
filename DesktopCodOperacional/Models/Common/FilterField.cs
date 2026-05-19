using CommunityToolkit.Mvvm.ComponentModel;

namespace DesktopCodOperacional.Models.Common
{
    public partial class FilterField : ObservableObject
    {
        [ObservableProperty]
        private string label = string.Empty;

        [ObservableProperty]
        private string propertyName = string.Empty;

        [ObservableProperty]
        private string value = string.Empty;

        partial void OnValueChanged(string value)
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler? ValueChanged;
    }
}
