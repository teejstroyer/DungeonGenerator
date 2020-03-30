namespace Dungeon
{
    public class Rect
    {
        public Rect(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public Rect(int width, int height, Point topLeftPoint)
        {
            Width = width;
            Height = height;
            TopLeftPoint = topLeftPoint;
        }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Area { get => Width * Height; }
        public Point TopLeftPoint { get; set; }
        public Point BottomRightPoint
        {
            get => new Point
            {
                X = TopLeftPoint.X + Width,
                Y = TopLeftPoint.Y + Height
            };
        }

        public Point Center
        {
            get => new Point
            (
              TopLeftPoint.X + (int)(.5f * Width),
              TopLeftPoint.Y + (int)(.5f * Height)
            );
        }

        public override string ToString()
        {
            return $"Top Left Point: {TopLeftPoint}, Width: {Width}, Heigh: {Height}";
        }
    }

}


