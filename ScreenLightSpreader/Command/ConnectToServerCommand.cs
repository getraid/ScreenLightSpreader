using System;
using System.ComponentModel;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using ScreenLightSpreader.ViewModel;
using ScreenLightSpreader.ViewModel.Data;

namespace ScreenLightSpreader.Command
{
    public class ConnectToServerCommand : ICommand
    {
        private readonly GeneralVM GeneralVm;

        public ConnectToServerCommand(GeneralVM generalVm)
        {
            GeneralVm = generalVm;
        }
        public bool CanExecute(object parameter)
        {
            if (!string.IsNullOrEmpty(GeneralVm.IpAdress) && !string.IsNullOrEmpty(GeneralVm.Port))
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// Connects to websocket server with the given port and ip in general
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            //todo in thread auslagern
            if (IPAddress.TryParse(GeneralVm.IpAdress, out var IpAdress) && int.TryParse(GeneralVm.Port, out var trashResult))
            {
                if (!WebSocketConnection.ConnectionEst)
                {
                    string Ip = Convert.ToString(IpAdress);
                    WebSocketConnection.WebSocket = new WebSocketSharp.WebSocket("ws://" + Ip + ":" + GeneralVm.Port);
                    GeneralVm.WebSocketConnector.InitEventHandlers(WebSocketConnection.WebSocket);


                    StartThreadAndWsConnection();
                }
                else
                {
                    CloseThreadAndWsConnection();
                }

            }
            else
            {
                MessageBox.Show("There seems to be an error in your input.");

            }

        }





        private void StartThreadAndWsConnection()
        {
            if (GeneralVm.WebSocketConnector.OpenConnection(WebSocketConnection.WebSocket))
            {
                SetConnected(true);
                //test
                RgbData rgb = new RgbData(0, 255, 0);

                rgb.SendValues(WebSocketConnection.WebSocket);
            }
            else
            {

                MessageBox.Show("Couldn't reach Websocket-Server. Timeout or wrong port");
                SetConnected(false);
            }

        }

        private void SetConnected(bool isConnected)
        {
            GeneralVm.SetConnectButtonAndLabel(isConnected);
            WebSocketConnection.ConnectionEst = isConnected;
        }

        public void CloseThreadAndWsConnection()
        {
            //todo add lights off on close when conn closed
            SetConnected(false);
            ScreenVM.ThreadEnabled = false;
            GeneralVm.ScreenVm.SlsWorkThread?.Join();
            WebSocketConnection.WebSocket?.Close();
            WebSocketConnection.WebSocket = null;
        }


        public event EventHandler CanExecuteChanged;
        event EventHandler ICommand.CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}