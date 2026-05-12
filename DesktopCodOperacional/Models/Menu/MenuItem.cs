using System.Collections.ObjectModel;

namespace DesktopCodOperacional.Models.Menu
{
    public class MenuItemModel
    {
        public string Title { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;

        public string ViewName { get; set; } = string.Empty;

        public ObservableCollection<MenuItemModel> Children { get; set; } = new();
    }
}
