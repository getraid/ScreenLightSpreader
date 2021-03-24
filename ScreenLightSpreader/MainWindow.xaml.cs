using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;

namespace ScreenLightSpreader
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();

            // Notify icon
            NotifyIcon notifyIcon = new NotifyIcon
            {
                Visible = true,
                Icon = ScreenLightSpreader.Properties.Resources.sec
            };
            notifyIcon.Click += OpenSLS;

            if (ScreenLightSpreader.Properties.Settings.Default.MinimizeOnStartup)
            {
                this.Hide();
            }
        }

        private void OpenSLS(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            this.Focus();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                this.Hide();
            }

            base.OnStateChanged(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (ScreenLightSpreader.Properties.Settings.Default.CloseDefaultToTray)
            {
                e.Cancel = true;
                this.Hide();

            }
            else
            {
                base.OnClosing(e);
            }

        }
    }
}