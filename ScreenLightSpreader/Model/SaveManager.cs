using System.Configuration;
using ScreenLightSpreader.Command;

namespace ScreenLightSpreader.Model
{
    public class SaveManager
    {
        public static void SaveIp(string ip)
        {
            Properties.Settings.Default.IpAdress = ip;
            Properties.Settings.Default.Save();
        }

        public static string LoadIp()
        {
            Properties.Settings.Default.Upgrade();
            return Properties.Settings.Default.IpAdress;
        }

        public static void SavePort(string port)
        {
            Properties.Settings.Default.PortNumber = port;
            Properties.Settings.Default.Save();
        }

        public static string LoadPort()
        {
            Properties.Settings.Default.Upgrade();
            return Properties.Settings.Default.PortNumber;
        }


        public static void SaveIsAutostarting(bool autostarting)
        {
            Properties.Settings.Default.isAutostarting = autostarting;
            Properties.Settings.Default.Save();
        }

        public static bool LoadIsAutostarting()
        {
            Properties.Settings.Default.Upgrade();
            return Properties.Settings.Default.isAutostarting;

        }

    }
}