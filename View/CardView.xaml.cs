using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Model;

namespace View {

    /// <summary>
    /// Interaction logic for CardView.xaml
    /// </summary>
    public partial class CardView : UserControl {

        public const string
            ANIM_FADE_IN = "animFadeIn",
            ANIM_FADE_OUT = "animFadeOut",
            ANIM_SHAKE = "animShake";

        private Card card;

        public CardView() {
            InitializeComponent();
        }

        public void SetCard(Card card) {
            this.card = card;
            cardImage.Source = Util.LoadImage(card.GetImageResourcePath());
        }

        public Card GetCard() {
            return card;
        }

        public void Animate(string animation) {
            Storyboard s = (Storyboard) Resources[animation];
            s.Begin();
        }

    }
}
