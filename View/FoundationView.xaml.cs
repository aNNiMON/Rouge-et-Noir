using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Model;
using View.ViewModel;

namespace View {

    /// <summary>
    /// Вид результирующей стопки.
    /// </summary>
    public partial class FoundationView : UserControl {

        public Foundation Foundation {
            get {
                return foundation;
            }
            set {
                foundation = value;
                model.Card = foundation.GetTopCard();
            }
        }
        private Foundation foundation;

        private FoundationViewModel model;

        public FoundationView() {
            InitializeComponent();
            model = new FoundationViewModel();
            cardView.DataContext = model;
        }

        /// <summary>
        /// Получить размер занимаемой на экране области.
        /// </summary>
        public Rect Bounds {
            get {
                var list = new List<Visual>();
                list.Add(cardPlace);
                list.Add(cardView);
                return Util.GetBoundingRect(list);
            }
        }

        public void SetFoundation(Foundation foundation, bool isLeft) {
            this.foundation = foundation;
            model.Card = foundation.GetTopCard();
            cardPlace.Symbol = (isLeft ? 'A' : ' ');
            RefreshView();
        }

        public void RefreshView() {
            model.Card = foundation.GetTopCard();
        }
    }
}
