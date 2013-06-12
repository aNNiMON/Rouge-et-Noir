using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Model;

namespace View {

    /// <summary>
    /// Вид результирующей стопки.
    /// </summary>
    public partial class FoundationView : UserControl {

        public Foundation Foundation {
            get {
                return foundation;
            }
        }
        private Foundation foundation;

        private readonly CardView cardView;

        public FoundationView() {
            InitializeComponent();
            cardView = new CardView();
        }

        /// <summary>
        /// Получить размер занимаемой на экране области.
        /// </summary>
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

            rootView.Children.Add(Util.CreateCardPlace(isLeft ? 'A' : ' '));

            RefreshView();
            Panel.SetZIndex(cardView, 1);
            rootView.Children.Add(cardView);
        }

        public void RefreshView() {
            Card card = foundation.GetTopCard();
            cardView.Visibility = (card == null) ? Visibility.Hidden : Visibility.Visible;
            if (card == null) return;

            cardView.Card = card;
        }
    }
}
