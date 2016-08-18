using Monte_Carlo_Method_3D.GraphRendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace Monte_Carlo_Method_3D.Util
{
    public class PointedValueBehavior : Behavior<Image>
    {
        protected override void OnAttached()
        {
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
    
                text.Text = context.GetValueAtImageCoordinates(e.GetPosition(AssociatedObject),
                    new System.Windows.Size(AssociatedObject.ActualWidth, AssociatedObject.ActualHeight)).ToString("E4");
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
