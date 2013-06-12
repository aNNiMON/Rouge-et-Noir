using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Model;

namespace View {

    /// <summary>
    /// Вид запаса.
    /// </summary>
    public partial class StockView : UserControl {

        private Stock stock;
        private readonly CardView cardView;

        public StockView() {
            InitializeComponent();
            cardView = new CardView {Cursor = Cursors.Hand};
        }

        public void SetStock(Stock stock) {
            this.stock = stock;

            rootView.Children.Add(Util.CreateCardPlace(' '));

            RefreshView();
            Panel.SetZIndex(cardView, 1);
            rootView.Children.Add(cardView);
        }

        public void RefreshView() {
            Card card = stock.GetTopCard();
            cardView.Visibility = (card == null) ? Visibility.Hidden : Visibility.Visible;
            if (card == null) return;

            cardView.Card = card;
        }

        private void rootView_MouseUp(object sender, MouseButtonEventArgs e) {
            // При нажатии на стопку раздаём карты.
            cardView.Animate(CardView.ANIM_SHAKE);
            GameView.Instance.HandOutFromStock();
        }
    }
}
