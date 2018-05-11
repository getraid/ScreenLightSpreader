using WebSocketSharp;

namespace ScreenLightSpreader.ViewModel
{
    public static class WebSocketConnection
    {
        public static WebSocket WebSocket { get; set; }
        public static bool ConnectionEst { get; set; }
    }
}