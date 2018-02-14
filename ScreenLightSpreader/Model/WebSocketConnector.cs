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
        public void InitEventHandlers(MainViewModel mv)
        {
            mv.ws.OnOpen += delegate (object sender, EventArgs e) { Ws_OnOpen(sender, e, mv); };
            mv.ws.OnClose += delegate (object sender, CloseEventArgs e) { Ws_OnClose(sender, e, mv); };
            mv.ws.OnError += Ws_OnError;
        }

        public bool OpenConnection(WebSocketSharp.WebSocket ws)
        {
            ws.Connect();
            return ws.IsAlive;
        }

        private void Ws_OnError(object sender, ErrorEventArgs e)
        {
            MessageBox.Show("Error occured: " + e.Message);
        }

        private void Ws_OnClose(object sender, CloseEventArgs e, MainViewModel mv)
        {
            mv.ConnectedVisibility = Visibility.Hidden;
            mv.Running = false;
            if (!e.WasClean)
            {
                MessageBox.Show(e.Reason);
            }

        }

        private void Ws_OnOpen(object sender, EventArgs e, MainViewModel mv)
        {
            mv.ConnectedVisibility = Visibility.Visible;
            mv.Running = true;
        }
    }
}