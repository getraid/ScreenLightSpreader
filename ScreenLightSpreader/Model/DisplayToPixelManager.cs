using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using ScreenLightSpreader.ViewModel;
using PixelFormat = System.Windows.Media.PixelFormat;
using Rectangle = System.Windows.Shapes.Rectangle;


namespace ScreenLightSpreader.Model
{
    public class DisplayToPixelManager
    {
        //https://codereview.stackexchange.com/questions/157667/getting-the-dominant-rgb-color-of-a-bitmap#comment298530_157667
        public RgbData GetAvgData()
        {
            Bitmap bmp = GetScreenBmp();
            BitmapData srcData = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            int stride = srcData.Stride;

            IntPtr Scan0 = srcData.Scan0;

            int[] totals = new int[] { 0, 0, 0 };

            int width = bmp.Width;
            int height = bmp.Height;

            unsafe
            {
                uint* p = (uint*)(void*)Scan0;

                uint pixelCount = (uint)(width * height);
                uint idx = 0;
                while (idx < (pixelCount & ~0xff))
                {
                    uint sumRR00BB = 0;
                    uint sum00GG00 = 0;
                    for (int j = 0; j < 0x100; j++)
                    {
                        sumRR00BB += p[idx] & 0xff00ff;
                        sum00GG00 += p[idx] & 0x00ff00;
                        idx++;
                    }

                    totals[0] += Convert.ToInt32(sumRR00BB >> 16);
                    totals[1] += Convert.ToInt32(sum00GG00 >> 8);
                    totals[2] += Convert.ToInt32(sumRR00BB & 0xffff);
                }

                // And the final partial block of fewer than 0x100 pixels.
                {
                    uint sumRR00BB = 0;
                    uint sum00GG00 = 0;
                    while (idx < pixelCount)
                    {
                        sumRR00BB += p[idx] & 0xff00ff;
                        sum00GG00 += p[idx] & 0x00ff00;
                        idx++;
                    }

                    totals[0] += (Convert.ToInt32(sumRR00BB >> 16));
                    totals[1] += (Convert.ToInt32(sum00GG00 >> 8));
                    totals[2] += Convert.ToInt32(sumRR00BB & 0xffff);
                }
            }

            int avgB = totals[0] / (width * height);
            int avgG = totals[1] / (width * height);
            int avgR = totals[2] / (width * height);

            bmp.UnlockBits(srcData);

            RgbData f = new RgbData(avgR, avgG, avgB);
            return f;
        }

        [Obsolete]
        public RgbData GetAvgDataBad()
        {
            Bitmap bmp = new Bitmap(1, 1);
            Bitmap orig = GetScreenBmp();
            using (Graphics gr = Graphics.FromImage(bmp))
            {
                // updated: the Interpolation mode needs to be set to 
                // HighQualityBilinear or HighQualityBicubic or this method
                // doesn't work at all.  With either setting, the results are
                // slightly different from the averaging method.
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.DrawImage(orig, new System.Drawing.Rectangle(0, 0, 1, 1));
            }
            Color pixel = bmp.GetPixel(0, 0);
            // pixel will contain average values for entire orig Bitmap

            int r = Convert.ToInt32(pixel.B);
            int g = Convert.ToInt32(pixel.G);
            int b = Convert.ToInt32(pixel.R);

            RgbData f = new RgbData(r, g, b);
            return f;
        }

        //https://stackoverflow.com/questions/362986/capture-the-screen-into-a-bitmap
        private Bitmap GetScreenBmp()
        {
            //Create a new bitmap.
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                Screen.PrimaryScreen.Bounds.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            // Create a graphics object from the bitmap.
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                Screen.PrimaryScreen.Bounds.Y,
                0,
                0,
                Screen.PrimaryScreen.Bounds.Size,
                CopyPixelOperation.SourceCopy);

            return bmpScreenshot;
        }
    }
}