using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Windows;
using WebSocketSharp;

namespace ScreenLightSpreader.ViewModel.Data
{
    /// <summary>
    /// This class is used to define RGB values and send them directly via a WebSocketSharp-connection (via serialized JSON)
    /// </summary>
    public class RgbData
    {
        private int _b;
        private int _g;
        private int _r;

        public RgbData()
        {
            r = 0;
            g = 0;
            b = 0;
        }

        public RgbData(bool white)
        {
            if (white)
            {
                r = 255;
                g = 255;
                b = 255;
            }
            else
            {
                r = 0;
                g = 0;
                b = 0;
            }
        }

        public RgbData(int r, int g, int b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public int r
        {
            get => _r;
            set
            {
                if (value <= 255 && value >= 0)
                    _r = value;
            }
        }

        public int g
        {
            get => _g;
            set
            {
                if (value <= 255 && value >= 0)
                    _g = value;
            }
        }

        public int b
        {
            get => _b;
            set
            {
                if (value <= 255 && value >= 0)
                    _b = value;
            }
        }

        /// <summary>
        /// Serialize this object into a string
        /// </summary>
        /// <returns></returns>
        private string GetJsonRgbObj()
        {
            var js = new DataContractJsonSerializer(typeof(RgbData));
            var ms = new MemoryStream();
            js.WriteObject(ms, this);
            ms.Position = 0;
            var sr = new StreamReader(ms);
            return sr.ReadToEnd();
        }

        /// <summary>
        /// Send this rgb-object to the websocket server
        /// </summary>
        /// <param name="ws">WebSocketSharp Websocket-object</param>
        public void SendValues(WebSocket ws)
        {
            ws.Send(GetJsonRgbObj());

        }
    }
}