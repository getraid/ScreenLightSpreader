using System;
using System.Windows.Input;
using ScreenLightSpreader.ViewModel;

namespace ScreenLightSpreader.Command
{
    public class ResetSettingsCommand:ICommand
    {
        private readonly OptionsVM _optionsVm;

        public ResetSettingsCommand(OptionsVM optionsVm)
        {
            _optionsVm = optionsVm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

        public event EventHandler CanExecuteChanged;
    }
}