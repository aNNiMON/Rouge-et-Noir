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
    /// Interaction logic for StockView.xaml
    /// </summary>
    public partial class StockView : UserControl {

        private Stock stock;
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
            if (card == null) return;

            cardView.SetCard(card);
        }
    }
}
