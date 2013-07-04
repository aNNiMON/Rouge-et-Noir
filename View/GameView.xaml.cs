using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Model;

namespace View {

    /// <summary>
    /// Главный компонент - вид игры.
    /// Взаимодействует с моделью - классом GameTable.
    /// </summary>
    public partial class GameView : UserControl {

        public static GameView Instance {
            get { return _instance ?? (_instance = new GameView()); }
        }
        private static GameView _instance;

        private readonly GameTable _table;


        private StockView StockView { get; set; }
        private FoundationView[] _foundationViews;
        private TableauView[] _tableauViews;

        private DispatcherTimer _timer;

        public GameView() {
            InitializeComponent();

            _instance = this;

            _table = new GameTable();
            _table.NewGame();
            SetStock();
            SetFoundations();
            SetTableau();
            SetTimer();
        }

        /// <summary>
        /// Получить корневой вид.
        /// </summary>
        /// <returns></returns>
        public UIElement GetRootView() {
            return RootView;
        }

        #region Игровые функции
        /// <summary>
        /// Начать новую игру.
        /// </summary>
        private void NewGame() {
            _table.NewGame();
            RefreshView();
        }

        /// <summary>
        /// Перезапуск игры.
        /// </summary>
        private void RestartGame() {
            _table.RestartGame();
            RefreshView();
        }

