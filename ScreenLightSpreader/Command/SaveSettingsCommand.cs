using System;
using System.Windows.Input;
using ScreenLightSpreader.Model;
using ScreenLightSpreader.ViewModel;

namespace ScreenLightSpreader.Command
{
    public class SaveSettingsCommand : ICommand
    {
        private readonly MainViewModel _mainViewModel;

        public SaveSettingsCommand(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
        public bool CanExecute(object parameter)
        {

            return true;
        }

        public void Execute(object parameter)
        {
            SaveManager.SaveIp(_mainViewModel.IpAdress);
            SaveManager.SavePort(_mainViewModel.PortNumber);
            SaveManager.SaveIsAutostarting(_mainViewModel.IsAutostarting);
            SaveManager.SaveBuffTime(_mainViewModel.BufferTime);
        }

        public event EventHandler CanExecuteChanged;
    }
}