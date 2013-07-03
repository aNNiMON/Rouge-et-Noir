using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using View.ViewModel;

namespace View {

    /// <summary>
    /// Вид результирующей стопки.
    /// </summary>
    public partial class FoundationView : UserControl {

        public FoundationViewModel Model {
            get;
            private set;
        }

        public FoundationView() {
            InitializeComponent();
            Model = new FoundationViewModel();
            cardPlace.DataContext = Model;
            cardView.DataContext = Model;
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
    }
}
