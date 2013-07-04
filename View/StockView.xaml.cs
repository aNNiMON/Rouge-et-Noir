using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Model;

namespace View {

    /// <summary>
    /// Вид запаса.
    /// </summary>
    public partial class StockView : UserControl {

        private Stock _stock;
        private readonly CardView _cardView;

        public StockView() {
            InitializeComponent();
            _cardView = new CardView {Cursor = Cursors.Hand};
        }

        public void SetStock(Stock stock) {
            this._stock = stock;

            RootView.Children.Add(Util.CreateCardPlace(' '));

            RefreshView();
            Panel.SetZIndex(_cardView, 1);
            RootView.Children.Add(_cardView);
        }

        public void RefreshView() {
            Card card = _stock.GetTopCard();
            _cardView.Visibility = (card == null) ? Visibility.Hidden : Visibility.Visible;
            if (card == null) return;

            _cardView.Card = card;
        }

        private void rootView_MouseUp(object sender, MouseButtonEventArgs e) {
            // При нажатии на стопку раздаём карты.
            _cardView.Animate(CardView.AnimShake);
            GameView.Instance.HandOutFromStock();
        }
    }
}
