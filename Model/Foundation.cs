using System.Collections.Generic;

namespace Model {

    /// <summary>
    /// Результирующая стопка карт.
    /// </summary>
    public abstract class Foundation : CardList {


        protected Foundation() : base() {
        }

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

        public Card GetTopCard() {
            if (base.Cards.Count == 0) return null;
            return base.Cards[base.Cards.Count - 1];
        }

        /// <summary>
        /// Проверка корректности хода.
        /// </summary>
        /// <param name="card">добавляемая в стопку карта</param>
        /// <returns>true - ход воможен, false - некорректный ход</returns>
        public abstract bool IsCorrectMove(Card card);

    }
}
