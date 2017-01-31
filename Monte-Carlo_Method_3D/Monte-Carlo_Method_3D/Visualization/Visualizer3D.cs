using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using JetBrains.Annotations;
using Monte_Carlo_Method_3D.DataModel;
using Monte_Carlo_Method_3D.Visualization.GraphMesh;
using Monte_Carlo_Method_3D.VisualizationModel;

namespace Monte_Carlo_Method_3D.Visualization
{
    public class Visualizer3D
    {
        private const int Dpi = 96;

        private readonly IGraphMesh m_Mesh;

        public Pallete Pallete { get; set; } = new Pallete();

        public GridSize Size { get; }

        public Visualizer3D(GridSize size, Func<GridSize, IGraphMesh> meshCreator)
        {
            Size = size;

            m_Mesh = meshCreator(size);
        }

        public Model3DVisualization GenerateModel3DVisualization([NotNull] GridData data)
        {
            if (data.Size != Size)
            {
                throw new ArgumentException("Wrong data size provided.");
            }

            var texture = GenerateGridColorImage(data);
            m_Mesh.UpdateMesh(data);
            var brush = new ImageBrush(texture);
            var material = new DiffuseMaterial(brush);
            var model = new GeometryModel3D(m_Mesh.Mesh, material);
            //model.BackMaterial = material;
            return new Model3DVisualization(model);
        }

        // TODO: Remove copied code
        private ImageSource GenerateGridColorImage([NotNull] GridData data)
        {
            var bitmap = new WriteableBitmap(data.Size.Width, data.Size.Height, Dpi, Dpi, PixelFormats.Bgr24, null);
            int bytesPerPixel = bitmap.Format.BitsPerPixel / 8;
            int stride = bitmap.BackBufferStride;

            bitmap.Lock();
            unsafe
            {
                byte* bytes = (byte*)bitmap.BackBuffer.ToPointer();
                foreach (var idx in data.Bounds.EnumerateRegion())
                {
                    DrawingUtil.DrawColorCell(bytes, idx.J, idx.I, Pallete.GetColor(data[idx]), bytesPerPixel, stride);
                }
            }
            bitmap.Unlock();
            RenderOptions.SetBitmapScalingMode(bitmap, BitmapScalingMode.NearestNeighbor);
            bitmap.Freeze();
            return bitmap;
        }
    }
}