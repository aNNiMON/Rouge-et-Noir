using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Model;

namespace View {

    /// <summary>
    /// Компонент для ввода имени игрока.
    /// </summary>
    public partial class EnterNameComponent : UserControl {

        public delegate void EnterTextHandler(object sender, EventArgs e);
        private event EnterTextHandler OnTextEntered;

        public EnterNameComponent() {
            InitializeComponent();
        }

        /// <summary>
        /// Показать диалог ввода имени.
        /// </summary>
        public void Show(EnterTextHandler onTextEnterEvent) {
            this.OnTextEntered = onTextEnterEvent;
            NameTextBox.Text = ScoreManager.DefaultName;
            this.Visibility = Visibility.Visible;
        }

        private void Close_Executed(object sender, ExecutedRoutedEventArgs e) {
            ScoreManager.Current.Name = NameTextBox.Text;
            ScoreManager.DefaultName = NameTextBox.Text;
            this.Visibility = Visibility.Hidden;

            if (OnTextEntered != null) {
                OnTextEntered(this, e);
            }
        }
    }
}
