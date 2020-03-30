using System;

namespace DungeonGeneration
{
    class Program
    {
        static void Main()
        {
            var grid = new Grid();
            grid.GenerateGrid(width: 500,
                              height: 500,
                              maxRoomTries: 5000,
                              maxRooms: 1000,
                              minRoomSizeX: 5,
                              minRoomSizeY: 5,
                              maxRoomSizeX: 90,
                              maxRoomSizeY: 50,
                              roomBorderLeniancy: 4);
            grid.PrintGrid();
        }
    }
}
