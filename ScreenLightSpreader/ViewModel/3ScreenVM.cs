using System.Threading;
using ScreenLightSpreader.Command;
using ScreenLightSpreader.Model;
using ScreenLightSpreader.Properties;

namespace ScreenLightSpreader.ViewModel
{
    public class ScreenVM : BaseVM
    {
        private int _connInfo;
        private bool _isRunning;

        public ScreenVM()
        {
          ThreadEnabled = true;

            RgbManager = new RgbManager();
            DisplayToPixelManager = new DisplayToPixelManager();
            SlsCommand = new SLSCommand(this);
        }

     public static bool ThreadEnabled { get; set; }

        public int ConnInfo
        {
            get => _connInfo;
            set
            {
                if (value == _connInfo) return;
                _connInfo = value;
                OnPropertyChanged();
            }
        }

        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                if (value == _isRunning) return;
                _isRunning = value;
                OnPropertyChanged();
            }
        }

        public string SaturationMultiplier
        {
            get => Settings.Default.SaturationMultiplier;
            set
            {
                if (value == Settings.Default.SaturationMultiplier) return;
                Settings.Default.SaturationMultiplier = value;
                Settings.Default.Save();
                OnPropertyChanged();
            }
        }

        public string Buffer
        {
            get => Settings.Default.Buffertime;
            set
            {
                if (value == Settings.Default.Buffertime) return;
                Settings.Default.Buffertime = value;
                Settings.Default.Save();
                OnPropertyChanged();
            }
        }

        public SLSCommand SlsCommand { get; set; }
        public Thread SlsWorkThread { get; set; }
        public RgbManager RgbManager { get; set; }
        public DisplayToPixelManager DisplayToPixelManager { get; set; }
    }
}