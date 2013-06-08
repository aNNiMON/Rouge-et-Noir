using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Model;

namespace Rouge_et_Noir {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            GameTable table = new GameTable();
            table.NewGame();

            /*Deck deck = new Deck104();

            LeftFoundation left = new LeftFoundation();
            for (int i = 0; i <= 1; i++) {
                foreach (Card card in deck) {
                    left.AddCard(card);
                }
                System.Diagnostics.Debug.Print(left.ToString());
                System.Diagnostics.Debug.Print("-------------------\n\n\n");
            }*/

        }
    }
}
