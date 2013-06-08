using System.Collections.Generic;
using System.Text;

namespace Model {

    /// <summary>
    /// Результирующая стопка карт.
    /// </summary>
    public abstract class Foundation : CardList {


        protected Foundation() : base() {
        }

        public override void AddCard(Card card) {
            if (IsCorrectMove(card)) {
                base.AddCard(card);
            }
        }

        public override void AddCards(IEnumerable<Card> cards) {
            foreach (var card in cards) {
                AddCard(card);
            }
        }

        /// <summary>
        /// Проверка корректности хода.
        /// </summary>
        /// <param name="card">добавляемая в стопку карта</param>
        /// <returns>true - ход воможен, false - некорректный ход</returns>
        protected abstract bool IsCorrectMove(Card card);

    }
}
