using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using ScreenLightSpreader.Properties;
using ScreenLightSpreader.ViewModel;

namespace ScreenLightSpreader.Command
{
    public class ResetSettingsCommand : ICommand
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
            Settings.Default.Reset();
            Application.Restart();
            Thread.Sleep(5);
            //needed a quick way ... sooo...
            Process.GetCurrentProcess().Kill();
        }

        public event EventHandler CanExecuteChanged;
    }
}