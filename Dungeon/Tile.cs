namespace Dungeon
{
    public struct Tile
    {
        public TileType Type { get; private set; }
        public Tile(TileType tileType)
        {
            Type = tileType;
        }
        public override string ToString() => "Type: " + Type;
    }
}
