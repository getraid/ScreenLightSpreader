using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using ScreenLightSpreader.ViewModel.Data;

namespace ScreenLightSpreader.Model
{
    public class DisplayToPixelManager
    {
        public float OldColorHue { get; set; }
        //Oh and yes I indeed feel bad for copying all this stackoverflow code, but I know what it does and it was exactly what i needed, so... sorry :P
        //Maybe I will change this later on to an implementation in c++ for better performance
        //https://codereview.stackexchange.com/questions/157667/getting-the-dominant-rgb-color-of-a-bitmap#comment298530_157667
        public RgbData GetAvgData(float sat)
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

            int avgR = totals[0] / (width * height);
            int avgG = totals[1] / (width * height);
            int avgB = totals[2] / (width * height);

            bmp.UnlockBits(srcData);
            GC.Collect();
            GC.WaitForPendingFinalizers();


            return ColorPush(avgR, avgG, avgB,sat);

        }

        public struct HSV { public float h; public float s; public float v; }

        //https://stackoverflow.com/questions/28759764/c-sharp-sethue-or-alternatively-convert-hsl-to-rgb-and-set-rgb
        public RgbData ColorPush(int r, int g, int b, float sat)
        {
          
            Color c = Color.FromArgb(255, r, g, b);
            var temp = new HSV();

            float tempSat = c.GetSaturation() * sat;
            if (tempSat >= 1f)
            {
                tempSat = 1f;
            }
            else if (tempSat < 0)
            {
                tempSat = 0;
            }

            temp.h = c.GetHue();
            temp.s = tempSat;
            temp.v = getBrightness(c);

            c = ColorFromHSL(temp);

            return new RgbData(Convert.ToInt32(c.R), Convert.ToInt32(c.G), Convert.ToInt32(c.B));
        }

        public static Color ColorFromHSL(HSV hsl)
        {
            if (hsl.s == 0)
            { int L = (int)hsl.v; return Color.FromArgb(255, L, L, L); }

            double min, max, h;
            h = hsl.h / 360d;

            max = hsl.v < 0.5d ? hsl.v * (1 + hsl.s) : (hsl.v + hsl.s) - (hsl.v * hsl.s);
            min = (hsl.v * 2d) - max;

            Color c = Color.FromArgb(255, (int)(255 * RGBChannelFromHue(min, max, h + 1 / 3d)),
                (int)(255 * RGBChannelFromHue(min, max, h)),
                (int)(255 * RGBChannelFromHue(min, max, h - 1 / 3d)));
            return c;
        }

        static double RGBChannelFromHue(double m1, double m2, double h)
        {
            h = (h + 1d) % 1d;
            if (h < 0) h += 1;
            if (h * 6 < 1) return m1 + (m2 - m1) * 6 * h;
            else if (h * 2 < 1) return m2;
            else if (h * 3 < 2) return m1 + (m2 - m1) * 6 * (2d / 3d - h);
            else return m1;

        }
        private float getBrightness(Color c)
        { return (c.R * 0.299f + c.G * 0.587f + c.B * 0.114f) / 256f; }

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