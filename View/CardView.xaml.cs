using System.Windows.Controls;
using System.Windows.Media.Animation;
using Model;

namespace View {

    /// <summary>
    /// Interaction logic for CardView.xaml
    /// </summary>
    public partial class CardView : UserControl {

        /// <summary>
        /// Константы для применения анимации к карте.
        /// </summary>
        public const string
            ANIM_FADE_IN = "animFadeIn",
            ANIM_FADE_OUT = "animFadeOut",
            ANIM_SHAKE = "animShake";

        public Card Card {
            get {
                return card;
            }
            set {
                card = value;
                cardImage.Source = Util.LoadImage(card.GetImageResourcePath());
            }
        }
        private Card card;

        public CardView() {
            InitializeComponent();
        }

        /// <summary>
        /// Применить анимацию.
        /// </summary>
        /// <param name="animation">ресурс анимации, прописанный в XAML</param>
        public void Animate(string animation) {
            var s = (Storyboard) Resources[animation];
            s.Begin();
        }

    }
}
