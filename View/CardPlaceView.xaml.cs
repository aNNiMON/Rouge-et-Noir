using System.Windows.Controls;

namespace View {

    /// <summary>
    /// Рамка с текстом отражающая место под карту.
    /// </summary>
    public partial class CardPlaceView : UserControl {

        public CardPlaceView() {
            InitializeComponent();
        }

        public CardPlaceView(char symbol) : this() {
            SetPlaceChar(symbol);
        }

        public void SetPlaceChar(char symbol) {
            label.Content = symbol;
        }
    }
}
