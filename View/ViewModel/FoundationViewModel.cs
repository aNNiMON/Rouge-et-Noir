using System.ComponentModel;
using Model;

namespace View.ViewModel {

    public class FoundationViewModel : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;

        public Card Card {
            get { return topCard; }
            set {
                topCard = value;
                OnPropertyChanged("Card");
            }
        }
        private Card topCard;

        protected void OnPropertyChanged(string name) {
            var handler = PropertyChanged;
            if (null != handler) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
