using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using Model;

namespace View {

    /// <summary>
    /// Interaction logic for StockView.xaml
    /// </summary>
    public partial class StockView : UserControl {

        private Stock stock;
        public Stock Stock {
            get {
                return stock;
            }
        }

        private CardView cardView;

        public StockView() {
            InitializeComponent();
            cardView = new CardView();
        }

        public void SetStock(Stock stock) {
            this.stock = stock;

            rootView.Children.Add(Util.CreateCardPlace(' '));

            RefreshView();
            Grid.SetZIndex(cardView, 1);
            rootView.Children.Add(cardView);
        }

        public void RefreshView() {
            Card card = stock.GetTopCard();
            cardView.Visibility = (card == null) ? Visibility.Hidden : Visibility.Visible;
            if (card == null) return;

            cardView.Card = card;
        }

        private void rootView_MouseUp(object sender, MouseButtonEventArgs e) {
            cardView.Animate(CardView.ANIM_SHAKE);
            GameView.Instance.HandOutFromStock();
        }
    }
}
