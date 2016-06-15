using Monte_Carlo_Method_3D.GraphRendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Monte_Carlo_Method_3D.Util
{
    public class PointedValueBehavior : Behavior<Image>
    {
        protected override void OnAttached()
        {
            AssociatedObject.MouseMove += (s, e) =>
            {
                VisualContext2D context = AssociatedObject.DataContext as VisualContext2D;
                if (context == null)
                    throw new InvalidOperationException("Cannot retrive data context.");

                context.PointedValue = context.GetValueAtImageCoordinates(e.GetPosition(AssociatedObject),
                    new System.Windows.Size(AssociatedObject.ActualWidth, AssociatedObject.ActualHeight));
            };
        }
    }
}
