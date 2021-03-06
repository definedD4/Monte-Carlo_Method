﻿using System.Windows;
using System.Windows.Media;
using Monte_Carlo_Method_3D.Visualization.GraphMesh.Factory;
using Newtonsoft.Json;

namespace Monte_Carlo_Method_3D.Visualization
{
    [JsonObject(MemberSerialization.OptIn)]
    public class VisualizationOptions
    {
        public Pallete Pallete { get; set; } = new Pallete();

        [JsonProperty]
        public Color BackgroundColor { get; set; } = Colors.White;

        [JsonProperty]
        public Color ForegroundColor { get; set; } = Colors.Black;

        public Brush ForegroundBrush => new SolidColorBrush(ForegroundColor);

        public Typeface TextTypeface { get; set; } = new Typeface("Segoe UI");

        [JsonProperty]
        public double TextEmSize { get; set; } = 0.2;

        [JsonProperty]
        public bool DrawGrid { get; set; } = true;

        [JsonProperty]
        public Color GridColor { get; set; } = Colors.DarkGray;

        [JsonProperty]
        public double GridThickness { get; set; } = 0.01D;

        public Pen GridPen => new Pen(new SolidColorBrush(GridColor), GridThickness);

        [JsonProperty]
        public Color PathColor { get; set; } = Colors.Black;

        [JsonProperty]
        public double PathThickness { get; set; } = 0.1D;

        public Pen PathPen => new Pen(new SolidColorBrush(PathColor), PathThickness);

        [JsonProperty]
        public bool DrawStartPoint { get; set; } = true;

        [JsonProperty]
        public Color StartPointColor { get; set; } = Colors.DarkBlue;

        public Drawing StartPointDrawing => new GeometryDrawing(
            new SolidColorBrush(StartPointColor),
            null,
            new EllipseGeometry(new Point(0, 0), 0.2, 0.2));

        [JsonProperty]
        public bool DrawEndPoint { get; set; } = true;

        [JsonProperty]
        public Color EndPointColor { get; set; } = Colors.Brown;

        public Drawing EndPointDrawing => new GeometryDrawing(
            new SolidColorBrush(EndPointColor),
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