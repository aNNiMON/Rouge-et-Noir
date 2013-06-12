using System.Windows.Controls;
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
        public Card Card {
            get {
                return card;
            }
            set {
                card = value;
                cardImage.Source = Util.LoadImage(card.GetImageResourcePath());
            }
        }

        public CardView() {
            InitializeComponent();
        }

        public void Animate(string animation) {
            var s = (Storyboard) Resources[animation];
            s.Begin();
        }

    }
}
