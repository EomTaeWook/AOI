﻿using System.Collections.Concurrent;

namespace Share
{
    public class Map
    {
        private readonly int _minY;
        private readonly int _maxY;
        private readonly int _maxX;
        private readonly int _minX;

        private readonly int _xCellSize;
        private readonly int _yCellSize;

        public int GetMaxX()
        {
            return _maxX;
        }
        public int GetMaxY()
        {
            return _maxX;
        }

        private readonly ConcurrentDictionary<int, HashSet<Player>> _cellPlayerDatas = new();
        public Map(int minX, int maxX, int minY, int maxY)
        {
            _minY = minY;
            _maxY = maxY;
            _minX = minX;
            _maxX = maxX;

            _cellPlayerDatas = new ConcurrentDictionary<int, HashSet<Player>>();
            var cellIndex = 0;
            for (int i = _minX; i < _maxX; ++i)
            {
                for (int ii = _minY; ii < _maxY; ++ii)
                {
                    _cellPlayerDatas.TryAdd(cellIndex, new HashSet<Player>());
                    cellIndex++;
                }
            }

            _xCellSize = maxX - minX;
            _yCellSize = maxY - minY;
        }

        public bool CanGo(Vector2Int cellPos)
        {
            if (cellPos.X < _minX || cellPos.X >= _maxX)
            {
                return false;
            }

            if (cellPos.Y < _minY || cellPos.Y >= _maxY)
            {
                return false;
            }
            return true;
        }

        public void AddPlayerInCell(Vector2Int cellPos, Player player)
        {
            var index = GetCellIndex(cellPos);
            player.CellPos = cellPos;
            _cellPlayerDatas[index].Add(player);
        }
        public void RemovePlayerInCell(Player player)
        {
            var index = GetCellIndex(player.CellPos);
            _cellPlayerDatas[index].Remove(player);
        }
        public List<int> GetAroundCellByIndex(int index)
        {
            var list = new List<int>();
            
            var xIndex = index % _xCellSize;

            if (xIndex > 0)
            {
                list.Add(index - 1);
            }

            list.Add(index);

            if (xIndex < _xCellSize - 1)
            {
                list.Add(index + 1);
            }
            var listX = new List<int>(list);
            for (int i = 0; i < listX.Count; ++i)
            {
                var yIndex = listX[i] / _yCellSize;
                if (yIndex > 0)
                {
                    list.Add(listX[i] - _yCellSize);
                }

                if (yIndex < _yCellSize - 1)
                {
                    list.Add(listX[i] + _yCellSize);
                }
            }
            return list;
        }
        public List<Player> GetAllPlayersInCell(int index)
        {
            var list = new List<Player>();

            foreach (var item in _cellPlayerDatas[index])
            {
                list.Add(item);
            }
            return list;
        }
        public int GetCellIndex(Vector2Int vector2Int)
        {
            var xIndex = vector2Int.X - _minX;
            var yIndex = vector2Int.Y - _minY;
            return yIndex * _xCellSize + xIndex;
        }
    }
}
