using System.ComponentModel;
using System.Threading;
using System.Windows;
using ScreenLightSpreader.ViewModel;
using WebSocketSharp;

namespace ScreenLightSpreader.Model
{
    public class RgbManager
    {
        public RgbManager()
        {

        }

        public void DoWork(WebSocket ws, int time, DisplayToPixelManager dpm)
        {
            RgbData rgb = new RgbData(0, 0, 0);
            while (true)
            {
                GAvg(ws, time, dpm);
            }
        }

        private static void GAvg(WebSocket ws, int time, DisplayToPixelManager dpm)
        {
            dpm.GetAvgData().SendValues(ws);
            System.Threading.Thread.Sleep(time);

        }

        private static void FlashWhite(WebSocket ws, RgbData rgb, int time)
        {

            for (int i = 0; i < 255; i++)
            {
                rgb.r = i;
                rgb.g = i;
                rgb.b = i;
                rgb.SendValues(ws);
                System.Threading.Thread.Sleep(time);
                if (i == 254)
                {
                    for (int j = 255; j > 0; j--)
                    {
                        rgb.r = j;
                        rgb.g = j;
                        rgb.b = j;
                        rgb.SendValues(ws);
                        System.Threading.Thread.Sleep(time);
                    }

                }

            }

        }
    }
}