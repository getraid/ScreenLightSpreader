using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Navigation;
using ScreenLightSpreader.Model;
using ScreenLightSpreader.Properties;
using WebSocketSharp;
using Microsoft.WindowsAPICodePack.ApplicationServices;
using ScreenLightSpreader.ViewModel.Data;


namespace ScreenLightSpreader.ViewModel
{
    public class MainVM : BaseVM
    {
        private int _selectedIndex;
        public List<BaseVM> BaseVm { get; set; }
        public GeneralVM GeneralVm { get; set; }
        public ScreenVM ScreenVm { get; set; }
        public LEDVM LedVm { get; set; }

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

        //todo in systray minimieren
        public MainVM()
        {
            ScreenVm = new ScreenVM();
            GeneralVm = new GeneralVM(ScreenVm);
            LedVm = new LEDVM();

            BaseVm = new List<BaseVM> { GeneralVm, LedVm, ScreenVm, new OptionsVM() };

            if (Settings.Default.SelectedIndex < 0 && Settings.Default.SelectedIndex > BaseVm.Count - 1)
            {
                Settings.Default.SelectedIndex = 0;
            }
            SelectedIndex = Settings.Default.SelectedIndex;
            PowerManager.IsMonitorOnChanged += MonitorOnChanged;

        }



        void MonitorOnChanged(object sender, EventArgs e)
        {
            if (PowerManager.IsMonitorOn)
            {
               LedVm.SetSelectedColour();

            }
            else
            {
                RgbData r = new RgbData(0, 0, 0);
                r.SendValues(WebSocketConnection.WebSocket);
            }

        }

    }
}