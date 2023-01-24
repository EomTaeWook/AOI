using Share;
using System.Numerics;

namespace AOIClient.Internal
{
    internal class Map
    {
        public int MaxX { get; set; }

        public int MaxY { get; set; }

        public const int CellSize = 50;
        public int GridIndex(Vector2Int vector2Int)
        {
            return vector2Int.Y * MaxX + vector2Int.X;
        }
    }
}
