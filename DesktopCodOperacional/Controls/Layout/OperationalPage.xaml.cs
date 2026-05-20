using System.Windows;
using System.Windows.Controls;

namespace DesktopCodOperacional.Controls.Layout
{
    public partial class OperationalPage : UserControl
    {
        public OperationalPage()
        {
            InitializeComponent();
        }

        public object FiltersContent
        {
            get => GetValue(FiltersContentProperty);
            set => SetValue(FiltersContentProperty, value);
        }

        public static readonly DependencyProperty FiltersContentProperty =
            DependencyProperty.Register(
                nameof(FiltersContent),
                typeof(object),
                typeof(OperationalPage));

        public object ToolbarContent
        {
            get => GetValue(ToolbarContentProperty);
            set => SetValue(ToolbarContentProperty, value);
        }

        public static readonly DependencyProperty ToolbarContentProperty =
            DependencyProperty.Register(
                nameof(ToolbarContent),
                typeof(object),
                typeof(OperationalPage));

        public object BodyContent
        {
            get => GetValue(BodyContentProperty);
            set => SetValue(BodyContentProperty, value);
        }

        public static readonly DependencyProperty BodyContentProperty =
            DependencyProperty.Register(
                nameof(BodyContent),
                typeof(object),
                typeof(OperationalPage));

        public object FooterContent
        {
            get => GetValue(FooterContentProperty);
            set => SetValue(FooterContentProperty, value);
        }

        public static readonly DependencyProperty FooterContentProperty =
            DependencyProperty.Register(
                nameof(FooterContent),
                typeof(object),
                typeof(OperationalPage));

        public object EmptyContent
        {
            get => GetValue(EmptyContentProperty);
            set => SetValue(EmptyContentProperty, value);
        }

        public static readonly DependencyProperty EmptyContentProperty =
            DependencyProperty.Register(
                nameof(EmptyContent),
                typeof(object),
                typeof(OperationalPage));

        public object LoadingContent
        {
            get => GetValue(LoadingContentProperty);
            set => SetValue(LoadingContentProperty, value);
        }

        public static readonly DependencyProperty LoadingContentProperty =
            DependencyProperty.Register(
                nameof(LoadingContent),
                typeof(object),
                typeof(OperationalPage));
    }
}
