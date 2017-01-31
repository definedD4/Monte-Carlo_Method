namespace Monte_Carlo_Method_3D.DataModel
{
    public struct GridIndex
    {
        public static GridIndex Zero => new GridIndex(0, 0);

        public GridIndex(int i, int j)
        {
            I = i;
            J = j;
        }

        public int I { get; }
        public int J { get; }

        public static GridIndex operator +(GridIndex left, GridIndex right) => new GridIndex(left.I + right.I, left.J + right.J);

        public GridIndex Right() =>       new GridIndex(I    , J + 1);
        public GridIndex TopRight() =>    new GridIndex(I - 1, J + 1);
        public GridIndex Top() =>         new GridIndex(I - 1, J    );
        public GridIndex TopLeft() =>     new GridIndex(I - 1, J - 1);
        public GridIndex Left() =>        new GridIndex(I    , J - 1);
        public GridIndex BottomLeft() =>  new GridIndex(I + 1, J - 1);
        public GridIndex Bottom() =>      new GridIndex(I + 1, J    );
        public GridIndex BottomRight() => new GridIndex(I + 1, J + 1);

        /// <summary>
        /// Returns position of indexed element in array as if it was a bitmap.
        /// </summary>
        /// <param name="stride">Data row size</param>
        /// <param name="positionsPerIndex">Data item size</param>
        /// <returns></returns>
        public int Offset(int stride, int positionsPerIndex = 1) => I * stride + J * positionsPerIndex;

        public int Offset(GridSize size) => Offset(size.Width);

        public bool Equals(GridIndex other)
        {
            return I == other.I && J == other.J;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is GridIndex && Equals((GridIndex) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (I * 397) ^ J;
            }
        }

        public static bool operator ==(GridIndex left, GridIndex right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GridIndex left, GridIndex right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"({I}, {J})";
        }
    }
}
