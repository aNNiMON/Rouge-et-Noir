using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace View {

    public static class Util {
        
        private const string CARD_IMAGE_FOLDER = "1/";
        private const string RESOURCE_URI = "pack://application:,,,/View;component/Resources/";

        public static BitmapImage LoadImage(string name) {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(RESOURCE_URI + CARD_IMAGE_FOLDER + name + ".png");
            bitmapImage.EndInit();
            return bitmapImage;
        }
    }
}
