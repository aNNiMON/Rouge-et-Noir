using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Model;

namespace View {

    /// <summary>
    /// Вид таблицы.
    /// </summary>
    public partial class TableauView : UserControl {

        public Tableau Tableau {
            get {
                return _tableau;
            }
        }
        private Tableau _tableau;

        /// <summary>
        /// На сколько пискелей опускать следующую карту в таблице.
        /// </summary>
        private readonly int _cardSpace;

        private readonly List<CardView> _cardViews;

        public TableauView() {
            InitializeComponent();
            _cardViews = new List<CardView>();
            _cardSpace = 15;
        }

        /// <summary>
        /// Получить размер занимаемой на экране области.
        /// </summary>
        public Rect Bounds {
            get {
                var list = new List<Visual>();
                list.Add(RootView.Children[0]);
                list.AddRange(_cardViews);
                return Util.GetBoundingRect(list);
            }
        }

        public void SetTableau(Tableau tableau) {
            this._tableau = tableau;

            List<Card> cards = tableau.GetList();
            RootView.Children.Add(Util.CreateCardPlace('K'));
            for (int i = 0; i < cards.Count; i++) {
                var card = cards[i];
                var cardView = new CardView();
                AddCard(cardView, card, i);
            }
        }

        /// <summary>
        /// Обновить вид таблицы.
        /// </summary>
        public void RefreshView() {
            List<Card> cards = _tableau.GetList();
            for (int i = 0; i < cards.Count; i++) {
                var card = cards[i];

                CardView cardView;
                if (i < _cardViews.Count) {
                    cardView = _cardViews[i];
                    cardView.Card = card;
                } else {
                    cardView = new CardView();
                    AddCard(cardView, card, i);
                    cardView.Animate(CardView.AnimFadeIn);
                }
            }
            // Удаляем лишнее
            for (int i = _cardViews.Count - 1; i >= cards.Count; i--) {
                CardView v = _cardViews[i];
                RootView.Children.Remove(v);
                _cardViews.Remove(v);
            }
        }

        private void AddCard(CardView cardView, Card card, int index) {
            cardView.PreviewMouseLeftButtonDown += cardView_PreviewMouseLeftButtonDown;

            cardView.Card = card;
            Canvas.SetTop(cardView, _cardSpace * index);
            Panel.SetZIndex(cardView, 1 + index);
            RootView.Children.Add(cardView);
            _cardViews.Add(cardView);
        }


        private void cardView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            // При нажатии левой кнопки мыши проверяем, можем ли мы переместить карты
            // от выбранной до нижней. Если можем - создаём из них DraggableCards и перемещаем их.
            var view = (CardView) sender;
            List<Card> draggable = Tableau.GetDraggableTopCards();
            for (int i = 0; i < draggable.Count; i++) {
                var dragCard = draggable[i];
                if (view.Card.Equals(dragCard)) {
                    // Собираем карты в новый компонент.
                    var draggableCards = new DraggableCards();
                    draggableCards.Cards = draggable.GetRange(i, draggable.Count - i);
                    // Карты в таблице скрываем.
                    foreach (var cardView in _cardViews) {
                        foreach (var card in draggableCards.Cards) {
                            if (cardView.Card.Equals(card)) {
                                cardView.Visibility = Visibility.Hidden;
                            }
                        }
                    }
                    // Добавляем новый компонент на форму.
                    Canvas.SetTop(draggableCards, Canvas.GetTop(view));
                    Canvas.SetLeft(draggableCards, Canvas.GetLeft(view));
                    Panel.SetZIndex(draggableCards, 200);
                    RootView.Children.Add(draggableCards);
                    DragHelper.Drag(draggableCards, OnDragCompleted, e.GetPosition(null));
                    return;
                }
            }
        }

        /// <summary>
        /// Событие завершения перемещения карт.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDragCompleted(object sender, MouseButtonEventArgs e) {
            var draggableCards = (DraggableCards) sender;
            // Показываем карты в таблице.
            foreach (var cardView in _cardViews) {
                foreach (var card in draggableCards.Cards) {
                    if (cardView.Card.Equals(card)) {
                        cardView.Visibility = Visibility.Visible;
                    }
                }
            }
            GameView.Instance.DragCompleted(this, draggableCards);
            RootView.Children.Remove(draggableCards);
        }
    }
}
