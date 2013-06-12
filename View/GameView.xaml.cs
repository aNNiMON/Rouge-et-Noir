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

        private readonly GameTable table;

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

        #region Игровые функции
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
            CheckAutoMovesToRightFoundation();
            stockView.RefreshView();
            for (int i = 0; i < GameTable.TABLEAUS; i++) {
                tableauViews[i].RefreshView();
            }
        }

        /// <summary>
        /// Событие окончания перемещения карт.
        /// Просматриваются новые позиции карты и определяется, куда их переместить.
        /// </summary>
        /// <param name="tableauView">таблица, из которой были перемещены карты</param>
        /// <param name="draggableCards">перемещаемые карты</param>
        public void DragCompleted(TableauView tableauView, DraggableCards draggableCards) {
            CardView bottomCardView = draggableCards.BottomCardView;
            Rect cardRect = GetCardRect(bottomCardView);
            // Просматриваем перемещение по стопкам.
            for (int i = 0; i < GameTable.FOUNDATIONS * 2; i++) {
                FoundationView view = foundationViews[i];

                Rect rect = view.Bounds;
                if (cardRect.IntersectsWith(rect)) {
                    // Проверка правых стопок.
                    if (i >= GameTable.FOUNDATIONS && draggableCards.Cards.Count != 13) {
                        CancelMove(draggableCards);
                        return;
                    }
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
                            table.MoveCards(view.Tableau.GetDraggableTopCards(), view.Tableau, fn);
                            break;
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
            // TODO: Game complete window
        }
        #endregion

        #region Вспомогательные функции
        /// <summary>
        /// Отмена операции перемещения и возвращение карт на место.
        /// </summary>
        /// <param name="uielement"></param>
        private void CancelMove(UIElement uielement) {
            uielement.RenderTransform = null;
        }

        private Rect GetCardRect(CardView cardView) {
            Rect cardRect = Util.GetBoundingRect(cardView);
            var cardPoint = new Point {
                X = cardRect.Left + cardRect.Width / 2,
                Y = cardRect.Top + cardRect.Height / 3
            };
            return new Rect(cardPoint, new Size(1, 1));
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
        /// Обновление экрана.
        /// </summary>
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
        #endregion

        #region Обработчики меню
        private void NewGame_Click(object sender, RoutedEventArgs e) {
            NewGame();
            RefreshView();
        }

        private void Exit_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void NewGame_Executed(object sender, ExecutedRoutedEventArgs e) {
            NewGame();
            RefreshView();
        }

        private void Undo_Executed(object sender, ExecutedRoutedEventArgs e) {
            table.Undo();
            RefreshView();
        }

        private void Redo_Executed(object sender, ExecutedRoutedEventArgs e) {
            table.Redo();
            RefreshView();
        }

        private void Statistics_Executed(object sender, ExecutedRoutedEventArgs e) {

        }

        private void Rules_Executed(object sender, ExecutedRoutedEventArgs e) {
            
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }
        #endregion

    }
}
