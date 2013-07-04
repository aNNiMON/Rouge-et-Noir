using System.Collections.Generic;
using System.Linq;
using Model.Enums;

namespace Model {

    /// <summary>
    /// Класс таблиц с исходными картами.
    /// </summary>
    public class Tableau : CardList {

        /// <summary>
        /// Добавление карты пользователем.
        /// </summary>
        /// <param name="card"></param>
        public override void AddCard(Card card) {
            if (IsCorrectMove(card)) {
                base.AddCard(card);
            }
        }

        /// <summary>
        /// Добавление карт пользователем.
        /// </summary>
        /// <param name="cards"></param>
        public override void AddCards(IEnumerable<Card> cards) {
            if (IsCorrectMove(cards.First())) {
                base.AddCards(cards);
            }
        }

        /// <summary>
        /// Добавление карт системой (из запаса).
        /// </summary>
        /// <param name="cards"></param>
        public void AddCardsBySystem(IEnumerable<Card> cards) {
            base.AddCards(cards);
        }

        /// <summary>
        /// Проверка корректности хода.
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public bool IsCorrectMove(Card card) {
            Card top = GetTopCard();
            return IsCorrectMove(card, top);
        }

        private bool IsCorrectMove(Card card, Card top) {
            // На пустую область можно ложить только короля.
            if (top == null) {
                return (card.Value == CardValue.King);
            }
            if (card.IsFaceDown || top.IsFaceDown)
                return false;

            bool isAlternatingColor = (top.IsRedSuit() ^ card.IsRedSuit());
            bool isNextCard = (card.Value - top.Value) == -1;

            return (isAlternatingColor && isNextCard);
        }

        /// <summary>
        /// Перевернуть верхнюю карту лицевой стороной.
        /// </summary>
        public void FaceUpTopCard() {
            Card top = GetTopCard();
            if (top != null) top.SetFaceUp();
        }

        /// <summary>
        /// Перевернуть верхнюю карту крапом вверх.
        /// </summary>
        public void FaceDownTopCard() {
            Card top = GetTopCard();
            if (top != null) top.SetFaceDown();
        }

        /// <summary>
        /// Взять верхнюю карту.
        /// </summary>
        /// <returns></returns>
        public Card GetTopCard() {
            if (Cards.Count == 0) return null;
            return Cards[Cards.Count - 1];
        }

        /// <summary>
        /// Проверить наличие последовательности карт от короля до туза
        /// для помещения в правую результирующую стопку.
        /// </summary>
        /// <returns></returns>
        public bool CheckFillKingToAce() {
            List<Card> cards = GetDraggableTopCards();
            return ( (cards != null) && (cards.Count == 13) );
        }

        /// <summary>
        /// Получить список карт, доступных для перемещения.
        /// </summary>
        /// <returns></returns>
        public List<Card> GetDraggableTopCards() {
            if (Cards.Count == 0) return null;

            var cards = new List<Card>();
            Card top = Cards[Cards.Count - 1];
            cards.Add(top);
            if (Cards.Count == 1) return cards;

            for (int i = Cards.Count - 2; i >= 0; i--) {
                Card beforeTop = Cards[i];
                if (IsCorrectMove(top, beforeTop)) {
                    cards.Add(beforeTop);
                } else break;
                top = beforeTop;
            }

            var rev = cards.Reverse<Card>();

            return rev.ToList();
        }
    }
}
