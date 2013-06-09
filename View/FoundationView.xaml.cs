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
    /// Interaction logic for FoundationView.xaml
    /// </summary>
    public partial class FoundationView : UserControl {

        private Foundation foundation;
        private bool isLeft;

        public FoundationView() {
            InitializeComponent();
        }

        public void SetFoundation(Foundation foundation, bool isLeft) {
            this.foundation = foundation;
            this.isLeft = isLeft;

            rootView.Children.Add(Util.CreateCardPlace(isLeft ? 'A' : ' '));

            Card card = foundation.GetTopCard();
            if (card == null) return;

            CardView cardView = new CardView();
            Grid.SetZIndex(cardView, 1);
            cardView.SetCard(card);

            rootView.Children.Add(cardView);
        }
    }
}
