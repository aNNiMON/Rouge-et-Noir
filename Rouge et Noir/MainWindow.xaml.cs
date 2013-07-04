using System.Windows;

namespace Rouge_et_Noir {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
            GameView.Focus();
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e) {
            // Если пользователь не подтвердил закрытие окна, то отменяем выход.
            if (GameView == null) return;

            if (!GameView.OnCloseView()) {
                e.Cancel = true;
            }
        }
    }
}
