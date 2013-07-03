using System;
using System.Globalization;
using System.Windows.Data;
using Model;

namespace View.Converters {

    public class CardToViewConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            Card card = (Card) value;
            var cardView = new CardView();
            cardView.Card = card;
            return cardView;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
