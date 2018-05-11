
using System.Threading;
using ScreenLightSpreader.ViewModel;
using ScreenLightSpreader.ViewModel.Data;
using WebSocketSharp;

namespace ScreenLightSpreader.Model
{
    public class RgbManager
    {
        public RgbManager()
        {

        }

        public void DoWork(WebSocket ws, int time, DisplayToPixelManager dpm, float sat)
        {
            while (ScreenVM.ThreadEnabled || WebSocketConnection.ConnectionEst)
            {
                GAvg(ws, time, dpm,sat);
            }
            Thread.CurrentThread.Interrupt();
    
            //  Thread.CurrentThread.Join();




        }

        private static void GAvg(WebSocket ws, int time, DisplayToPixelManager dpm, float sat)
        {
            dpm.GetAvgData(sat).SendValues(ws);
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