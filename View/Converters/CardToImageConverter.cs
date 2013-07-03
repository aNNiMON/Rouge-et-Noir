using System;
using System.Globalization;
using System.Windows.Data;
using Model;

namespace View.Converters {

    public class CardToImageConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            Card card = (Card) value;
            if (card == null) return null;

            return Util.LoadImage(card.GetImageResourcePath());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
