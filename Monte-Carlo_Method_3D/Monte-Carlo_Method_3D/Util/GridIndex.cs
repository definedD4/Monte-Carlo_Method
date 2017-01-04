namespace Monte_Carlo_Method_3D.Util
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
    }
}
