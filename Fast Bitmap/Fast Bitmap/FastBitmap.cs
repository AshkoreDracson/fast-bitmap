using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace FastBitmapLib
{
    public class FastBitmap : IDisposable
    {
        // VARIABLES & PROPERTIES

        public int Width { get; set; }
        public int Height { get; set; }
        public bool Disposed { get; private set; }

        private FastColor[,] data;

        public FastBitmap(int width, int height)
        {
            this.Width = width;
            this.Height = height;

            data = new FastColor[width, height];
        }

        // METHODS & FUNCTIONS

        public FastColor GetPixel(int x, int y)
        {
            return data[x, y];
        }
        public void SetPixel(int x, int y, FastColor c)
        {
            data[x, y] = c;
        }

        public void Save(string path)
        {
            ((Bitmap)this).Save(path);
        }

        public void Dispose()
        {
            if (Disposed) return;
            Disposed = true;
        }

        // STATIC METHODS & FUNCTIONS

        public static FastBitmap FromFile(string path)
        {
            return (FastBitmap)Bitmap.FromFile(path);
        }

        // OVERRIDES

        public override string ToString()
        {
            return $"FastBitmap[Width:{Width}, Height:{Height}]";
        }

        // OPERATORS

        public static implicit operator Bitmap(FastBitmap f)
        {
            Bitmap b = new Bitmap(f.Width, f.Height, PixelFormat.Format24bppRgb);

            BitmapData bData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            IntPtr ptr = bData.Scan0;

            int bytes = Math.Abs(bData.Stride) * b.Height;
            byte[] rgbValues = new byte[bytes];

            Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int y = 0; y < f.Height; y++)
            {
                for (int x = 0; x < f.Width; x++)
                {
                    Color c = f.GetPixel(x, y);
                    int i = (y * f.Width + x) * 3;

                    rgbValues[i] = c.B;
                    rgbValues[i + 1] = c.G;
                    rgbValues[i + 2] = c.R;
                }
            }

            Marshal.Copy(rgbValues, 0, ptr, bytes);

            b.UnlockBits(bData);

            return b;
        }
        public static implicit operator FastBitmap(Bitmap b)
        {
            if (b.PixelFormat != PixelFormat.Format24bppRgb)
                throw new Exception("Invalid pixel format. Must be 24bppRGB");

            FastBitmap f = new FastBitmap(b.Width, b.Height);

            BitmapData bData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            IntPtr ptr = bData.Scan0;

            int bytes = Math.Abs(bData.Stride) * b.Height;
            byte[] rgbValues = new byte[bytes];

            Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int i = 0; i < rgbValues.Length; i += 3)
            {
                int index = i / 3;
                int x = index % f.Width;
                int y = (int)(index / f.Width);

                f.SetPixel(x, y, Color.FromArgb(rgbValues[i + 2], rgbValues[i + 1], rgbValues[i]));
            }

            return f;
        }
    }
}