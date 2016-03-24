using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Linq;

namespace FastBitmapLib
{
    public class FastBitmap : IDisposable
    {
        // VARIABLES & PROPERTIES

        /// <summary>
        /// The width of this FastBitmap
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// The height of this FastBitmap
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Access various image effects.
        /// </summary>
        public ImageEffects Effects { get; private set; }
        /// <summary>
        /// Determines if this FastBitmap is disposed.
        /// </summary>
        public bool Disposed { get; private set; }

        private FastColor[,] data;

        /// <summary>
        /// Initializes a new instance of FastBitmap.
        /// </summary>
        /// <param name="width">The width in pixels of the FastBitmap</param>
        /// <param name="height">The height in pixels of the FastBitmap</param>
        public FastBitmap(int width, int height)
        {
            Width = width;
            Height = height;
            data = new FastColor[width, height];
            Effects = new ImageEffects(this);
        }
        /// <summary>
        /// Initializes a new instance of FastBitmap with a specified clear color.
        /// </summary>
        /// <param name="width">The width in pixels of the FastBitmap</param>
        /// <param name="height">The height in pixels of the FastBitmap</param>
        /// <param name="color">The clear color</param>
        public FastBitmap(int width, int height, FastColor color)
        {
            Width = width;
            Height = height;
            data = new FastColor[width, height];
            Clear(color);
            Effects = new ImageEffects(this);
        }

        // METHODS & FUNCTIONS

        /// <summary>
        /// Clears the image with a specified color.
        /// </summary>
        /// <param name="color">The color</param>
        public void Clear(FastColor color)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    data[x, y] = color;
                }
            }
        }
        /// <summary>
        /// Gets the pixel color at the specified coordinates.
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        /// <returns>The pixel color</returns>
        public FastColor GetPixel(int x, int y)
        {
            x = Mathf.Clamp(x, 0, Width - 1);
            y = Mathf.Clamp(y, 0, Height - 1);

            return data[x, y];
        }
        /// <summary>
        /// Sets the pixel color at the specified coordinates.
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        /// <param name="c">The pixel color</param>
        public void SetPixel(int x, int y, FastColor c)
        {
            x = Mathf.Clamp(x, 0, Width - 1);
            y = Mathf.Clamp(y, 0, Width - 1);

            data[x, y] = c;
        }

        /// <summary>
        /// Saves this FastBitmap as an image.
        /// </summary>
        /// <param name="path">The filepath to save this FastBitmap at</param>
        public void Save(string path)
        {
            ((Bitmap)this).Save(path);
        }

        /// <summary>
        /// Disposes the instance of this FastBitmap.
        /// </summary>
        public void Dispose()
        {
            if (Disposed) return;
            Disposed = true;
        }

        // STATIC METHODS & FUNCTIONS

        /// <summary>
        /// Initializes a FastBitmap from an image file.
        /// </summary>
        /// <param name="path">The filepath of the image file.</param>
        /// <returns>A new instance of FastBitmap containing the loaded image data.</returns>
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

        // NESTED CLASSES

        public class ImageEffects
        {
            private FastBitmap f;

            public ImageEffects(FastBitmap parent)
            {
                this.f = parent;
            }

            /// <summary>
            /// Inverts the image.
            /// </summary>
            public void Invert()
            {
                for (int x = 0; x < f.Width; x++)
                {
                    for (int y = 0; y < f.Height; y++)
                    {
                        FastColor color = f.GetPixel(x, y);
                        FastColor invertedColor = new FastColor((byte)(255 - color.R), (byte)(255 - color.G), (byte)(255 - color.B));
                        f.SetPixel(x, y, invertedColor);
                    }
                }
            }
            /// <summary>
            /// Sets the luminosity of the image.
            /// </summary>
            /// <param name="luminosityFactor">The luminosity factor</param>
            public void Luminosity(float luminosityFactor)
            {
                for (int x = 0; x < f.Width; x++)
                {
                    for (int y = 0; y < f.Height; y++)
                    {
                        FastColor c = f.GetPixel(x, y);
                        c = new FastColor(
                            Mathf.ClampByte((byte)(c.R * luminosityFactor)), 
                            Mathf.ClampByte((byte)(c.G * luminosityFactor)), 
                            Mathf.ClampByte((byte)(c.B * luminosityFactor)));
                        f.SetPixel(x, y, c);
                    }
                }
            }
            /// <summary>
            /// Blurs the image using the Box Blur technique, currently atrociously slow.
            /// </summary>
            /// <param name="blurSize">The size of the blur, a higher size means more blurring.</param>
            public void BoxBlur(int blurSize) // todo: Make this effect sonic fast.
            {
                if (blurSize < 1) return;

                FastColor[,] finalData = new FastColor[f.Width, f.Height];

                for (int x = 0; x < f.Width; x++)
                {
                    for (int y = 0; y < f.Height; y++)
                    {
                        float r = 0, g = 0, b = 0;
                        int blurDivision = 0;
                        for (int x2 = -blurSize; x2 <= blurSize; x2++)
                        {
                            for (int y2 = -blurSize; y2 <= blurSize; y2++)
                            {
                                Color c = f.GetPixel(x + x2, y + y2);
                                r += c.R;
                                g += c.G;
                                b += c.B;
                                blurDivision++;
                            }
                        }
                        r /= blurDivision;
                        g /= blurDivision;
                        b /= blurDivision;

                        finalData[x, y] = new FastColor((byte)r, (byte)g, (byte)b);
                    }
                }

                // Final pass, feed blurred data to FastBitmap

                for (int x = 0; x < f.Width; x++)
                {
                    for (int y = 0; y < f.Height; y++)
                    {
                        f.SetPixel(x, y, finalData[x, y]);
                    }
                }
            }
            /// <summary>
            /// Detects edges in your image, uses a 3x3 kerning. (Atrociously slow too, I really need to find better ways at this)
            /// </summary>
            public void EdgeDetection()
            {
                FastColor[,] finalData = new FastColor[f.Width, f.Height];

                for (int x = 0; x < f.Width; x++)
                {
                    for (int y = 0; y < f.Height; y++)
                    {
                        FastColor[] temp = new FastColor[3 * 3];
                        int tempIndex = 0;

                        for (int x2 = -1; x2 <= 1; x2++) // 3x3 kerning
                        {
                            for (int y2 = -1; y2 <= 1; y2++)
                            {
                                temp[tempIndex++] = f.GetPixel(x + x2, y + y2);
                            }
                        }

                        byte[] rArr = temp.Select(o => o.R).ToArray();
                        byte[] gArr = temp.Select(o => o.G).ToArray();
                        byte[] bArr = temp.Select(o => o.B).ToArray();

                        byte rMin = Mathf.Min(rArr);
                        byte gMin = Mathf.Min(gArr);
                        byte bMin = Mathf.Min(bArr);

                        byte rMax = Mathf.Max(rArr);
                        byte gMax = Mathf.Max(gArr);
                        byte bMax = Mathf.Max(bArr);

                        byte final = Mathf.Max((byte)(rMax - rMin), (byte)(gMax - gMin), (byte)(bMax - bMin));

                        finalData[x, y] = new FastColor(final, final, final);
                    }
                }

                for (int x = 0; x < f.Width; x++)
                {
                    for (int y = 0; y < f.Height; y++)
                    {
                        f.SetPixel(x, y, finalData[x, y]);
                    }
                }
            }
        }
    }
}