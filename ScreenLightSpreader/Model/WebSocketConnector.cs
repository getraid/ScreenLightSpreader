using System.Diagnostics;

namespace ScreenLightSpreader.Model
{
    public class WebSocketConnector
    {
        public bool OpenConnection(WebSocketSharp.WebSocket ws)
        {
            ws.Connect();
            if (ws.IsAlive)
            {
                Debug.WriteLine("Connection established");
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}