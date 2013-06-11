using System.Collections.Generic;
using System.Windows.Controls;
using Model;

namespace View {

    /// <summary>
    /// Interaction logic for DraggableCards.xaml
    /// </summary>
    public partial class DraggableCards : UserControl {

        private List<Card> cards;
        public List<Card> Cards {
            get {
                return cards;
            }
            set {
                cards = value;

                for (int i = 0; i < cards.Count; i++) {
                    var card = cards[i];
                    var cardView = new CardView();
                    AddCard(cardView, card, i);
                }
            }
        }

        private CardView bottomCardView;
        public CardView BottomCardView {
            get {
                return bottomCardView;
            }
        }

        public DraggableCards() {
            InitializeComponent();
        }

        private void AddCard(CardView cardView, Card card, int index) {
            if (index == 0) bottomCardView = cardView;
            cardView.Card = card;
            Canvas.SetTop(cardView, 15 * index);
            Canvas.SetZIndex(cardView, 1 + index);
            rootView.Children.Add(cardView);
        }
    }
}
