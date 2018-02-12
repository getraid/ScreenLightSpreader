using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ScreenLightSpreader.Annotations;
using ScreenLightSpreader.Command;
using ScreenLightSpreader.Model;

namespace ScreenLightSpreader.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private bool _automode;
        private string _ipAdress;
        public SaveSettingsCommand SaveSettingsCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public string IpAdress
        {
            get { return _ipAdress; }
            set
            {
                if (value == _ipAdress) return;
                _ipAdress = value;
                OnPropertyChanged();
            }
        }

        public bool Automode
        {
            get { return _automode; }
            set
            {
                if (value == _automode) return;
                _automode = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            SaveSettingsCommand = new SaveSettingsCommand(this);
            LoadSavedValues();
        }

        private void LoadSavedValues()
        {
            IpAdress = SaveManager.LoadIP();
            Automode = SaveManager.LoadAutoMode();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}