using System.ComponentModel;
using Model;

namespace View.ViewModel {

    public class FoundationViewModel : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;

        public Foundation Foundation {
            get { return foundation; }
            set {
                foundation = value;
                OnPropertyChanged("Foundation");
                OnPropertyChanged("Card");
            }
        }
        private Foundation foundation;


        public Card Card {
            get { return foundation.GetTopCard(); }
        }


        public bool IsLeft {
            get { return isLeft; }
            set {
                isLeft = value;
                OnPropertyChanged("IsLeft");
            }
        }
        private bool isLeft;



        protected void OnPropertyChanged(string name) {
            var handler = PropertyChanged;
            if (null != handler) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public void RefreshView() {
            OnPropertyChanged("Card");
        }
    }
}
