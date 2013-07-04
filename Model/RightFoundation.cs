using System.Collections.Generic;
using Model.Enums;
using System.Linq;

namespace Model {

    /// <summary>
    /// Правая стопка, в которую кладутся собранные
    /// карте одной масти от короля до туза.
    /// </summary>
    public class RightFoundation : Foundation {

        /// <summary>
        /// Проверка корректности хода.
        /// </summary>
        /// <param name="card">первая помещаемая карта</param>
        /// <returns>true - ход корректен</returns>
        public override bool IsCorrectMove(Card card) {
            return (card.Value == CardValue.King) && (Cards.Count == 0);
        }

        public override void AddCards(IEnumerable<Card> cards) {
            // Обращаем карты, чтобы верхним был король.
            var reverse = cards.Reverse();
            AddCardsBySystem(reverse);
        }
    }
}
