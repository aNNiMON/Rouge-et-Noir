using System.Windows.Controls;

namespace View {

    /// <summary>
    /// Рамка с текстом, отражающая место под карту.
    /// </summary>
    public partial class CardPlaceView : UserControl {

        public CardPlaceView() {
            InitializeComponent();
        }

        public CardPlaceView(char symbol) : this() {
            Label.Content = symbol;
        }
    }
}
