using Monte_Carlo_Method_3D.Visualization;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Monte_Carlo_Method_3D.Controls
{
    public class Gauge : FrameworkElement
    {
        public Pallete Pallete
        {
            get { return (Pallete)GetValue(PalleteProperty); }
            set { SetValue(PalleteProperty, value); }
        }

        public static readonly DependencyProperty PalleteProperty =
            DependencyProperty.Register("Pallete", typeof(Pallete), typeof(Gauge), new PropertyMetadata(null));

        protected override void OnRender(DrawingContext drawingContext)
        {
            BitmapSource gradient = RenderGradient();

            drawingContext.DrawImage(gradient, new Rect(0, 0, RenderSize.Width / 2, RenderSize.Height));

            int labelCount = 10;
            for (int i = 0; i < labelCount; i++)
            {
                double val = (double)i / labelCount;
                double pos = Math.Pow(val, 1d / 4);

                drawingContext.DrawText(new FormattedText(val.ToString(),
                    CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Segoe UI"), 12d, new SolidColorBrush(Colors.Black)),
                    new Point(RenderSize.Width * 5 / 8, pos * RenderSize.Height));
                drawingContext.DrawLine(new Pen(Brushes.Black, 1d), new Point(0, pos * RenderSize.Height), new Point(RenderSize.Width * 3 / 4, pos * RenderSize.Height));
            }
        }

        private BitmapSource RenderGradient()
        {
            int dpi = 96;
            PixelFormat format = PixelFormats.Bgr24;
            int bytesPerPixel = format.BitsPerPixel / 8;
            int height = (int)RenderSize.Height * 2;

            byte[] pixels = new byte[height * bytesPerPixel];

            for (int i = 0; i < height; i++)
            {
                double val = Math.Pow((double)i / height, 4d);
                Color color = Pallete?.GetColor(val) ?? Colors.Gray;

                pixels[i * bytesPerPixel + 0] = color.B;
                pixels[i * bytesPerPixel + 1] = color.G;
                pixels[i * bytesPerPixel + 2] = color.R;
            }

            BitmapSource gradient = BitmapSource.Create(1, height, dpi, dpi, format, null, pixels, bytesPerPixel);
            return gradient;
        }
    }
}
