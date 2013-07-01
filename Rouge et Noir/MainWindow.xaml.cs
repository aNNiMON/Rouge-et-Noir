using System.Windows;

namespace Rouge_et_Noir {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
            gameView.Focus();
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e) {
            // Если пользователь не подтвердил закрытие окна, то отменяем выход.
            if (gameView == null) return;

            if (!gameView.CloseView()) {
                e.Cancel = true;
            }
        }
    }
}
