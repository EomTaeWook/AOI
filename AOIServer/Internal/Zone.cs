using Kosher.Sockets.Interface;
using Share;

namespace AOIServer.Internal
{
    internal class Zone
    {
        public int Index { get; private set; }

        private readonly HashSet<User> _players = new();

        private readonly Dictionary<int, HashSet<User>> _cellUserDatas = new();
        private readonly int _cellSize;
        private readonly int _minY;
        private readonly int _maxY;
        private readonly int _maxX;
        private readonly int _minX;
        public Zone(int index, int cellSize, int zoneX, int zoneY)
        {
            Index = index;
            _cellUserDatas = new Dictionary<int, HashSet<User>>();

            _minY = index / zoneY * cellSize;
            _maxY = _minY + cellSize;

            _minX = index % zoneX * cellSize;
            _maxX = _minX + cellSize;

            _cellSize = cellSize;
            var cellIndex = 0;

            for(int i=0; i<_cellSize; ++i)
            {
                for(int ii=0; ii<_cellSize; ++ii)
                {
                    _cellUserDatas.Add(cellIndex, new HashSet<User>());
                    cellIndex++;
                }
            }
        }
        
        public bool CanGo(Vector2Int cellPos)
        {
            if(cellPos.X < _minX || cellPos.X > _maxX)
            {
                return false;
            }
            if (cellPos.Y < _minY || cellPos.Y > _maxY)
            {
                return false;
            }

            return true;
        }
        public void AddCell(Vector2Int cellPos, User user)
        {
            var index = GetCellIndex(cellPos);
            user.Player.CellPos = cellPos;
            _cellUserDatas[index].Add(user);
        }
        public void LeaveCell(User user)
        {
            var index = GetCellIndex(user.Player.CellPos);
            _cellUserDatas[index].Remove(user);
        }

        public int GetCellIndex(Vector2Int vector2Int)
        {
            var xIndex = vector2Int.X - _minX;
            var yIndex = vector2Int.Y - _minY;
            return yIndex * _cellSize + xIndex;
        }

        public void AddUser(User client)
        {
            _players.Add(client);
            //AddCell(client.Player.CellPos, client);

        }
        public void RemoveUser(User client)
        {
            //LeaveCell(client);
            _players.Remove(client);
        }
        public void AllUserBroadcast(IPacket packet)
        {
            foreach(var player in _players)
            {
                player.Session.Send(packet);
            }
        }
        public List<User> GetPlayerByCellIndex(int index)
        {
            var list = new List<User>();

            foreach(var item in _cellUserDatas[index])
            {
                list.Add(item);
            }
            return list;
        }
        public List<int> GetAroundCellByIndex(int index)
        {
            var list = new List<int>();

            var xIndex = index % _cellSize;

            if (xIndex > 0)
            {
                list.Add(index - 1);
            }

            list.Add(index);

            if (xIndex < _cellSize - 1)
            {
                list.Add(index + 1);
            }
            var listX = new List<int>(list);
            for (int i = 0; i < listX.Count; ++i)
            {
                var yIndex = listX[i] / _cellSize;
                if (yIndex > 0)
                {
                    list.Add(listX[i] - _cellSize);
                }

                if (yIndex < _cellSize - 1)
                {
                    list.Add(listX[i] + _cellSize);
                }
            }
            return list;
        }
    }
}
