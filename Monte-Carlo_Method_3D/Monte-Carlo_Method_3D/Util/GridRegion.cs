﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monte_Carlo_Method_3D.Util
{
    public struct GridRegion
    {
        public GridRegion(GridIndex pos, GridSize size)
        {
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
    }
}