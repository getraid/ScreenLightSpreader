using System.IO;
using System.Reflection.Emit;
using System.Runtime.Serialization.Json;

namespace ScreenLightSpreader.ViewModel
{
    public class RgbData
    {
        private int _r;
        private int _g;
        private int _b;

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
                    _b = value;
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
                    _r = value;
            }
        }


        public string GetJsonRgbObj()
        {
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(RgbData));
            MemoryStream ms = new MemoryStream();
            js.WriteObject(ms, this);
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            return sr.ReadToEnd();

        }

        public void SendValues(WebSocketSharp.WebSocket ws)
        {
            ws.Send(GetJsonRgbObj());
        }
    }
}