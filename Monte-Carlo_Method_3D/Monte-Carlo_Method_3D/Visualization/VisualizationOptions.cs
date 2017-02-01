using System.Windows;
using System.Windows.Media;
using Monte_Carlo_Method_3D.Visualization.GraphMesh.Factory;

namespace Monte_Carlo_Method_3D.Visualization
{
    public class VisualizationOptions
    {
        public static VisualizationOptions Current { get; set; } = new VisualizationOptions();

        public Pallete Pallete { get; set; } = new Pallete();
        public Color BackgroundColor { get; set; } = Colors.White;

        public Brush ForegroundBrush { get; set; } = new SolidColorBrush(Colors.Black);
        public Typeface TextTypeface { get; set; } = new Typeface("Segoe UI");
        public double TextEmSize { get; set; } = 0.15;

        public bool DrawGrid { get; set; } = true;
        public Pen GridPen { get; set; } = new Pen(new SolidColorBrush(Colors.DarkGray), 0.01D);

        public Pen PathPen { get; set; } = new Pen(new SolidColorBrush(Colors.Black), 0.1D);

        public bool DrawStartPoint { get; set; } = true;

        public Drawing StartPointDrawing { get; set; } = new GeometryDrawing(new SolidColorBrush(Colors.DarkBlue),
            null,
            new EllipseGeometry(new Point(0, 0), 0.2, 0.2));

        public bool DrawEndPoint { get; set; } = true;

        public Drawing EndPointDrawing { get; set; } = new GeometryDrawing(new SolidColorBrush(Colors.Brown),
            null,
            new EllipseGeometry(new Point(0, 0), 0.2, 0.2));

        public GraphMeshKind GraphMeshKind { get; set; } = GraphMeshKind.Histogram;

        /// <summary>
        /// Returns memberwise copy of current options object
        /// </summary>
        /// <returns></returns>
        public VisualizationOptions Copy()
        {
            return (VisualizationOptions)this.MemberwiseClone();
        }
    }
}