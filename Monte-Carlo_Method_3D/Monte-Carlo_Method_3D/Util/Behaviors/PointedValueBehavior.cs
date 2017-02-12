using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using Monte_Carlo_Method_3D.VisualizationModel;

namespace Monte_Carlo_Method_3D.Util
{
    public class PointedValueBehavior : Behavior<Image>
    {
        public static readonly DependencyProperty VisualizationProperty = DependencyProperty.Register(
            nameof(Visualization), typeof(IGridVisualization), typeof(PointedValueBehavior), new PropertyMetadata(default(IGridVisualization)));

        public IGridVisualization Visualization
        {
            get { return (IGridVisualization) GetValue(VisualizationProperty); }
            set { SetValue(VisualizationProperty, value); }
        }

        protected override void OnAttached()
        {         
            AssociatedObject.MouseMove += (s, e) =>
            {
                var popup = AssociatedObject.FindName("pointedValuePopup") as Popup;
                var text = AssociatedObject.FindName("pointedValueText") as TextBlock;

                if (Visualization == null || popup == null || text == null)
                    return;

                var pos = e.GetPosition(AssociatedObject);
                var size = new System.Windows.Size(AssociatedObject.ActualWidth, AssociatedObject.ActualHeight);

                var val =
                    Visualization.GetValueAtRelativeCoords(new System.Windows.Point(pos.X / size.Width,
                        pos.Y / size.Height));

                if (val.HasValue)
                {
                    if (!popup.IsOpen)
                    {
                        popup.IsOpen = true;
                    }

                    popup.HorizontalOffset = pos.X + 20;
                    popup.VerticalOffset = pos.Y;

                    text.Text = val.Value.ToString("E4");
                }
                else
                {
                    popup.IsOpen = false;
                }
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
