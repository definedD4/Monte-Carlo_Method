namespace Monte_Carlo_Method_3D.Util
{
    public class GridSize
    {
        public int Rows { get; }
        public int Columns { get; }

        public GridSize(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
        }

        protected bool Equals(GridSize other)
        {
            return Rows == other.Rows && Columns == other.Columns;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GridSize) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Rows * 397) ^ Columns;
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
            return $"({Rows}, {Columns})";
        }
    }
}