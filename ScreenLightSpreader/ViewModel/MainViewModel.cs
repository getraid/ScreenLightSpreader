using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using ScreenLightSpreader.Annotations;
using ScreenLightSpreader.Command;
using ScreenLightSpreader.Model;
using WebSocketSharp;

namespace ScreenLightSpreader.ViewModel
{
    /// <summary>
    ///     MainViewModel ties all properties together.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _btnStartText;
        private string _bufferTime;
        private int _bufferTimeInt;
        private Visibility _connectedVisibility;
        private string _ipAdress;
        private bool _isAutostarting;
        private string _portNumber;
        private bool _running;

        public MainViewModel()
        {
            InitCommand();
            InitModel();
            LoadSavedValues();
            InitViewModelProps();
            InitAutostart();
        }

        public int BufferTimeInt
        {
            get => _bufferTimeInt;
            set
            {
                if (value >= 0)
                    _bufferTimeInt = value;
            }
        }

        public Thread MainWorkThread { get; set; }
        public SaveSettingsCommand SaveSettingsCommand { get; set; }
        public TempOpenLEDControllerCommand TempOpenLedControllerCommand { get; set; }
        public StartCommand StartCommand { get; set; }
        public WebSocketConnector WebSocketConnector { get; set; }
        public WebSocket ws { get; set; }

        public RgbManager RgbManager { get; set; }

        public string BtnStartText
        {
            get => _btnStartText;
            set
            {
                if (value == _btnStartText) return;
                _btnStartText = value;
                OnPropertyChanged();
            }
        }

        public string BufferTime
        {
            get => _bufferTime;
            set
            {
                if (value == _bufferTime) return;
                _bufferTime = value;
                OnPropertyChanged();
            }
        }

        public Visibility ConnectedVisibility
        {
            get => _connectedVisibility;
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

        public bool Running
        {
            get => _running;
            set
            {
                if (value == _running) return;
                _running = value;
                OnPropertyChanged();
                BtnStartText = !Running ? "Start" : "Stop";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void InitAutostart()
        {
            if (IsAutostarting)
                if (StartCommand.CanExecute(null))
                    StartCommand.Execute(null);
        }

        private void InitViewModelProps()
        {
            RgbManager = new RgbManager();
            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.Closing += MainWindow_Closing;
            BtnStartText = "Start";
            ConnectedVisibility = Visibility.Hidden;

        }

        public void CreateThread()
        {
            MainWorkThread = new Thread(() =>
                RgbManager.DoWork(ws, BufferTimeInt));
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
            BufferTime = SaveManager.LoadBufftime();
        }


        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            ws?.Close();
            SaveSettingsCommand.Execute(null);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}