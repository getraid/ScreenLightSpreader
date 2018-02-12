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
            SaveManager.SaveIP(_mainViewModel.IpAdress);
           SaveManager.SaveAutoMode(_mainViewModel.Automode);
        }

        public event EventHandler CanExecuteChanged;
    }
}