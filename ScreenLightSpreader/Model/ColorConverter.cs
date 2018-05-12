namespace ScreenLightSpreader.Model
{
    public static class ColorConverter
    {
        public static System.Windows.Media.Color GetSystemColor(System.Drawing.Color Col)
        {
            return System.Windows.Media.Color.FromArgb(Col.A, Col.R, Col.G, Col.B);
        }

        public static System.Drawing.Color GetDrawingColor(System.Windows.Media.Color Col)
        {
            return System.Drawing.Color.FromArgb(Col.A, Col.R, Col.G, Col.B);
        }
    }
}