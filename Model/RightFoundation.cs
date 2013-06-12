using Model.Enums;

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
            return (card.Value == CardValue.King);
        }
    }
}
