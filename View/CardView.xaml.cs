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
            AnimFadeIn = "AnimFadeIn",
            AnimFadeOut = "AnimFadeOut",
            AnimShake = "AnimShake";

        public Card Card {
            get {
                return _card;
            }
            set {
                _card = value;
                CardImage.Source = Util.LoadImage(_card.GetImageResourcePath());
            }
        }
        private Card _card;

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
