using System;
using Dungeon;

namespace Dungeon.Console
{
    class Program
    {
        static void Main()
        {
            var grid = new Grid();
            var Dungeon = grid.GenerateGrid(width: 100,
                                            height: 100,
                                            maxRooms: 15,
                                            maxRoomTries: 500,
                                            minRoomSizeX: 3,
                                            minRoomSizeY: 3,
                                            maxRoomSizeX: 15,
                                            maxRoomSizeY: 15,
                                            roomBorderLeniency: 3,
                                            randomWalkTunnels: false,
                                            triangulatedTunnels: true);
            grid.PrintGrid();
        }
    }
}
