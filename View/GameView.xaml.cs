using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            SetStock();
            SetFoundations();
            SetTableau();
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

        public UIElement GetRootView() {
            return rootView;
        }

        public UIElementCollection GetRootViewElements() {
            return rootView.Children;
        }

        /// <summary>
        /// Начать новую игру.
        /// </summary>
        public void NewGame() {
            table.NewGame();
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
            Grid.SetRow(stockView, 1);
            rootView.Children.Add(stockView);
        }

        private void SetFoundations() {
            foundationViews = new FoundationView[GameTable.FOUNDATIONS * 2];
            for (int i = 0; i < GameTable.FOUNDATIONS * 2; i++) {
                var foundationView = new FoundationView();

                int num = i % GameTable.FOUNDATIONS;
                bool left = i < GameTable.FOUNDATIONS;

                Grid.SetColumn(foundationView, 2 + i);
                Grid.SetRow(foundationView, 1);
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
                Grid.SetRow(tableauView, 2);
                Panel.SetZIndex(tableauView, 0);
                tableauView.SetTableau(table.GetTableau(i));

                rootView.Children.Add(tableauView);
                tableauViews[i] = tableauView;
            }
        }

        /// <summary>
        /// Событие окончания перемещения карт.
        /// Просматриваются новые позиции карты и определяется, куда их переместить.
        /// </summary>
        /// <param name="tableauView"></param>
        /// <param name="draggableCards"></param>
        public void DragCompleted(TableauView tableauView, DraggableCards draggableCards) {
            CardView bottomCardView = draggableCards.BottomCardView;
            Rect cardRect = GetCardRect(bottomCardView);
            // Просматриваем перемещение по стопкам.
            for (int i = 0; i < GameTable.FOUNDATIONS; i++) {
                FoundationView view = foundationViews[i];

                Rect rect = view.Bounds;
                if (cardRect.IntersectsWith(rect)) {
                    if (!view.Foundation.IsCorrectMove(bottomCardView.Card)) {
                        CancelMove(draggableCards);
                        return;
                    }
                    // Добавляем карты в стопку.
                    table.MoveCards(draggableCards.Cards, tableauView.Tableau, view.Foundation);
                    tableauView.RefreshView();
                    view.RefreshView();
                    CheckGameOver();
                    return;
                }
            }
            // Просматриваем перемещение по таблицам.
            for (int i = 0; i < GameTable.TABLEAUS; i++) {
                TableauView view = tableauViews[i];
                if (view.Equals(tableauView))
                    continue;

                Rect rect = view.Bounds;
                if (cardRect.IntersectsWith(rect)) {
                    if (!view.Tableau.IsCorrectMove(bottomCardView.Card)) {
                        CancelMove(draggableCards);
                        return;
                    }
                    // Переносим карты в другую таблицу.
                    table.MoveCards(draggableCards.Cards, tableauView.Tableau, view.Tableau);
                    tableauView.RefreshView();
                    view.RefreshView();
                    CheckAutoMovesToRightFoundation();
                    return;
                }
            }

            CancelMove(draggableCards);
        }

        /// <summary>
        /// Проверка автоматических перемещений в правую стопку из таблиц.
        /// </summary>
        private void CheckAutoMovesToRightFoundation() {
            for (int i = 0; i < GameTable.TABLEAUS; i++) {
                TableauView view = tableauViews[i];
                if (view.Tableau.CheckFillKingToAce()) {
                    for (int j = 0; j < GameTable.FOUNDATIONS; j++) {
                        Foundation fn = table.GetFoundation(j, false);
                        if (fn.GetTopCard() == null) {
                            table.MoveCards(view.Tableau.GetDraggableTopCards(), view.Tableau, fn, false);
                        }
                    }
                    RefreshView();
                    CheckGameOver();
                }

            }
        }

        /// <summary>
        /// Проверка окончания игры.
        /// </summary>
        private void CheckGameOver() {
            for (int j = 0; j < GameTable.FOUNDATIONS * 2; j++) {
                if (!foundationViews[j].Foundation.IsFinished())
                    return;
            }
            MessageBox.Show("Игра окончена. Поздравляем!!!");
            ///TODO: Game complete window
        }

        /// <summary>
        /// Отмена операции перемещения и возвращение карт на место.
        /// </summary>
        /// <param name="uielement"></param>
        private void CancelMove(UIElement uielement) {
            uielement.RenderTransform = null;
        }

        private Rect GetCardRect(CardView cardView) {
            Rect cardRect = Util.GetBoundingRect(cardView);
            Point cardPoint = new Point {
                X = cardRect.Left + cardRect.Width / 2,
                Y = cardRect.Top + cardRect.Height / 3
            };
            return new Rect(cardPoint, new Size(1, 1));
        }

        private void RefreshView() {
            for (int i = 0; i < GameTable.FOUNDATIONS * 2; i++) {
                foundationViews[i].RefreshView();
            }
            for (int i = 0; i < GameTable.TABLEAUS; i++) {
                Panel.SetZIndex(tableauViews[i], 0);
                tableauViews[i].RefreshView();
            }
            stockView.RefreshView();
        }

        #region Обработчики меню
        private void NewGame_Click(object sender, RoutedEventArgs e) {
            NewGame();
            RefreshView();
        }

        private void Exit_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void Undo_Executed(object sender, ExecutedRoutedEventArgs e) {
            table.Undo();
            RefreshView();
        }

        private void Redo_Executed(object sender, ExecutedRoutedEventArgs e) {
            table.Redo();
            RefreshView();
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }
        #endregion

    }
}
