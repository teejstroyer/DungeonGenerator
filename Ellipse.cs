using System;

namespace DungeonGeneration
{
    public class Ellipse
    {
        public Ellipse(int radius)
        {
            Radius = radius;
        }

        public Ellipse(int radius, Point center)
        {
            Radius = radius;
            Center = center;
        }
        public int Radius { get; set; }
        public int Area { get => (int)(MathF.PI * Radius * Radius); }
        public Point Center { get; set; }

        public override string ToString()
        {
            return $"Center: {Center}, Radius: {Radius}";
        }
    }
}
