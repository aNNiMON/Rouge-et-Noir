using System.Windows.Controls;
using Model;

namespace View {

    /// <summary>
    /// Interaction logic for CardView.xaml
    /// </summary>
    public partial class CardView : UserControl {

        private Card card;

        public CardView() {
            InitializeComponent();
        }

        public void SetCard(Card card) {
            this.card = card;
            cardImage.Source = Util.LoadImage(card.ToString().ToLower());
        }

        
    }
}
