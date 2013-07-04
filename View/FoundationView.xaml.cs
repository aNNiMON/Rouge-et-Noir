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
                return _foundation;
            }
        }
        private Foundation _foundation;

        private readonly CardView _cardView;

        public FoundationView() {
            InitializeComponent();
            _cardView = new CardView();
        }

        /// <summary>
        /// Получить размер занимаемой на экране области.
        /// </summary>
        public Rect Bounds {
            get {
                var list = new List<Visual>();
                list.Add(RootView.Children[0]);
                list.Add(_cardView);
                return Util.GetBoundingRect(list);
            }
        }

        public void SetFoundation(Foundation foundation, bool isLeft) {
            this._foundation = foundation;

            RootView.Children.Add(Util.CreateCardPlace(isLeft ? 'A' : ' '));

            RefreshView();
            Panel.SetZIndex(_cardView, 1);
            RootView.Children.Add(_cardView);
        }

        public void RefreshView() {
            Card card = _foundation.GetTopCard();
            _cardView.Visibility = (card == null) ? Visibility.Hidden : Visibility.Visible;
            if (card == null) return;

            _cardView.Card = card;
        }
    }
}
