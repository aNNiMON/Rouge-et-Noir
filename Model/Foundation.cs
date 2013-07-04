using System.Collections.Generic;

namespace Model {

    /// <summary>
    /// Результирующая стопка карт.
    /// </summary>
    public abstract class Foundation : CardList {

        public override void AddCard(Card card) {
            if (IsCorrectMove(card)) {
                card.SetFaceUp();
                base.AddCard(card);
            }
        }

        public override void AddCards(IEnumerable<Card> cards) {
            foreach (var card in cards) {
                AddCard(card);
            }
        }

        public void AddCardsBySystem(IEnumerable<Card> cards) {
            base.AddCards(cards);
        }

        public Card GetTopCard() {
            if (Cards.Count == 0) return null;
            return Cards[Cards.Count - 1];
        }

        /// <summary>
        /// Проверка окончания заполненности стопки.
        /// Стопка считается заполненной по достижению 13 карт по одному из правил.
        /// </summary>
        /// <returns>true - стопка заполнена</returns>
        public bool IsFinished() {
            return (Cards.Count == 13);
        }

        /// <summary>
        /// Проверка корректности хода.
        /// </summary>
        /// <param name="card">добавляемая в стопку карта</param>
        /// <returns>true - ход возможен, false - некорректный ход</returns>
        public abstract bool IsCorrectMove(Card card);

    }
}
