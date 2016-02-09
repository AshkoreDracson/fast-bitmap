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
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        // OPERATORS

        public static FastColor operator +(FastColor a, FastColor b)
        {
            return new FastColor((byte)(a.R + b.R), (byte)(a.G + b.G), (byte)(a.B + b.B));
        }
        public static FastColor operator -(FastColor a, FastColor b)
        {
            return new FastColor((byte)(a.R - b.R), (byte)(a.G - b.G), (byte)(a.B - b.B));
        }

        public static FastColor operator +(FastColor a, byte b)
        {
            return new FastColor((byte)(a.R + b), (byte)(a.G + b), (byte)(a.B + b));
        }
        public static FastColor operator -(FastColor a, byte b)
        {
            return new FastColor((byte)(a.R - b), (byte)(a.G - b), (byte)(a.B - b));
        }

        public static bool operator ==(FastColor a, FastColor b)
        {
            return (a.R == b.R && a.G == b.G && a.B == b.B);
        }
        public static bool operator !=(FastColor a, FastColor b)
        {
            return (a.R != b.R && a.G != b.G && a.B != b.B);
        }

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