using System;
using System.Linq;
using System.Collections.Generic;

namespace Dungeon
{
    public struct Grid
    {
        private List<Point> RoomCenters;
        private char[,] _grid;
        private int MaxRoomSizeX;
        private int MaxRoomSizeY;
        private int MaxRoomTries;
        private int MaxRooms;
        private int MinRoomSizeX;
        private int MinRoomSizeY;
        private int RoomBorderLeniency;
        public int Height { get => _grid.GetLength(1); }
        public int Width { get => _grid.GetLength(0); }

        public char[,] GenerateGrid(
            int width,
            int height,
            int maxRooms = 3,
            int maxRoomTries = 300,
            int minRoomSizeX = 3,
            int minRoomSizeY = 3,
            int maxRoomSizeX = 15,
            int maxRoomSizeY = 15,
            int roomBorderLeniency = 1,
            bool randomWalkTunnels = false,
            bool triangulatedTunnels = true
            )
        {
            MaxRooms = maxRooms;
            MaxRoomTries = maxRoomTries;
            MinRoomSizeX = minRoomSizeX;
            MinRoomSizeY = minRoomSizeY;
            MaxRoomSizeX = maxRoomSizeX;
            MaxRoomSizeY = maxRoomSizeY;
            RoomBorderLeniency = roomBorderLeniency;
            CreateGrid(width, height, ' ');
            CreateRooms();
            CreateTunnels(randomWalkTunnels: randomWalkTunnels, triangulatedTunnels: triangulatedTunnels);
            AddWalls();
            //DecorateRooms();
            return _grid;
        }
        public void PrintGrid()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Console.Write(_grid[x, y]);
                }
                Console.Write("\r\n");
            }
        }
        private void CreateGrid(int width, int height, char initVal)
        {
            _grid = new char[width, height];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    _grid[x, y] = initVal;
                }
            }
        }
        private bool IsValidPoint(int x, int y, bool includeExterior = false)
        {
            if (includeExterior) return y >= 0 && x >= 0 && y <= Height - 1 && x <= Width - 1;
            else return y > 0 && x > 0 && y < Height - 1 && x < Width - 1;
        }
        private bool IsValidRect(Rect rect, char val)
        {
            if (!IsValidPoint(rect.TopLeftPoint.X - RoomBorderLeniency, rect.TopLeftPoint.Y - RoomBorderLeniency)
                || !IsValidPoint(rect.TopLeftPoint.X + rect.Width + RoomBorderLeniency, rect.TopLeftPoint.Y + rect.Height + RoomBorderLeniency))
            {
                return false;
            }
            for (int y = rect.TopLeftPoint.Y - RoomBorderLeniency; y < rect.TopLeftPoint.Y + rect.Height + RoomBorderLeniency; y++)
            {
                for (int x = rect.TopLeftPoint.X - RoomBorderLeniency; x < rect.TopLeftPoint.X + rect.Width + RoomBorderLeniency; x++)
                {
                    var suspect = GetValue(x, y);
                    if (suspect == val || suspect == '!') return false;
                }
            }
            return true;
        }
        private bool IsValidEllipse(Ellipse ellipse, char val)
        {
            ellipse.Radius += RoomBorderLeniency;
            if (
                ellipse.Center.X - ellipse.Radius <= 0 ||
                ellipse.Center.Y - ellipse.Radius <= 0 ||
                ellipse.Center.X + ellipse.Radius >= Width - 1 ||
                ellipse.Center.Y + ellipse.Radius >= Height - 1
            ) return false;

            for (int y = ellipse.Center.Y - ellipse.Radius; y < ellipse.Center.Y + ellipse.Radius; y++)
            {
                for (int x = ellipse.Center.X - ellipse.Radius; x < ellipse.Center.X + ellipse.Radius; x++)
                {
                    if (GetValue(x, y) == val) return false;
                }
            }
            return true;
        }
        private void Draw(Rect rect, char val)
        {
            for (int y = rect.TopLeftPoint.Y; y < rect.TopLeftPoint.Y + rect.Height; y++)
            {
                for (int x = rect.TopLeftPoint.X; x < rect.TopLeftPoint.X + rect.Width; x++)
                {
                    SetValue(x, y, val);
                }
            }
        }
        private void Draw(Ellipse ellipse, char val)
        {
            for (int y = ellipse.Center.Y - ellipse.Radius; y < ellipse.Center.Y + ellipse.Radius; y++)
            {
                for (int x = ellipse.Center.X - ellipse.Radius; x < ellipse.Center.X + ellipse.Radius; x++)
                {
                    var dX = x - ellipse.Center.X;
                    var dY = y - ellipse.Center.Y;
                    if (dX * dX + dY * dY < (ellipse.Radius * ellipse.Radius))
                    {
                        SetValue(x, y, val);
                    }
                }
            }
        }
        private void DrawLine(Point p1, Point p2)
        {
            if (p1.X == p2.X)
            {
                int start = Math.Min(p1.Y, p2.Y);
                int finish = Math.Max(p1.Y, p2.Y);
                for (int y = start; y <= finish; y++)
                {
                    SetValue(p1.X, y, '.');
                }
            }
            else if (p1.Y == p2.Y)
            {
                int start = Math.Min(p1.X, p2.X);
                int finish = Math.Max(p1.X, p2.X);
                for (int x = start; x <= finish; x++)
                {
                    SetValue(x, p1.Y, '.');
                }
            }
        }
        private void SetValue(int x, int y, char val)
        {
            if (!IsValidPoint(x, y)) return;
            _grid[x, y] = val;
        }
        private char GetValue(int x, int y)
        {
            if (!IsValidPoint(x, y)) return ' ';
            return _grid[x, y];
        }
        private Point RandomPoint()
        {
            Random rng = new Random();
            return new Point(
                rng.Next(2, Width - 2),
                rng.Next(2, Height - 2)
                );
        }
        private float GetDistance(Point p1, Point p2) => (float)Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        private Rect RandomRect()
        {
            Random rng = new Random();
            return new Rect(
                rng.Next(MinRoomSizeX, MaxRoomSizeX),
                rng.Next(MinRoomSizeY, MaxRoomSizeY),
                RandomPoint()
                );
        }
        private Ellipse RandomEllipse()
        {
            Random rng = new Random();
            return new Ellipse(
                rng.Next(Math.Min(MinRoomSizeX, MinRoomSizeY), Math.Min(MaxRoomSizeX, MaxRoomSizeY)),
                RandomPoint()
                );
        }
        private void CreateRooms()
        {
            RoomCenters = new List<Point>();
            Random rng = new Random();
            for (int roomCount = 0, tries = 0; tries < MaxRoomTries && roomCount < MaxRooms; tries++)
            {
                if (rng.Next(0, 5) > 0)
                {
                    var r = RandomRect();
                    if (IsValidRect(r, '.'))
                    {
                        Draw(r, '.');
                        roomCount++;
                        RoomCenters.Add(r.Center);
                    }
                }
                else
                {
                    var e = RandomEllipse();
                    if (IsValidEllipse(e, '.'))
                    {
                        Draw(e, '.');
                        RoomCenters.Add(e.Center);
                        roomCount++;
                    }
                }
            }
        }
        private void CreateTunnels(bool randomWalkTunnels, bool triangulatedTunnels)
        {
            var pairs = new List<(Point, Point, float)>();
            for (int p1 = 0; p1 < RoomCenters.Count; p1++)
            {
                for (int p2 = p1 + 1; p2 < RoomCenters.Count; p2++)
                {
                    pairs.Add((RoomCenters[p1], RoomCenters[p2], GetDistance(RoomCenters[p1], RoomCenters[p2])));
                }
            }

            pairs = pairs.OrderByDescending(i => i.Item3).ToList();
            var maxPair = pairs.FirstOrDefault();

            if (randomWalkTunnels) CreateRandomWalkTunnels(pairs);
            if (triangulatedTunnels) CreateTriangulatedTunnels(pairs);

            SetValue(maxPair.Item1.X, maxPair.Item1.Y, 'S');
            SetValue(maxPair.Item2.X, maxPair.Item2.Y, 'E');
        }
        private void CreateRandomWalkTunnels(List<(Point, Point, float)> pairs)
        {
            foreach (var pair in pairs)
            {
                var direction = Direction.DOWN;
                for (int x = pair.Item1.X, y = pair.Item1.Y; x != pair.Item2.X && y != pair.Item2.Y;)
                {
                    var options = new List<(Direction, float)>() {
                        (Direction.UP , GetDistance(new Point(x, y - 1), pair.Item2)),
                       (Direction.DOWN , GetDistance(new Point(x, y + 1), pair.Item2)),
                       (Direction.LEFT , GetDistance(new Point(x - 1, y), pair.Item2)),
                       (Direction.RIGHT , GetDistance(new Point(x + 1, y), pair.Item2))
                    };
                    var minDist = options.Min(i => i.Item2);
                    if (!options.Any(i => i.Item1 == direction && i.Item2 == minDist))
                    {
                        direction = options.First(i => i.Item2 == minDist).Item1;
                    }

                    switch (direction)
                    {
                        case Direction.UP:
                            y--;
                            break;
                        case Direction.DOWN:
                            y++;
                            break;
                        case Direction.LEFT:
                            x--;
                            break;
                        case Direction.RIGHT:
                            x++;
                            break;
                    }
                    if (GetValue(x, y) != '.') SetValue(x, y, '.');
                }
            }
        }
        private void CreateTriangulatedTunnels(List<(Point, Point, float)> pairs)
        {
            foreach (var pair in pairs)
            {
                DrawLine(pair.Item1, new Point(pair.Item2.X, pair.Item1.Y));
                DrawLine(pair.Item2, new Point(pair.Item2.X, pair.Item1.Y));
            }
        }
        private void AddWalls()
        {
            for (int y = 1; y < Height - 1; y++)
            {
                for (int x = 1; x < Width - 1; x++)
                {
                    if (GetValue(x, y) == ' ')
                    {
                        if (
                            '.' == GetValue(x, y - 1) ||
                            '.' == GetValue(x, y + 1) ||
                            '.' == GetValue(x - 1, y) ||
                            '.' == GetValue(x + 1, y) ||
                            '.' == GetValue(x + 1, y + 1) ||
                            '.' == GetValue(x + 1, y - 1) ||
                            '.' == GetValue(x - 1, y + 1) ||
                            '.' == GetValue(x - 1, y - 1)
                        )
                        {
                            SetValue(x, y, '#');
                        }
                    }
                }
            }
        }

    }
}
