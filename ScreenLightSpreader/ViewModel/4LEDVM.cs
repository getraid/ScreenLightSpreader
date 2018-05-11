using System.Windows.Media;

namespace ScreenLightSpreader.ViewModel
{
    public class LEDVM : BaseVM
    {
        private Color _selectedColor;

        public Color SelectedColor
        {
            get => _selectedColor;
            set
            {
                if (value.Equals(_selectedColor)) return;
                _selectedColor = value;
                OnPropertyChanged();
            }
        }

        public LEDVM()
        {
            //load color from setting
            SelectedColor = Colors.Red;
        }
        //todo ledvm
    }
}