using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            cardSpace = 20;
        }

        public void SetTableau(Tableau tableau) {
            this.tableau = tableau;

            List<Card> cards = tableau.GetList();
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
