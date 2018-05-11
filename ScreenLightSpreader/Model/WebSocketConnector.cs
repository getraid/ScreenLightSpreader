using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Effects;
using ScreenLightSpreader.ViewModel;
using WebSocketSharp;

namespace ScreenLightSpreader.Model
{
    public class WebSocketConnector
    {
        public void InitEventHandlers(WebSocketSharp.WebSocket ws)
        {
            ws.OnOpen += delegate (object sender, EventArgs e) { Ws_OnOpen(sender, e); };
            ws.OnClose += delegate (object sender, CloseEventArgs e) { Ws_OnClose(sender, e); };
            ws.OnError += Ws_OnError;
        }

        public bool OpenConnection(WebSocketSharp.WebSocket ws)
        {
            WebSocketConnection.ConnectionEst = true;
            ws.Connect();
            return ws.IsAlive;
        }

        private void Ws_OnError(object sender, ErrorEventArgs e)
        {
            WebSocketConnection.ConnectionEst = false;
            MessageBox.Show("Error occured: " + e.Message);
        }

        private void Ws_OnClose(object sender, CloseEventArgs e)
        {
            WebSocketConnection.ConnectionEst = false;
            if (!e.WasClean)
            {
                MessageBox.Show(e.Reason);
            }

        }

        private void Ws_OnOpen(object sender, EventArgs e)
        {

        }
    }
}