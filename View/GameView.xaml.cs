using System;
using System.Windows.Controls;
using Model;

namespace View {

    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : UserControl {
        
        private static GameView instance;
        public static GameView Instance {
            get {
                if (instance == null) {
                    instance = new GameView();
                }
                return instance;
            }
        }

        private GameTable table;
        /*public GameTable Table {
            get {
                return table;
            }
        }*/

        private StockView stockView;
        public StockView StockView {
            get {
                return stockView;
            }
        }

        private FoundationView[] foundationViews;
        private TableauView[] tableauViews;

        public GameView() {
            InitializeComponent();

            instance = this;

            table = new GameTable();
            NewGame();
            
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

        public TableauView GetTableauView(int num) {
            if (num >= GameTable.TABLEAUS) throw new IndexOutOfRangeException();
            return tableauViews[num];
        }

        public FoundationView GetFoundationView(int num, bool left) {
            if (num >= GameTable.FOUNDATIONS) throw new IndexOutOfRangeException();

            if (left) num += GameTable.FOUNDATIONS;
            return foundationViews[num];
        }

        /// <summary>
        /// Начать новую игру.
        /// </summary>
        public void NewGame() {
            table.NewGame();

            SetStock();
            SetFoundations();
            SetTableau();
        }

        /// <summary>
        /// Раздача карт из запаса.
        /// </summary>
        public void HandOutFromStock() {
            table.HandOutFromStock();
            stockView.RefreshView();
            for (int i = 0; i < GameTable.TABLEAUS; i++) {
                tableauViews[i].RefreshView();
            }
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
