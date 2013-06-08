using System.Collections.Generic;
using System.Text;

namespace Model {

    /// <summary>
    /// Результирующая стопка карт.
    /// </summary>
    public abstract class Foundation {

        protected List<Card> Cards {
            get;
            private set;
        }

        protected Foundation() {
            Cards = new List<Card>();
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

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            foreach (var card in Cards) {
                sb.AppendLine(card.ToString());
            }
            return sb.ToString();
        }
    }
}
