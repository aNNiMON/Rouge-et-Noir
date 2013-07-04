using System.Collections.Generic;
using System.Windows.Controls;
using Model;

namespace View {

    /// <summary>
    /// Перемещаемые карты.
    /// </summary>
    public partial class DraggableCards : UserControl {

        /// <summary>
        /// Список перемещаемых карт.
        /// </summary>
        public List<Card> Cards {
            get {
                return _cards;
            }
            set {
                _cards = value;

                for (int i = 0; i < _cards.Count; i++) {
                    var card = _cards[i];
                    var cardView = new CardView();
                    AddCard(cardView, card, i);
                }
            }
        }
        private List<Card> _cards;

        /// <summary>
        /// Вид нижней карты.
        /// Служит для определения корректности перемещения карт.
        /// </summary>
        public CardView BottomCardView {
            get {
                return _bottomCardView;
            }
        }
        private CardView _bottomCardView;

        public DraggableCards() {
            InitializeComponent();
        }

        private void AddCard(CardView cardView, Card card, int index) {
            if (index == 0) _bottomCardView = cardView;
            cardView.Card = card;
            Canvas.SetTop(cardView, 15 * index);
            Panel.SetZIndex(cardView, 1 + index);
            RootView.Children.Add(cardView);
        }
    }
}
