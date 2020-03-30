using System;

namespace Dungeon
{
    public class Ellipse
    {
        public Ellipse(int radius) { Radius = radius; }
        public Ellipse(int radius, Point center)
        {
            Radius = radius;
            Center = center;
        }
        public int Radius { get; set; }
        public int Area { get => (int)(Math.PI * Radius * Radius); }
        public Point Center { get; set; }
        public override string ToString() => $"Center: {Center}, Radius: {Radius}";
    }
}