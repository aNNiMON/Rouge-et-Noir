using System.Windows;
using System.Windows.Controls;

namespace View {

    /// <summary>
    /// Рамка с текстом, отражающая место под карту.
    /// </summary>
    public partial class CardPlaceView : UserControl {

        public static readonly DependencyProperty SymbolProperty =
            DependencyProperty.Register("Symbol", typeof(char), typeof(CardPlaceView));

        public char Symbol {
            get {
                return (char) GetValue(SymbolProperty);
            }
            set {
                SetValue(SymbolProperty, value);
            }
        }

        public CardPlaceView() {
            InitializeComponent();
            DataContext = this;
        }
    }
}
