using System.Windows.Controls;

namespace View {

    /// <summary>
    /// Рамка с текстом, отражающая место под карту.
    /// </summary>
    public partial class CardPlaceView : UserControl {

        public char Symbol { get; set; }

        public CardPlaceView() {
            InitializeComponent();
            DataContext = this;
        }
    }
}
