using System;
using Dungeon;

namespace Dungeon.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            var grid = new Grid();
            var Dungeon = grid.GenerateGrid(width: 50,
                                            height: 50,
                                            maxRooms: 3,
                                            maxRoomTries: 300,
                                            minRoomSizeX: 3,
                                            minRoomSizeY: 3,
                                            maxRoomSizeX: 15,
                                            maxRoomSizeY: 15,
                                            roomBorderLeniancy: 1);
            grid.PrintGrid();
        }
    }
}
