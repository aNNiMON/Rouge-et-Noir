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

        public TableauView() {
            InitializeComponent();
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
            }
        }

        public void SetCardSpace(int cardSpace) {
            this.cardSpace = cardSpace;
        }
    }
}
