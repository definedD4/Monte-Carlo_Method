using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monte_Carlo_Method_3D.Simulation;

namespace Monte_Carlo_Method_3D.Util
{
    public struct GridRegion
    {
        public static GridRegion FromSize(GridSize size) => new GridRegion(GridIndex.Zero, size );

        public GridRegion(GridIndex pos, GridSize size)
        {
            if(pos == null)
                throw new ArgumentException("pos can't be null.");

            if (size == null)
                throw new ArgumentException("size can't be null.");

            Pos = pos;
            Size = size;
        }

        public GridIndex Pos { get; }
        public GridSize Size { get; }

        public bool IsInside(GridIndex index)
        {
            return index.I >= Pos.I && index.I < Pos.I + Size.Rows && index.J >= Pos.J && index.J < Pos.J + Size.Columns;
        }

        public GridRegion Shrink(int margin)
        {
            var pos = new GridIndex(Pos.I + margin, Pos.J + margin);
            var size = new GridSize(Size.Rows - 2 * margin, Size.Columns - 2 * margin);
            if (size.Rows < 0 || size.Columns < 0)
                throw new InvalidOperationException("Region cannot shrink.");
            return new GridRegion(pos, size);
        }

        public IEnumerable<GridIndex> EnumerateRegion()
        {
            for(int i = Pos.I; i < Pos.I + Size.Rows; i++)
            {
                for(int j = Pos.J; j < Pos.J + Size.Columns; j++)
                {
                    yield return new GridIndex(i, j);
                }
            }
        }

        public IEnumerable<GridIndex> EnumerateEdge()
        {
            for (int i = Pos.I; i < Pos.I + Size.Rows; i++)
            {
                yield return new GridIndex(i, Pos.J);
                yield return new GridIndex(i, Pos.J + Size.Columns - 1);
            }

            for (int j = Pos.J + 1; j < Pos.J + Size.Columns - 1; j++)
            {
                yield return new GridIndex(Pos.I, j);
                yield return new GridIndex(Pos.I + Size.Rows - 1, j);
            }
        }

        public bool Equals(GridRegion other)
        {
            return Pos.Equals(other.Pos) && Size.Equals(other.Size);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is GridRegion && Equals((GridRegion) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Pos.GetHashCode() * 397) ^ Size.GetHashCode();
            }
        }

        public static bool operator ==(GridRegion left, GridRegion right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GridRegion left, GridRegion right)
        {
            return !left.Equals(right);
        }
    }
}
