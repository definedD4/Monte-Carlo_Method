using Monte_Carlo_Method_3D.GraphRendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Monte_Carlo_Method_3D.Util
{
    public class PointedValueBehavior : Behavior<Image>
    {
        protected override void OnAttached()
        {
            AssociatedObject.DataContextChanged += (s, e) =>
            {
                var context = AssociatedObject.DataContext as IGridContext;

                if (context == null)
                    return;

                context.DataChanged += (_s, _e) =>
                {
                    var popup = AssociatedObject.FindName("pointedValuePopup") as Popup;
                    var text = AssociatedObject.FindName("pointedValueText") as TextBlock;

                    if (popup == null || text == null)
                        return;

                    if (popup.IsOpen)
                    {
                        var pos = Mouse.GetPosition(AssociatedObject);
                        var size = new System.Windows.Size(AssociatedObject.ActualWidth, AssociatedObject.ActualHeight);
                        text.Text = context.GetValueAtImageCoordinates(new System.Windows.Point(pos.X / size.Width, pos.Y / size.Height)).ToString("E4");
                    }
                };
            };           

            AssociatedObject.MouseMove += (s, e) =>
            {
                var popup = AssociatedObject.FindName("pointedValuePopup") as Popup;
                var text = AssociatedObject.FindName("pointedValueText") as TextBlock;
                var context = AssociatedObject.DataContext as IGridContext;

                if (context == null || popup == null || text == null)
                    return;

                var pos = e.GetPosition(AssociatedObject);

                if (!popup.IsOpen) { popup.IsOpen = true; }

                popup.HorizontalOffset = pos.X + 20;
                popup.VerticalOffset = pos.Y;

                var size = new System.Windows.Size(AssociatedObject.ActualWidth, AssociatedObject.ActualHeight);
                text.Text = context.GetValueAtImageCoordinates(new System.Windows.Point(pos.X / size.Width, pos.Y / size.Height)).ToString("E4");
            };

            AssociatedObject.MouseLeave += (s, e) =>
            {
                var popup = AssociatedObject.FindName("pointedValuePopup") as Popup;

                if (popup == null)
                    return;

                popup.IsOpen = false;
            };
        }
    }
}
