using System;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using ScreenLightSpreader.ViewModel;
using WebSocketSharp;

namespace ScreenLightSpreader.Command
{
    public class StartCommand : ICommand
    {
        public MainViewModel _mainViewModel { get; }

        public StartCommand(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
        public bool CanExecute(object parameter)
        {
            if (!(string.IsNullOrEmpty((_mainViewModel.IpAdress))) && !(string.IsNullOrEmpty(_mainViewModel.PortNumber)))
            {
                return true;
            }
            return false;

        }

        public void Execute(object parameter)
        {
            //UMSCHREIBEN-> ws.OnOpen/OnClose um fehler und status zu setzen

            if (IPAddress.TryParse(_mainViewModel.IpAdress, out var ipAdress) && int.TryParse(_mainViewModel.PortNumber, out var trashInt))
            {
                string Ip = Convert.ToString(ipAdress);
                _mainViewModel.ws = new WebSocketSharp.WebSocket("ws://" + Ip + ":" + _mainViewModel.PortNumber);

                if (_mainViewModel.WebSocketConnector.OpenConnection(_mainViewModel.ws))
                {
                    _mainViewModel.Automode = true;
                    _mainViewModel.ConnectedVisibility = Visibility.Visible;
                }
                else
                {
                    _mainViewModel.ConnectedVisibility = Visibility.Hidden;
                    _mainViewModel.Automode = false;
                    MessageBox.Show("Couldn't reach Websocket-Server. Timeout or wrong port");
                }
            }
            else
            {
                _mainViewModel.ConnectedVisibility = Visibility.Hidden;
                _mainViewModel.Automode = false;
                MessageBox.Show("Couldn't find Websocket-Server. Ip or Port wrong");
            }
        }

        private event EventHandler CanExecuteChanged;

        event EventHandler ICommand.CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}