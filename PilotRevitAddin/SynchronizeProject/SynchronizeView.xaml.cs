using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ascon.Pilot.Theme.ColorScheme;
using Ascon.Pilot.Themes;

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

            InitializeComponent();
        }
    }
}
