using System.Windows.Media;
using ScreenLightSpreader.Properties;
using ScreenLightSpreader.ViewModel.Data;
using ColorConverter = ScreenLightSpreader.Model.ColorConverter;

namespace ScreenLightSpreader.ViewModel
{
    public class LEDVM : BaseVM
    {
        public RgbData RgbData { get; set; }

        public Color SelectedColor
        {
            get
            {
                RgbData.r = Settings.Default.SelectedColor.R;
                RgbData.g = Settings.Default.SelectedColor.G;
                RgbData.b = Settings.Default.SelectedColor.B;
                RgbData?.SendValues(WebSocketConnection.WebSocket);
                return ColorConverter.GetSystemColor(Settings.Default.SelectedColor);
            }
            set
            {
                if (value.Equals(ColorConverter.GetSystemColor(Settings.Default.SelectedColor))) return;
                Settings.Default.SelectedColor = ColorConverter.GetDrawingColor(value);
                Settings.Default.Save();
   
                
                OnPropertyChanged();
            }
        }
        //todo favorites abspeichern lassen
        public LEDVM()
        {
            RgbData = new RgbData();
        }

    }
}