using System.Reflection;
using System.Windows.Media.Imaging;

namespace PilotRevitAddin.Utils
{
    public static class ImageUtils
    {
        public static BitmapImage NewBitmapImage(Assembly a, string imageName)
        {
            var s = a.GetManifestResourceStream("PilotRevitAddin.Resources." + imageName);
            var img = new BitmapImage();
            img.BeginInit();
            img.StreamSource = s;
            img.EndInit();
            return img;
        }
    }
}
