using System.Collections.Generic;
using System.Text;

namespace Model {

    /// <summary>
    /// Результирующая стопка карт.
    /// </summary>
    public abstract class Foundation : CardList {


        protected Foundation() : base() {
        }

        public void AddCard(Card card) {
            if (IsCorrectMove(card)) {
                Cards.Add(card);
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
