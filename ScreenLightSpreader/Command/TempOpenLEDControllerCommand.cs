using System;
using System.Windows.Input;
using ScreenLightSpreader.ViewModel;

namespace ScreenLightSpreader.Command
{
    public class TempOpenLEDControllerCommand:ICommand
    {
        private readonly MainViewModel _mainViewModel;

        public TempOpenLEDControllerCommand(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            System.Diagnostics.Process.Start("http://"+_mainViewModel.IpAdress);
        }

        public event EventHandler CanExecuteChanged;
    }
}