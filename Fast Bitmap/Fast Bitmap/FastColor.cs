using System.Drawing;
namespace FastBitmapLib
{
    public struct FastColor
    {
        // STATIC PROPERTIES

        public static FastColor black
        {
            get
            {
                return new FastColor(0, 0, 0);
            }
        }
        public static FastColor grey
        {
            get
            {
                return new FastColor(127, 127, 127);
            }
        }
        public static FastColor gray
        {
            get
            {
                return new FastColor(127, 127, 127);
            }
        }
        public static FastColor white
        {
            get
            {
                return new FastColor(255, 255, 255);
            }
        }
        public static FastColor red
        {
            get
            {
                return new FastColor(255, 0, 0);
            }
        }
        public static FastColor green
        {
            get
            {
                return new FastColor(0, 255, 0);
            }
        }
        public static FastColor blue
        {
            get
            {
                return new FastColor(0, 0, 255);
            }
        }
        public static FastColor yellow
        {
            get
            {
                return new FastColor(255, 255, 0);
            }
        }
        public static FastColor cyan
        {
            get
            {
                return new FastColor(0, 255, 255);
            }
        }
        public static FastColor purple
        {
            get
            {
                return new FastColor(255, 0, 255);
            }
        }


        // VARIABLES & PROPERTIES

        /// <summary>
        /// The red channel of the color
        /// </summary>
        public byte R { get; set; }
        /// <summary>
        /// The green channel of the color
        /// </summary>
        public byte G { get; set; }
        /// <summary>
        /// The blue channel of the color
        /// </summary>
        public byte B { get; set; }

        /// <summary>
        /// Initializes a new instance of the FastColor struct
        /// </summary>
        /// <param name="r">The red channel amount (0-255)</param>
        /// <param name="g">The red channel amount (0-255)</param>
        /// <param name="b">The red channel amount (0-255)</param>
        public FastColor(byte r, byte g, byte b)
        {
            this.R = r;
            this.G = g;
            this.B = b;
        }

        // OVERRIDES

        /// <summary>
        /// Converts this FastColor to a string.
        /// </summary>
        /// <returns>A formatted string of this FastColor</returns>
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