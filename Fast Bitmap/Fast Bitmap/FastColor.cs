using System.Drawing;
namespace FastBitmapLib
{
    public struct FastColor
    {
        // VARIABLES & PROPERTIES

        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public FastColor(byte r, byte g, byte b)
        {
            this.R = r;
            this.G = g;
            this.B = b;
        }

        // OVERRIDES

        public override string ToString()
        {
            return $"[R:{R}, G:{G}, B:{B}]";
        }

        // OPERATORS

        public static implicit operator FastColor(Color c)
        {
            return new FastColor(c.R, c.G, c.B);
        }
        public static implicit operator Color(FastColor c)
        {
            return Color.FromArgb(c.R, c.G, c.B);
        }
    }
}