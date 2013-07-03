using System.Windows;
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

        public static readonly DependencyProperty CardProperty =
            DependencyProperty.Register("Card", typeof(Card), typeof(CardView));

        public Card Card {
            get {
                return (Card) GetValue(CardProperty);
            }
            set {
                SetValue(CardProperty, value);
            }
        }

        public CardView() {
            InitializeComponent();
            cardImage.DataContext = this;
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
