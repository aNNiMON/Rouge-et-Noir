using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
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

        private List<CardView> cardViews;

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
                CardView cardView = new CardView();
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
            DragHelper.Drag(cardView, OnDragCompleted);
            cardView.Card = card;
            Canvas.SetTop(cardView, cardSpace * index);
            Canvas.SetZIndex(cardView, 1 + index);
            rootView.Children.Add(cardView);
            cardViews.Add(cardView);
        }

        void OnDragCompleted(object sender, MouseButtonEventArgs e) {
            CardView view = (CardView) sender;
            GameView.Instance.DragCompleted(this, view);
        }
    }
}
