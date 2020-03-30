namespace DungeonGeneration
{
  public struct Tile
  {
    public TileType Type { get; private set; }
    public Tile(TileType tileType)
    {
      Type = tileType;
    }
    public override string ToString()
    {
      return "Type: " + Type;
    }
  }
}
