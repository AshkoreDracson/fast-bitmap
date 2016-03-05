using FastBitmapLib;
using System;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;
using DialogResult = System.Windows.Forms.DialogResult;
class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        FastColor c = new FastColor(127, 192, 255);

        Console.WriteLine("Please select an image.");
        OpenFileDialog ofd = new OpenFileDialog() { Filter = "BMP Files|*.bmp|JPG Files|*.jpg|PNG Files|*.png", CheckFileExists = true, CheckPathExists = true };
        SaveFileDialog sfd = new SaveFileDialog() { Filter = "BMP Files|*.bmp", CheckPathExists = true };

        if (ofd.ShowDialog() == DialogResult.OK)
        {
            FastBitmap f = FastBitmap.FromFile(ofd.FileName);

            // Darken it
            f.Effects.Luminosity(0.5f);
            

            Console.WriteLine("Please select the save path");

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                f.Save(sfd.FileName);
            }
        }
    }
}