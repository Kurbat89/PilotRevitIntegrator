using Ascon.Pilot.Theme;
using Ascon.Pilot.Theme.ColorScheme;
using System.Windows.Controls;
using System.Windows.Media;

namespace PilotRevitAddin.SynchronizeProject
{
    /// <summary>
    /// Interaction logic for SynchronizeView.xaml
    /// </summary>
    public partial class SynchronizeView : UserControl
    {
        public SynchronizeView()
        {
            var accentColor = (Color)ColorConverter.ConvertFromString("#FF7F95C8");
            ColorScheme.Initialize(accentColor);
            var color = AccentColor.Value;

            InitializeComponent();
        }
    }
}
