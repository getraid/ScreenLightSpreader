using System.ComponentModel;
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

        // UMSCHREIBEN -> WEG MIT BACKGROUND WORKER, NORMALER THREAD; name buffer umbennenen

        public void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e, WebSocket ws, int time)
        {
            RgbData rgb = new RgbData(0,0,0);
            while (true)
            {
                FlashWhite(ws, rgb, time);
                e.Cancel
            }
          
        }

        private static void FlashWhite(WebSocket ws, RgbData rgb,int time)
        {
          
                for (int i = 0; i < 255; i++)
                {
                    rgb.r = i;
                    rgb.g = i;
                    rgb.b = i;
                    rgb.SendValues(ws);
                    System.Threading.Thread.Sleep(time);
                }
            
        }
    }
}