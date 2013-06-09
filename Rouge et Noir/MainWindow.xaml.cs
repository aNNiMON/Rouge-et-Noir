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
using View;

namespace Rouge_et_Noir {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private GameTable table;

        private StockView stockView;
        private FoundationView[] foundationViews;
        private TableauView[] tableauViews;
        

        public MainWindow() {
            InitializeComponent();

            table = new GameTable();
            table.NewGame();

            SetStock();
            SetFoundations();
            SetTableau();
            /*for (int i = 0; i <= 7; i++) {
                Tableau tab = table.GetTableau(i);
                for (int j = tab.GetList().Count - 1; j >= 0; j--) {
                    var card = tab.GetList()[j];
                    if (table.GetFoundation(0, true).IsCorrectMove(card)) {
                        table.GetFoundation(0, true).AddCard(card);
                        tab.GetList().Remove(card);
                        tab.GetTopCard().SetFaceUp();
                    }
                }
                tableauViews[i].RefreshView();
                
                System.Diagnostics.Debug.Print(table.GetFoundation(0, true).ToString());
                System.Diagnostics.Debug.Print("-------------------\n\n\n");
            }
            foundationViews[0].RefreshView();*/
        }

        private void SetStock() {
            stockView = new StockView();
            stockView.SetStock(table.GetStock());
            Grid.SetColumn(stockView, 0);
            Grid.SetRow(stockView, 0);
            rootView.Children.Add(stockView);
        }

        private void SetFoundations() {
            foundationViews = new FoundationView[GameTable.FOUNDATIONS * 2];
            for (int i = 0; i < GameTable.FOUNDATIONS * 2; i++) {
                var foundationView = new FoundationView();

                int num = i % GameTable.FOUNDATIONS;
                bool left = i < GameTable.FOUNDATIONS;

                Grid.SetColumn(foundationView, 2 + i);
                Grid.SetRow(foundationView, 0);
                foundationView.SetFoundation(table.GetFoundation(num, left), left);

                rootView.Children.Add(foundationView);
                foundationViews[i] = foundationView;
            }
        }

        private void SetTableau() {
            tableauViews = new TableauView[GameTable.TABLEAUS];
            for (int i = 0; i < GameTable.TABLEAUS; i++) {
                var tableauView = new TableauView();

                Grid.SetColumn(tableauView, i);
                Grid.SetRow(tableauView, 1);
                tableauView.SetTableau(table.GetTableau(i));

                rootView.Children.Add(tableauView);
                tableauViews[i] = tableauView;
            }
        }
    }
}
