namespace ShareModel
{
    public class Vector2Int
    {
        public int X { get; set; }

        public int Y { get; set; }

        public Vector2Int()
        { }
        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vector2Int Up { get { return new Vector2Int(0, 1); } }
        public static Vector2Int Left { get { return new Vector2Int(-1, 0); } }
        public static Vector2Int Right { get { return new Vector2Int(1, 0); } }
        public static Vector2Int Down { get { return new Vector2Int(0, -1); } }
        public static Vector2Int operator +(Vector2Int left, Vector2Int right)
        {
            return new Vector2Int(left.X + right.X, left.Y + right.Y);
        }

    }
}
