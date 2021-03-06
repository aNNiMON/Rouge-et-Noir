﻿using Model.Enums;

namespace Model {

    /// <summary>
    /// Левая стопка, в которую кладутся по одной
    /// карте одинакового цвета от туза до короля.
    /// </summary>
    public class LeftFoundation : Foundation {

        /// <summary>
        /// Проверка корректности хода.
        /// </summary>
        /// <param name="card">первая помещаемая карта</param>
        /// <returns>true - ход корректен</returns>
        public override bool IsCorrectMove(Card card) {
            Card top = GetTopCard();
            if (top == null) {
                return (card.Value == CardValue.Ace);
            }

            // Карты одинакового цвета по возрастанию.
            bool isSameColor = !(top.IsRedSuit() ^ card.IsRedSuit());
            bool isNextCard = (card.Value - top.Value) == 1;

            return (isSameColor && isNextCard);
        }
    }
}
