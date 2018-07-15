using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using ScreenLightSpreader.Command;
using ScreenLightSpreader.Model;
using ScreenLightSpreader.Properties;
using ScreenLightSpreader.ViewModel.Data;

namespace ScreenLightSpreader.ViewModel
{
    public class GeneralVM : BaseVM
    {
        private string _connectButtonText;
        private Brush _connectedForegroundColor;
        private string _connectedLabel;
        private bool setti;
     
        public GeneralVM(ScreenVM screenVM)
        {
            ScreenVm = screenVM;
            WebSocketConnector = new WebSocketConnector();
            ConnectToServerCommand = new ConnectToServerCommand(this);

            ConnectedForegroundColor = Brushes.Black;
            SetConnectButtonAndLabel(false);

            if (AutoConnectOnOpen)
            {
                if (ConnectToServerCommand.CanExecute(null))
                {
                    ConnectToServerCommand.Execute(null);
                }
            }

            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Closing += MainWindow_Closing;
            }
        }

        public string IpAdress
        {
            get => Settings.Default.IpAdress;
            set
            {
                if (value == Settings.Default.IpAdress) return;
                Settings.Default.IpAdress = value;
                Settings.Default.Save();
                OnPropertyChanged();
            }
        }

        public string Port
        {
            get => Settings.Default.PortNumber;
            set
            {
                if (value == Settings.Default.PortNumber) return;
                Settings.Default.PortNumber = value;
                Settings.Default.Save();
                OnPropertyChanged();
            }
        }

        public bool AutoConnectOnOpen
        {
            get => Settings.Default.AutoConnectOnOpen;
            set
            {
                if (value == Settings.Default.AutoConnectOnOpen) return;
                Settings.Default.AutoConnectOnOpen = value;
                Settings.Default.Save();
                OnPropertyChanged();
            }
        }

        public bool LightsOffOnClose
        {
            get => Settings.Default.LightsOffOnClose;
            set
            {
                if (value == Settings.Default.LightsOffOnClose) return;
                Settings.Default.LightsOffOnClose = value;
                Settings.Default.Save();
                OnPropertyChanged();
            }
        }

        public bool ConnectionEst { get; set; }
        public ScreenVM ScreenVm { get; set; }
        public WebSocketConnector WebSocketConnector { get; set; }


        public Brush ConnectedForegroundColor
        {
            get => _connectedForegroundColor;
            set
            {
                if (Equals(value, _connectedForegroundColor)) return;
                _connectedForegroundColor = value;
                OnPropertyChanged();
            }
        }

        public string ConnectedLabel
        {
            get => _connectedLabel;
            set
            {
                if (value == _connectedLabel) return;
                _connectedLabel = value;
                OnPropertyChanged();
            }
        }

        public string ConnectButtonText
        {
            get => _connectButtonText;
            set
            {
                if (value == _connectButtonText) return;
                _connectButtonText = value;
                OnPropertyChanged();
            }
        }

        public ConnectToServerCommand ConnectToServerCommand { get; set; }


        public void SetConnectButtonAndLabel(bool connected)
        {
            if (!connected)
            {
                ConnectButtonText = "Connect to Server";
                if (string.IsNullOrEmpty(ConnectedLabel))
                {
                    ConnectedLabel = "Enter IP, Port and connect";
                }
                else
                {
                    ConnectedLabel = "disconnected";
                    ConnectedForegroundColor = Brushes.Red;
                }
            }
            else
            {
                ConnectButtonText = "Disconnect from Server";
                ConnectedLabel = "connected";
                ConnectedForegroundColor = Brushes.DarkGreen;
            }
        }

        public void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            ScreenVM.ThreadEnabled = false;
            //todo fix unschönheit bei thread abort
            ScreenVm.SlsWorkThread?.Abort();
            ScreenVm.SlsWorkThread?.Join();

            if (LightsOffOnClose && WebSocketConnection.ConnectionEst)
            {
                //send rgb 000 
                var r = new RgbData(0, 0, 0);
                r.SendValues(WebSocketConnection.WebSocket);
            }
            ConnectToServerCommand.CloseThreadAndWsConnection();
        }
    }
}