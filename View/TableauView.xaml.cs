using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;
using Model;

namespace View {

    /// <summary>
    /// Interaction logic for TableauView.xaml
    /// </summary>
    public partial class TableauView : UserControl {

        private Tableau tableau;
        private int cardSpace;

        private List<CardView> cardViews;

        public TableauView() {
            InitializeComponent();
            cardViews = new List<CardView>();
            cardSpace = 15;
        }

        public void SetTableau(Tableau tableau) {
            this.tableau = tableau;

            List<Card> cards = tableau.GetList();
            rootView.Children.Add(Util.CreateCardPlace('K'));
            for (int i = 0; i < cards.Count; i++) {
                var card = cards[i];
                CardView cardView = new CardView();

                Canvas.SetTop(cardView, cardSpace * i);
                Canvas.SetZIndex(cardView, 1 + i);
                cardView.SetCard(card);

                rootView.Children.Add(cardView);
                cardViews.Add(cardView);
            }
        }

        public void SetCardSpace(int cardSpace) {
            this.cardSpace = cardSpace;
        }

        public void RefreshView() {
            List<Card> cards = tableau.GetList();
            for (int i = 0; i < cards.Count; i++) {
                var card = cards[i];

                CardView cardView;
                if (cardViews.Count < i) {
                    cardView = cardViews[i];
                    cardView.SetCard(card);
                } else {
                    cardView = new CardView();
                    cardView.SetCard(card);
                    cardViews.Add(cardView);
                    Canvas.SetTop(cardView, cardSpace * i);
                    Canvas.SetZIndex(cardView, 1 + i);
                    rootView.Children.Add(cardView);
                }
            }
            // Удаляем лишнее
            for (int i = cardViews.Count - 1; i >= cards.Count; i--) {
                CardView v = cardViews[i];
                rootView.Children.Remove(v);
                cardViews.Remove(v);
            }
        }
    }
}
