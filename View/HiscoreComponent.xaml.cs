using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Model;

namespace View {

    /// <summary>
    /// Компонент, показывающий таблицу лучших результатов.
    /// </summary>
    public partial class HiscoreComponent : UserControl {

        public HiscoreComponent() {
            InitializeComponent();
        }

        /// <summary>
        /// Показать таблицу рекордов.
        /// </summary>
        /// <param name="scores">список лучших результатов</param>
        public void Show(List<Score> scores) {
            bool listEmpty = ( (scores == null) || (scores.Count == 0) );
            if (listEmpty) Show(null, "Результатов нет");
            else Show(scores, null);
        }

        /// <summary>
        /// Показать таблицу рекордов и комментарий.
        /// </summary>
        /// <param name="scores">список лучших результатов</param>
        /// <param name="text">комментарий</param>
        public void Show(List<Score> scores, string text) {
            // Если список пуст, то скрываем его.
            bool listEmpty = ( (scores == null) || (scores.Count == 0) );
            hiscores.Visibility = convert(!listEmpty);
            if (!listEmpty) {
                hiscores.ItemsSource = scores;
                int place = ScoreManager.GetPlace() - 1;
                if ( (0 <= place) && (place < scores.Count) ) {
                    hiscores.SelectedIndex = place;
                }
            }

            // Если комментария нет, скрываем текстовую метку.
            bool textEmpty = ( (text == null) || (text.Length == 0) );
            message.Visibility = convert(!textEmpty);
            if (!textEmpty) {
                message.Content = text;
            }
            
            this.Visibility = Visibility.Visible;
        }

        private void Close_Executed(object sender, ExecutedRoutedEventArgs e) {
            this.Visibility = Visibility.Hidden;
            message.Visibility = Visibility.Hidden;
        }

        private Visibility convert(bool vis) {
            return (vis ? Visibility.Visible : Visibility.Hidden);
        }
    }
}
