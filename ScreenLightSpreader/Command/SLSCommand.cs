using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using ScreenLightSpreader.Model;
using ScreenLightSpreader.ViewModel;

namespace ScreenLightSpreader.Command
{
    public class SLSCommand : ICommand
    {
        private readonly ScreenVM _screenVm;

        public SLSCommand(ScreenVM screenVm)
        {
            _screenVm = screenVm;
        }
        public bool CanExecute(object parameter)
        {
            //todo wsc connection est bei button
            // wenn connection est
            //  return WebSocketConnection.ConnectionEst;
            return true;
        }

        public void Execute(object parameter)
        {
            if (WebSocketConnection.ConnectionEst)
            {
                if (CheckIfSaturationIsFloat(out var fSatResult))
                {
                    return;
                }

                if (_screenVm.IsRunning == false && _screenVm.SlsWorkThread == null)
                {
                    StartWorkingThread(fSatResult);
                }
                else
                {
                    ExitWorkingThread();
                }
            }
            else
            {
                MessageBox.Show("Connect to server first, please.");
            }
        }

        /// <summary>
        /// Starts working thread
        /// </summary>
        /// <param name="fSatResult">Saturation float</param>
        private void StartWorkingThread(float fSatResult)
        {
            //better readabilty
            ScreenVM s = _screenVm;

            s.SlsWorkThread = new Thread(() => s.RgbManager.DoWork(WebSocketConnection.WebSocket, System.Convert.ToInt32(s.Buffer), s.DisplayToPixelManager, fSatResult));
            s.SlsWorkThread.Start();
            s.IsRunning = true;
        }

        /// <summary>
        /// Closes and sets work thread to null
        /// </summary>
        private void ExitWorkingThread()
        {
            _screenVm.SlsWorkThread?.Abort();
            if (_screenVm.SlsWorkThread != null)
            {
                _screenVm.SlsWorkThread = null;
            }

            _screenVm.IsRunning = false;
        }

        private bool CheckIfSaturationIsFloat(out float dSatResult)
        {
            if (!float.TryParse(_screenVm.SaturationMultiplier, out dSatResult))
            {
                MessageBox.Show("Please set a valid value for the saturation of the lights. (Standard: 10)");
                return true;
            }

            return false;
        }

        public event EventHandler CanExecuteChanged;
    }
}