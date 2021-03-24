using ScreenLightSpreader.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScreenLightSpreader.Command
{
    public class ExitCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly GeneralVM GeneralVm;

        public ExitCommand(GeneralVM generalVm)
        {
            GeneralVm = generalVm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            GeneralVm.MainWindow_Closing(null, null);
            Environment.Exit(0);
        }
    }
}
