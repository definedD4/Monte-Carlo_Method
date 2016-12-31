using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Monte_Carlo_Method_3D.Controls
{
    /// <summary>
    /// Логика взаимодействия для ZoomAndPanImage.xaml
    /// </summary>
    public partial class ZoomAndPanImage : UserControl
    {
        private Point m_PanStart;
        private Point m_PanOrigin;

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source", typeof(ImageSource), typeof(ZoomAndPanImage), new PropertyMetadata(default(ImageSource)));

        public ImageSource Source
        {
            get { return (ImageSource) GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty MinZoomProperty = DependencyProperty.Register(
            "MinZoom", typeof(double), typeof(ZoomAndPanImage), new PropertyMetadata(1d));

        public double MinZoom
        {
            get { return (double)GetValue(MinZoomProperty); }
            set { SetValue(MinZoomProperty, value); }
        }

        public static readonly DependencyProperty MaxZoomProperty = DependencyProperty.Register(
            "MaxZoom", typeof(double), typeof(ZoomAndPanImage), new PropertyMetadata(5d));

        public double MaxZoom
        {
            get { return (double) GetValue(MaxZoomProperty); }
            set { SetValue(MaxZoomProperty, value); }
        }

        public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register(
            "Zoom", typeof(double), typeof(ZoomAndPanImage), 
            new FrameworkPropertyMetadata(
                1d,
                FrameworkPropertyMetadataOptions.AffectsMeasure,
                new PropertyChangedCallback(ZoomChanged),
                new CoerceValueCallback(CoerceZoom)));

        private static object CoerceZoom(DependencyObject dependencyObject, object baseValue)
        {
            ZoomAndPanImage zoomAndPanImage = (ZoomAndPanImage) dependencyObject;
            double current = (double)baseValue;
            if (current < zoomAndPanImage.MinZoom) current = zoomAndPanImage.MinZoom;
            if (current > zoomAndPanImage.MaxZoom) current = zoomAndPanImage.MaxZoom;
            return current;
        }

        private static void ZoomChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            dependencyObject.CoerceValue(MinZoomProperty);
            dependencyObject.CoerceValue(MaxZoomProperty);
        }

        public double Zoom
        {
            get { return (double) GetValue(ZoomProperty); }
            set { SetValue(ZoomProperty, value); }
        }

        public static readonly DependencyProperty PanXProperty = DependencyProperty.Register(
            "PanX", typeof(double), typeof(ZoomAndPanImage), new PropertyMetadata(default(double),
                null, new CoerceValueCallback(CoercePanX)));

        private static object CoercePanX(DependencyObject dependencyObject, object baseValue)
        {
            ZoomAndPanImage control = (ZoomAndPanImage) dependencyObject;
            double current = (double) baseValue;
            /*Image img = control.ImageContainer;
            Border border = control.Border;
            double borderWidth = border.RenderSize.Width;
            double imgWidth = img.RenderSize.Width;
            if (borderWidth > imgWidth)
            {
                // Image doesn't fill the screen fully
                Console.WriteLine($"Image doen't fill screen. imgWidth: {imgWidth} borderWidth: {borderWidth}");
            }
            else
            {
                Console.WriteLine($"Image fills screen. imgWidth: {imgWidth} borderWidth: {borderWidth}");
            }*/
            return current;
        }

        public double PanX
        {
            get { return (double) GetValue(PanXProperty); }
            set { SetValue(PanXProperty, value); }
        }

        public static readonly DependencyProperty PanYProperty = DependencyProperty.Register(
            "PanY", typeof(double), typeof(ZoomAndPanImage), new PropertyMetadata(default(double)));

        public double PanY
        {
            get { return (double) GetValue(PanYProperty); }
            set { SetValue(PanYProperty, value); }
        }

        public ZoomAndPanImage()
        {
            InitializeComponent();
        }

        private void ImageContainer_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Zoom += e.Delta > 0 ? .2 : -.2;
        }

        private void ImageContainer_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            m_PanStart = e.GetPosition(Border);
            m_PanOrigin = new Point(PanX, PanY);
            ImageContainer.CaptureMouse();
        }

        private void ImageContainer_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ImageContainer.ReleaseMouseCapture();
        }

        private void ImageContainer_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (ImageContainer.IsMouseCaptured)
            {
                Vector delta = m_PanStart - e.GetPosition(Border);
                Point pan = m_PanOrigin - delta;
                PanX = pan.X;
                PanY = pan.Y;
            }
        }
    }
}
