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

    /// <summary>
    /// Класс вспомогательных функций.
    /// </summary>
    public static class Util {
        
        private const string CARD_IMAGE_FOLDER = "1/";
        private const string RESOURCE_URI = "pack://application:,,,/View;component/Resources/";

        /// <summary>
        /// Заргузка изображения из ресурсов.
        /// </summary>
        /// <param name="name">имя файла в ресурсах без расширения</param>
        /// <returns></returns>
        public static BitmapImage LoadImage(string name) {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(RESOURCE_URI + CARD_IMAGE_FOLDER + name + ".png");
            bitmapImage.EndInit();
            return bitmapImage;
        }

        /// <summary>
        /// Загрузка RTF-документа из ресурсов в компонент RichTextBox.
        /// </summary>
        /// <param name="rtb">компонент RichTextBox</param>
        /// <param name="name">имя файла в ресурсах без расширения</param>
        public static void LoadRtf(this RichTextBox rtb, string name) {
            var textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            var uri = new Uri(RESOURCE_URI + name + ".rtf");
            StreamResourceInfo info = Application.GetResourceStream(uri);
            if (info != null) textRange.Load(info.Stream, DataFormats.Rtf);
        }

        /// <summary>
        /// Создание вида места под карту.
        /// </summary>
        /// <param name="symbol">символ, отображаемый в рамке</param>
        /// <param name="zIndex"></param>
        /// <returns></returns>
        public static CardPlaceView CreateCardPlace(char symbol, int zIndex = 0) {
            var view = new CardPlaceView(symbol);
            Panel.SetZIndex(view, zIndex);

            return view;
        }

        /// <summary>
        /// Получить размер занимаемой на экране области.
        /// </summary>
        /// <param name="view">компонент, размер которого нужно посчитать</param>
        /// <param name="relativeTo">относительно какого компонента вывести результат</param>
        /// <returns>рамка</returns>
        public static Rect GetBoundingRect(Visual view, Visual relativeTo = null) {
            if (relativeTo == null) relativeTo = GameView.Instance.GetRootView();
            Vector relativeOffset = new Point() - relativeTo.PointToScreen(new Point());

            var result = new Rect(view.PointToScreen(new Point()) + relativeOffset, VisualTreeHelper.GetDescendantBounds(view).Size);
            return result;
        }

        /// <summary>
        /// Получить размер занимаемой на экране области нескольких компонент.
        /// </summary>
        /// <param name="visuals">список компонент, размер котороых нужно посчитать</param>
        /// <param name="relativeTo">относительно какого компонента вывести результат</param>
        /// <returns>рамка</returns>
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
