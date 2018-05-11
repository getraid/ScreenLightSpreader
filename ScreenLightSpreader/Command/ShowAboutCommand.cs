using System;
using System.Windows;
using System.Windows.Input;

namespace ScreenLightSpreader.Command
{
    public class ShowAboutCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            MessageBox.Show(
                "© Thomas Kellner / getraid.com. WTFPL Licence\n\nNuGet Plugins used: WebSocketSharp MIT, Extended WPF Toolkit™ Ms-PL");
        }

        public event EventHandler CanExecuteChanged;
    }
}