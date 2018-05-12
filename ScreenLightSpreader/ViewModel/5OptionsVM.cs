using System.Collections.Generic;
using ScreenLightSpreader.Command;
using ScreenLightSpreader.Properties;

namespace ScreenLightSpreader.ViewModel
{
    public class OptionsVM : BaseVM
    {
        public ShowAboutCommand ShowAboutCommand { get; set; }
        public List<string> StandardBootupChoice { get; set; }
        public ResetSettingsCommand ResetSettingsCommand { get; set; }

        public int SelectedIndex
        {
            get => Settings.Default.SelectedIndex;
            set
            {
                if (value == Settings.Default.SelectedIndex) return;
                Settings.Default.SelectedIndex = value;
                Settings.Default.Save();
                OnPropertyChanged();
            }
        }

        public OptionsVM()
        {
            StandardBootupChoice = new List<string> { "General Settings", "LED Controller", "Screen Average Program" };
            ResetSettingsCommand = new ResetSettingsCommand(this);
            ShowAboutCommand = new ShowAboutCommand();
        }
    }
}