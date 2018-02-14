using System;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using ScreenLightSpreader.Model;
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
            if (IPAddress.TryParse(_mainViewModel.IpAdress, out var ipAdress) && int.TryParse(_mainViewModel.PortNumber, out var trashInt) && int.TryParse(_mainViewModel.BufferTime, out var buffRes))
            {
                _mainViewModel.BufferTimeInt = buffRes;
                if (!_mainViewModel.Running)
                {
                    string Ip = Convert.ToString(ipAdress);
                    _mainViewModel.ws = new WebSocketSharp.WebSocket("ws://" + Ip + ":" + _mainViewModel.PortNumber);
                    _mainViewModel.WebSocketConnector.InitEventHandlers(_mainViewModel);
                    StartThreadAndWsConnection();
                }
                else
                {
                    _mainViewModel.MainWorkThread.Abort();
                    _mainViewModel.MainWorkThread = null;
                    _mainViewModel.ws.Close();
                }
            }
            else
            {
                MessageBox.Show("There seems to be an error in your input.");
            }
        }

        private void StartThreadAndWsConnection()
        {
            if (_mainViewModel.WebSocketConnector.OpenConnection(_mainViewModel.ws))
            {
                _mainViewModel.CreateThread();
                _mainViewModel.MainWorkThread.Start();
            }
            else
            {
                MessageBox.Show("Couldn't reach Websocket-Server. Timeout or wrong port");
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