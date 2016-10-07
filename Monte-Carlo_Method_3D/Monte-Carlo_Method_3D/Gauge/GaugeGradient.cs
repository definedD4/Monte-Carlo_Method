using Monte_Carlo_Method_3D.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Monte_Carlo_Method_3D.Gauge
{
    public class GaugeGradient : FrameworkElement
    {
        public Pallete Pallete
        {
            get { return (Pallete)GetValue(PalleteProperty); }
            set { SetValue(PalleteProperty, value); }
        }

        public static readonly DependencyProperty PalleteProperty =
            DependencyProperty.Register("Pallete", typeof(Pallete), typeof(GaugeGradient), new PropertyMetadata(null));

        protected override void OnRender(DrawingContext drawingContext)
        {
            int dpi = 96;
            PixelFormat format = PixelFormats.Bgr24;
            int bytesPerPixel = format.BitsPerPixel / 8;
            int height = (int)RenderSize.Height * 2;

            byte[] pixels = new byte[height * bytesPerPixel];

            for (int i = 0; i < height; i++)
            {
                double val = (double)i / height;
                Color color = Pallete?.GetColor(val)??Colors.Gray;

                pixels[i * bytesPerPixel + 0] = color.B;
                pixels[i * bytesPerPixel + 1] = color.G;
                pixels[i * bytesPerPixel + 2] = color.R;
            }

            BitmapSource gradient = BitmapSource.Create(1, height, dpi, dpi, format, null, pixels, bytesPerPixel);

            drawingContext.DrawImage(gradient, new Rect(0, 0, RenderSize.Width, RenderSize.Height));
        }
    }
}