        /// <summary>
        /// Раздача карт из запаса.
        /// </summary>
        public void HandOutFromStock() {
            _table.HandOutFromStock();
            CheckAutoMovesToRightFoundation();
            StockView.RefreshView();
            for (int i = 0; i < GameTable.Tableaus; i++) {
                _tableauViews[i].RefreshView();
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
            for (int i = 0; i < GameTable.Foundations * 2; i++) {
                FoundationView view = _foundationViews[i];

                Rect rect = view.Bounds;
                if (cardRect.IntersectsWith(rect)) {
                    // Проверка правых стопок.
                    if (i >= GameTable.Foundations && draggableCards.Cards.Count != 13) {
                        CancelMove(draggableCards);
                        return;
                    }
                    if (!view.Foundation.IsCorrectMove(bottomCardView.Card)) {
                        CancelMove(draggableCards);
                        return;
                    }
                    // Добавляем карты в стопку.
                    _table.MoveCards(draggableCards.Cards, tableauView.Tableau, view.Foundation);
                    tableauView.RefreshView();
                    view.RefreshView();
                    CheckGameOver();
                    return;
                }
            }
            // Просматриваем перемещение по таблицам.
            for (int i = 0; i < GameTable.Tableaus; i++) {
                TableauView view = _tableauViews[i];
                if (view.Equals(tableauView))
                    continue;

                Rect rect = view.Bounds;
                if (cardRect.IntersectsWith(rect)) {
                    if (!view.Tableau.IsCorrectMove(bottomCardView.Card)) {
                        CancelMove(draggableCards);
                        return;
                    }
                    // Переносим карты в другую таблицу.
                    _table.MoveCards(draggableCards.Cards, tableauView.Tableau, view.Tableau);
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
            for (int i = 0; i < GameTable.Tableaus; i++) {
                TableauView view = _tableauViews[i];
                if (!view.Tableau.CheckFillKingToAce()) continue;
                // Найдена последовательность от короля до туза.
                // Ищем, куда её переместить.
                for (int j = 0; j < GameTable.Foundations; j++) {
                    Foundation fn = _table.GetFoundation(j, false);
                    if (fn.GetTopCard() == null) {
                        _table.MoveCards(view.Tableau.GetDraggableTopCards(), view.Tableau, fn);
                        break;
                    }
                }
                RefreshView();
                CheckGameOver();
            }
        }

        /// <summary>
        /// Проверка окончания игры.
        /// </summary>
        private void CheckGameOver() {
            for (int j = 0; j < GameTable.Foundations * 2; j++) {
                if (!_foundationViews[j].Foundation.IsFinished())
                    return;
            }
            // Игра окончена - выводим финальную заставку.
            // Откатываем ходы в самое начало (не сбрасывая счётчики)
            _table.RestartGame(false);
            // Показываем в фоне историю действий игрока.
            var dt = new DispatcherTimer();
            dt.Tick += (s, ee) => {
                _table.Redo();
                RefreshView();
            };
            dt.Interval = new TimeSpan(500 * 10000);
            dt.Start();
            // Запрашиваем имя игрока.
            EnterNameComponent.Show((s, ee) => {
                _table.EndGame(true);
                // Показываем 
                ScoreManager.Load();
                int place = ScoreManager.GetPlace();
                string text = (place > 0) ? "Вы на " + place + " месте в рейтинге" : "";
                HiscoreComponent.Show(ScoreManager.HiScores, text);
                dt.Stop();
                NewGame();
            });
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

        /// <summary>
        /// Получить область взаиодействия с другими картами.
        /// Чтобы карта не налегала на рядом стоящие стопки,
        /// область взята в центре вверху и равна одному пикселю.
        /// </summary>
        /// <param name="cardView"></param>
        /// <returns></returns>
        private Rect GetCardRect(CardView cardView) {
            Rect cardRect = Util.GetBoundingRect(cardView);
            var cardPoint = new Point {
                X = cardRect.Left + cardRect.Width / 2,
                Y = cardRect.Top + cardRect.Height / 3
            };
            return new Rect(cardPoint, new Size(1, 1));
        }

        private void SetStock() {
            StockView = new StockView();
            StockView.SetStock(_table.GetStock());
            Grid.SetColumn(StockView, 0);
            Grid.SetRow(StockView, 1);
            RootView.Children.Add(StockView);
        }

        private void SetFoundations() {
            _foundationViews = new FoundationView[GameTable.Foundations * 2];
            for (int i = 0; i < GameTable.Foundations * 2; i++) {
                var foundationView = new FoundationView();

                int num = i % GameTable.Foundations;
                bool left = i < GameTable.Foundations;

                Grid.SetColumn(foundationView, 2 + i);
                Grid.SetRow(foundationView, 1);
                foundationView.SetFoundation(_table.GetFoundation(num, left), left);

                RootView.Children.Add(foundationView);
                _foundationViews[i] = foundationView;
            }
        }

        private void SetTableau() {
            _tableauViews = new TableauView[GameTable.Tableaus];
            for (int i = 0; i < GameTable.Tableaus; i++) {
                var tableauView = new TableauView();

                Grid.SetColumn(tableauView, i);
                Grid.SetRow(tableauView, 2);
                Panel.SetZIndex(tableauView, 0);
                tableauView.SetTableau(_table.GetTableau(i));

                RootView.Children.Add(tableauView);
                _tableauViews[i] = tableauView;
            }
        }

        /// <summary>
        /// Установить таймер, обновляющий статистику игры на панели инструментов.
        /// </summary>
        private void SetTimer() {
            _timer = new DispatcherTimer();
            _timer.Tick += (s, e) => {
                GameScoreLabel.Content = ScoreManager.Current.ScoreValue;
                GameTimeLabel.Content = string.Format("{0:mm\\:ss}", ScoreManager.GetGameTime());
            };
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();
        }

        /// <summary>
        /// Обновление экрана.
        /// </summary>
        private void RefreshView() {
            for (int i = 0; i < GameTable.Foundations * 2; i++) {
                _foundationViews[i].RefreshView();
            }
            for (int i = 0; i < GameTable.Tableaus; i++) {
                Panel.SetZIndex(_tableauViews[i], 0);
                _tableauViews[i].RefreshView();
            }
            StockView.RefreshView();
        }
        #endregion

        /// <summary>
        /// Подтверждение при закрытия окна.
        /// Если есть какие-либо результаты, то выводит запрос отправки
        /// результата с возможностью отмены закрытия окна. Иначе закрывает
        /// окно без подтверждения.
        /// </summary>
        /// <returns>true - разрешено завершить приложение</returns>
        public bool OnCloseView() {
            if (ScoreManager.Current.ScoreValue <= 0) return true;

            var result = ThreeButtonsMessageBox.Show("Отправить результат и выйти?",
                "Выйти из игры", "Отправить", "Выйти", "Отмена");

            if (result == MessageBoxResult.Cancel) {
                // Пользователь отменил выход из приложения.
                return false;
            }
            if (result == MessageBoxResult.No) {
                // Выход из приложения.
                _timer.Stop();
                return true;
            }
            // Отправляем результат, после чего выходим.
            EnterNameComponent.Show((s, e) => {
                ScoreManager.EndGame(false);
                _timer.Stop();
                // Чтобы вновь не появился запрос на выход, сбрасываем текущий результат.
                ScoreManager.Current.ScoreValue = 0;
                Window.GetWindow(this).Close();
            });
            // Выход будет производится по окончанию ввода имени.
            return false;
        }

        #region Обработчики меню
        private void Exit_Click(object sender, RoutedEventArgs e) {
            // Вызывается событие FormClosing главного окна, в котором
            // вызывается функция bool CloseView.
            Window.GetWindow(this).Close();
        }

        private void NewGame_Executed(object sender, ExecutedRoutedEventArgs eventArgs) {
            if (ScoreManager.Current.ScoreValue > 0) {
                var result = ThreeButtonsMessageBox.Show("Отправить результат и начать новую игру?",
                    "Новая игра", "Отправить", "Новая игра", "Отмена");

                if (result == MessageBoxResult.Cancel) {
                    return;
                }
                if (result == MessageBoxResult.Yes) {
                    // Отправка результата.
                    EnterNameComponent.Show((s, ee) => {
                        ScoreManager.EndGame(false);
                        ScoreManager.Load();
                        int place = ScoreManager.GetPlace();
                        string text = (place > 0) ? "Вы на " + place + " месте в рейтинге" : "";
                        HiscoreComponent.Show(ScoreManager.HiScores, text);
                        NewGame();
                    });
                    return;
                }
            }
            // Новая игра
            NewGame();
        }

        private void RestartGame_Executed(object sender, ExecutedRoutedEventArgs eventArgs) {
            if (ScoreManager.Current.ScoreValue > 0) {
                var result = ThreeButtonsMessageBox.Show("Отправить результат и перезапустить игру?",
                    "Перезапуск", "Отправить", "Заново", "Отмена");

                if (result == MessageBoxResult.Cancel) {
                    return;
                }
                if (result == MessageBoxResult.Yes) {
                    // Отправка результата.
                    EnterNameComponent.Show((s, ee) => {
                        ScoreManager.EndGame(false);
                        ScoreManager.Load();
                        int place = ScoreManager.GetPlace();
                        string text = (place > 0) ? "Вы на " + place + " месте в рейтинге" : "";
                        HiscoreComponent.Show(ScoreManager.HiScores, text);
                        RestartGame();
                    });
                    return;
                }
            }
            // Перезапуск игры
            RestartGame();
        }

        private void Undo_Executed(object sender, ExecutedRoutedEventArgs e) {
            _table.Undo();
            RefreshView();
        }

        private void Redo_Executed(object sender, ExecutedRoutedEventArgs e) {
            _table.Redo();
            RefreshView();
        }

        private void Statistics_Executed(object sender, ExecutedRoutedEventArgs e) {
            ScoreManager.Load();
            HiscoreComponent.Show(ScoreManager.HiScores);
        }

        private void Rules_Executed(object sender, ExecutedRoutedEventArgs e) {
            InfoRichTextBox.LoadRtf("rules-rus");
            InfoRichTextBox.IsReadOnly = true;

            InfoDialog.Visibility = Visibility.Visible;
        }

        private void About_Executed(object sender, ExecutedRoutedEventArgs e) {
            InfoRichTextBox.LoadRtf("about");
            InfoRichTextBox.IsReadOnly = true;

            InfoDialog.Visibility = Visibility.Visible;
        }

        private void CloseDialog_Executed(object sender, ExecutedRoutedEventArgs e) {
            InfoDialog.Visibility = Visibility.Hidden;
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }
        #endregion
    }
}
