using Share;

namespace AOIServer.Internal
{
    internal class Map
    {
        private readonly Dictionary<int, Zone> _zoneManager = new();
        public const int CellSize = 50;
        public int MaxXCellSize { get; private set; }
        public int MaxYCellSize { get; private set; }

        public int MaxX { get; private set; }
        public int MaxY { get; private set; }
        
        public Zone GetZone(int index)
        {
            _zoneManager.TryGetValue(index, out Zone zone);
            return zone;
        }
        public Map(int maxZoneX, int maxZoneY)
        {
            var id = 0;
            for(int i=0; i< maxZoneY; ++i )
            {
                for(int ii=0; ii< maxZoneX; ++ii)
                {
                    _zoneManager.Add(id, new Zone(id, CellSize, maxZoneX, maxZoneY));
                    id++;
                }
            }
            MaxXCellSize = maxZoneX * CellSize;
            MaxYCellSize = maxZoneY * CellSize;

            MaxX = maxZoneX;
            MaxY = maxZoneY;
        }
        public int GetZoneIndex(Vector2Int vector2Int)
        {
            var xIndex = vector2Int.X / CellSize;
            var yIndex = vector2Int.Y / CellSize;
            return yIndex * MaxX + xIndex;
        }
        public List<int> GetNeighborZoneByIndex(int index)
        {
            var list = new List<int>();

            if (_zoneManager.ContainsKey(index) == false)
            {
                return list;
            }

            list.Add(index);

            var xIndex = index % MaxX;

            if (xIndex > 0)
            {
                list.Add(index - 1);
            }
            if(xIndex < MaxX - 1)
            {
                list.Add(index + 1);
            }
            var listX = new List<int>(list);
            for(int i=0; i<listX.Count; ++i)
            {
                var yIndex = listX[i] / MaxY;
                if(yIndex > 0)
                {
                    list.Add(listX[i] - MaxY);
                }

                if(yIndex < MaxY -1)
                {
                    list.Add(listX[i] + MaxY);
                }
            }
            return list;
        }
    }
}
