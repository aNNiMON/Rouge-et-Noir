using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Model;

namespace View {

    /// <summary>
    /// Interaction logic for TableauView.xaml
    /// </summary>
    public partial class TableauView : UserControl {

        private Tableau tableau;
        public Tableau Tableau {
            get {
                return tableau;
            }
        }

        private int cardSpace;

        private readonly List<CardView> cardViews;

        public TableauView() {
            InitializeComponent();
            cardViews = new List<CardView>();
            cardSpace = 15;
        }

        public Rect Bounds {
            get {
                var list = new List<Visual>();
                list.Add(rootView.Children[0]);
                list.AddRange(cardViews);
                return Util.GetBoundingRect(list);
            }
        }

        public void SetTableau(Tableau tableau) {
            this.tableau = tableau;

            List<Card> cards = tableau.GetList();
            rootView.Children.Add(Util.CreateCardPlace('K'));
            for (int i = 0; i < cards.Count; i++) {
                var card = cards[i];
                var cardView = new CardView();
                AddCard(cardView, card, i);
            }
        }

        public void SetCardSpace(int cardSpace) {
            this.cardSpace = cardSpace;
        }

        public void RefreshView() {
            List<Card> cards = tableau.GetList();
            for (int i = 0; i < cards.Count; i++) {
                var card = cards[i];

                CardView cardView;
                if (i < cardViews.Count) {
                    cardView = cardViews[i];
                    cardView.Card = card;
                } else {
                    cardView = new CardView();
                    AddCard(cardView, card, i);
                    cardView.Animate(CardView.ANIM_FADE_IN);
                }
            }
            // Удаляем лишнее
            for (int i = cardViews.Count - 1; i >= cards.Count; i--) {
                CardView v = cardViews[i];
                rootView.Children.Remove(v);
                cardViews.Remove(v);
            }
        }

        private void AddCard(CardView cardView, Card card, int index) {
            cardView.PreviewMouseLeftButtonDown += cardView_PreviewMouseLeftButtonDown;

            cardView.Card = card;
            Canvas.SetTop(cardView, cardSpace * index);
            Panel.SetZIndex(cardView, 1 + index);
            rootView.Children.Add(cardView);
            cardViews.Add(cardView);
        }


        private void cardView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            var view = (CardView) sender;
            List<Card> draggable = Tableau.GetDraggableTopCards();
            for (int i = 0; i < draggable.Count; i++) {
                var card = draggable[i];
                if (view.Card.Equals(card)) {
                    // Собираем карты в новый компонент.
                    var draggableCards = new DraggableCards();
                    draggableCards.Cards = draggable.GetRange(i, draggable.Count - i);
                    // Карты в таблице скрываем.
                    foreach (var cardView in cardViews) {
                        foreach (var _card in draggableCards.Cards) {
                            if (cardView.Card.Equals(_card)) {
                                cardView.Visibility = Visibility.Hidden;
                            }
                        }
                    }
                    // Добавляем новый компонент на форму.
                    Canvas.SetTop(draggableCards, Canvas.GetTop(view));
                    Canvas.SetLeft(draggableCards, Canvas.GetLeft(view));
                    Panel.SetZIndex(draggableCards, 200);
                    rootView.Children.Add(draggableCards);
                    DragHelper.Drag(draggableCards, OnDragCompleted, e.GetPosition(null));
                    return;
                }
            }
        }

        private void OnDragCompleted(object sender, MouseButtonEventArgs e) {
            var draggableCards = (DraggableCards) sender;
            // Показываем карты в таблице.
            foreach (var cardView in cardViews) {
                foreach (var _card in draggableCards.Cards) {
                    if (cardView.Card.Equals(_card)) {
                        cardView.Visibility = Visibility.Visible;
                    }
                }
            }
            GameView.Instance.DragCompleted(this, draggableCards);
            rootView.Children.Remove(draggableCards);
        }
    }
}
