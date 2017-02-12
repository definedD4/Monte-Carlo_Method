using System.Windows;
using System.Windows.Controls;

namespace Monte_Carlo_Method_3D.Controls
{
    public class AdvancedTabControl : TabControl
    {
        static AdvancedTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(AdvancedTabControl),
                new FrameworkPropertyMetadata(typeof(AdvancedTabControl)));
        }

        public object AdditionalContent
        {
            get { return (object)GetValue(AdditionalContentProperty); }
            set { SetValue(AdditionalContentProperty, value); }
        }

        public static readonly DependencyProperty AdditionalContentProperty =
            DependencyProperty.Register("AdditionalContent", typeof(object), typeof(AdvancedTabControl), new PropertyMetadata(0));
    }
}
