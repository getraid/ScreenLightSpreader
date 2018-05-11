using System;
using System.Collections.Generic;
using System.Windows.Navigation;
using ScreenLightSpreader.Model;
using ScreenLightSpreader.Properties;
using WebSocketSharp;

namespace ScreenLightSpreader.ViewModel
{
    public class MainVM : BaseVM
    {
        private int _selectedIndex;
        public List<BaseVM> BaseVm { get; set; }
        public GeneralVM GeneralVm { get; set; }
        public ScreenVM ScreenVm { get; set; }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (value == _selectedIndex) return;
                _selectedIndex = value;
                OnPropertyChanged();
            }
        }

        public MainVM()
        {
            ScreenVm = new ScreenVM();
            GeneralVm = new GeneralVM(ScreenVm);
            BaseVm = new List<BaseVM> { GeneralVm, ScreenVm, new LEDVM(), new OptionsVM() };

            if (Settings.Default.SelectedIndex < 0 && Settings.Default.SelectedIndex > BaseVm.Count - 1)
            {
                Settings.Default.SelectedIndex = 0;
            }
            SelectedIndex = Settings.Default.SelectedIndex;


        }
    }
}