using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ScreenLightSpreader.Annotations;
using ScreenLightSpreader.Command;
using ScreenLightSpreader.Model;
using WebSocketSharp;
using WebSocketSharp.Net.WebSockets;

namespace ScreenLightSpreader.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private bool _automode;
        private string _ipAdress;
        private bool _isAutostarting;
        private string _portNumber;
        private Visibility _connectedVisibility;

        public MainViewModel()
        {
            InitCommand();
            InitModel();
            LoadSavedValues();
            ConnectedVisibility = Visibility.Hidden;
        }

        public SaveSettingsCommand SaveSettingsCommand { get; set; }
        public TempOpenLEDControllerCommand TempOpenLedControllerCommand { get; set; }
        public StartCommand StartCommand { get; set; }
        public WebSocketConnector WebSocketConnector { get; set; }
        public WebSocketSharp.WebSocket ws { get; set; }

        public Visibility ConnectedVisibility
        {
            get { return _connectedVisibility; }
            set
            {
                if (value == _connectedVisibility) return;
                _connectedVisibility = value;
                OnPropertyChanged();
            }
        }
        public string PortNumber
        {
            get => _portNumber;
            set
            {
                if (value == _portNumber) return;
                _portNumber = value;
                OnPropertyChanged();
            }
        }

        public bool IsAutostarting
        {
            get => _isAutostarting;
            set
            {
                if (value == _isAutostarting) return;
                _isAutostarting = value;
                OnPropertyChanged();
            }
        }

        public string IpAdress
        {
            get => _ipAdress;
            set
            {
                if (value == _ipAdress) return;
                _ipAdress = value;
                OnPropertyChanged();
            }
        }

        public bool Automode
        {
            get => _automode;
            set
            {
                if (value == _automode) return;
                _automode = value;
                OnPropertyChanged();
            }
        }

        private void InitCommand()
        {
            SaveSettingsCommand = new SaveSettingsCommand(this);
            StartCommand = new StartCommand(this);
            TempOpenLedControllerCommand = new TempOpenLEDControllerCommand(this);
        }

        private void InitModel()
        {
            WebSocketConnector = new WebSocketConnector();

        }

        private void LoadSavedValues()
        {
            IpAdress = SaveManager.LoadIp();
            PortNumber = SaveManager.LoadPort();
            IsAutostarting = SaveManager.LoadIsAutostarting();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}