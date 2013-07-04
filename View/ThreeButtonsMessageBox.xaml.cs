using System.Windows;
using System.Windows.Input;

namespace View {

    /// <summary>
    /// Трёхкнопочный MessageBox с возможностью изменять текст кнопки.
    /// </summary>
    public partial class ThreeButtonsMessageBox : Window {

        public ThreeButtonsMessageBox() {
            InitializeComponent();
            Result = MessageBoxResult.Cancel;
        }

        private MessageBoxResult Result;

        public static MessageBoxResult Show(string caption, string first, string second, string third) {
            return Show(caption, "", first, second, third);
        }

        public static MessageBoxResult Show(string caption, string title, string first, string second, string third) {
            var messageBox = new ThreeButtonsMessageBox();
            messageBox.ShowDialog(caption, title, first, second, third);
            return messageBox.Result;
        }

        private void ShowDialog(string caption, string title, string first, string second, string third) {
            Caption.Text = caption;
            Title = title;
            FirstButton.Content = first;
            SecondButton.Content = second;
            DefaultButton.Content = third;
            ShowDialog();
        }

        private void First_Button_Executed(object sender, ExecutedRoutedEventArgs e) {
            SetResultAndExit(MessageBoxResult.Yes);
        }

        private void Second_Button_Executed(object sender, ExecutedRoutedEventArgs e) {
            SetResultAndExit(MessageBoxResult.No);
        }

        private void Default_Button_Executed(object sender, ExecutedRoutedEventArgs e) {
            SetResultAndExit(MessageBoxResult.Cancel);
        }

        private void SetResultAndExit(MessageBoxResult result) {
            Result = result;
            Close();
        }
    }
}

