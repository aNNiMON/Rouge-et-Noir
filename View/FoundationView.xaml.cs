using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Model;

namespace View {

    /// <summary>
    /// Interaction logic for FoundationView.xaml
    /// </summary>
    public partial class FoundationView : UserControl {

        private Foundation foundation;
        public Foundation Foundation {
            get {
                return foundation;
            }
        }
        private bool isLeft;

        private CardView cardView;

        public FoundationView() {
            InitializeComponent();
            cardView = new CardView();
        }

        public Rect Bounds {
            get {
                var list = new List<Visual>();
                list.Add(rootView.Children[0]);
                list.Add(cardView);
                return Util.GetBoundingRect(list);
            }
        }

        public void SetFoundation(Foundation foundation, bool isLeft) {
            this.foundation = foundation;
            this.isLeft = isLeft;

            rootView.Children.Add(Util.CreateCardPlace(isLeft ? 'A' : ' '));

            RefreshView();
            Grid.SetZIndex(cardView, 1);
            rootView.Children.Add(cardView);
        }

        public void RefreshView() {
            Card card = foundation.GetTopCard();
            cardView.Visibility = (card == null) ? Visibility.Hidden : Visibility.Visible;
            if (card == null) return;

            cardView.SetCard(card);
        }
    }
}
