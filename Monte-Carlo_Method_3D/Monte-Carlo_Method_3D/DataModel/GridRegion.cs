using System;
using System.Collections.Generic;

namespace Monte_Carlo_Method_3D.DataModel
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
            return index.I >= Pos.I && index.I < Pos.I + Size.Height && index.J >= Pos.J && index.J < Pos.J + Size.Width;
        }

        public GridRegion Shrink(int margin)
        {
            var pos = new GridIndex(Pos.I + margin, Pos.J + margin);
            var size = new GridSize(Size.Width - 2 * margin, Size.Height - 2 * margin);
            if (size.Height < 0 || size.Width < 0)
                throw new InvalidOperationException("Region cannot shrink.");
            return new GridRegion(pos, size);
        }

        public IEnumerable<GridIndex> EnumerateRegion()
        {
            for(int i = Pos.I; i < Pos.I + Size.Height; i++)
            {
                for(int j = Pos.J; j < Pos.J + Size.Width; j++)
                {
                    yield return new GridIndex(i, j);
                }
            }
        }

        public IEnumerable<GridIndex> EnumerateEdge()
        {
            for (int i = Pos.I; i < Pos.I + Size.Height; i++)
            {
                yield return new GridIndex(i, Pos.J);
                yield return new GridIndex(i, Pos.J + Size.Width - 1);
            }

            for (int j = Pos.J + 1; j < Pos.J + Size.Width - 1; j++)
            {
                yield return new GridIndex(Pos.I, j);
                yield return new GridIndex(Pos.I + Size.Height - 1, j);
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
