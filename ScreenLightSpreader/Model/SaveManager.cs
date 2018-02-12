using System.Configuration;
using ScreenLightSpreader.Command;

namespace ScreenLightSpreader.Model
{
    public class SaveManager
    {
        public static void SaveIP(string ip)
        {
            Properties.Settings.Default.IpAdress = ip;
            Properties.Settings.Default.Save();
        }

        public static string LoadIP()
        {
            Properties.Settings.Default.Upgrade();
            return Properties.Settings.Default.IpAdress;
        }

        public static void SaveAutoMode(bool auto)
        {

            Properties.Settings.Default.automode = auto;
            Properties.Settings.Default.Save();
        }

        public static bool LoadAutoMode()
        {
            Properties.Settings.Default.Upgrade();

            return Properties.Settings.Default.automode;

        }
    }
}