using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace View {

    public static class Util {
        
        private const string CARD_IMAGE_FOLDER = "1/";
        private const string RESOURCE_URI = "pack://application:,,,/View;component/Resources/";

        public static BitmapImage LoadImage(string name) {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(RESOURCE_URI + CARD_IMAGE_FOLDER + name + ".png");
            bitmapImage.EndInit();
            return bitmapImage;
        }

        public static void LoadRtf(this RichTextBox rtb, string name) {
            var textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            Uri uri = new Uri(RESOURCE_URI + name + ".rtf");
            StreamResourceInfo info = Application.GetResourceStream(uri);
            textRange.Load(info.Stream, DataFormats.Rtf);
        }

        public static CardPlaceView CreateCardPlace(char symbol, int zIndex = 0) {
            var view = new CardPlaceView(symbol);
            Panel.SetZIndex(view, zIndex);

            return view;
        }

        public static int GetMaxZIndex(UIElementCollection collection) {
            int maxZIndex = 0;
            foreach (UIElement element in collection) {
                int z = Panel.GetZIndex(element);
                if (z > maxZIndex) {
                    maxZIndex = z;
                }
            }
            return maxZIndex + 1;
        }

        public static Rect GetBoundingRect(Visual view, Visual relativeTo = null) {
            if (relativeTo == null) relativeTo = GameView.Instance.GetRootView();
            Vector relativeOffset = new Point() - relativeTo.PointToScreen(new Point());

            var result = new Rect(view.PointToScreen(new Point()) + relativeOffset, VisualTreeHelper.GetDescendantBounds(view).Size);
            return result;
        }

        public static Rect GetBoundingRect(List<Visual> visuals, Visual relativeTo = null) {
            if (relativeTo == null) relativeTo = GameView.Instance.GetRootView();
            Vector relativeOffset = new Point() - relativeTo.PointToScreen(new Point());

            List<Rect> rects = visuals
                .Select(v => new Rect(v.PointToScreen(new Point()) + relativeOffset, VisualTreeHelper.GetDescendantBounds(v).Size))
                .ToList();

            Rect result = rects[0];
            for (int i = 1; i < rects.Count; i++)
                result.Union(rects[i]);
            return result;
        }
    }
}
