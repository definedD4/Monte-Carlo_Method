namespace Monte_Carlo_Method_3D.Util
{
    public struct IntPoint
    {
        public IntPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }

        public static IntPoint operator +(IntPoint left, IntPoint right)
        {
            return new IntPoint(left.X + right.X, left.Y + right.Y);
        }

        public bool InBoundsStrict(int xStart, int xEnd, int yStart, int yEnd)
        {
            return X > xStart && X < xEnd && Y > yStart && Y < yEnd;
        }

        public bool InBounds(int xStart, int xEnd, int yStart, int yEnd)
        {
            return X >= xStart && X <= xEnd && Y >= yStart && Y <= yEnd;
        }
    }
}
