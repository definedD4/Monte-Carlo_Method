namespace Monte_Carlo_Method_3D.DataModel
{
    public struct GridSize
    {
        public static GridSize Zero => new GridSize(0, 0);

        public int Width { get; }
        public int Height { get; }

        public GridSize(int width, int height)
        {
            Height = height;
            Width = width;
        }

        public bool Equals(GridSize other)
        {
            return Height == other.Height && Width == other.Width;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is GridSize && Equals((GridSize) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Height * 397) ^ Width;
            }
        }

        public static bool operator ==(GridSize left, GridSize right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(GridSize left, GridSize right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"({Height}, {Width})";
        }
    }
}