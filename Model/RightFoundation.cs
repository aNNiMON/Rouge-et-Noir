using System.Collections.Generic;

namespace Model {

    /// <summary>
    /// Правая стопка, в которую кладутся собранные
    /// карте одной масти от короля до туза.
    /// </summary>
    public class RightFoundation : Foundation {

        public void AddCards(List<Card> cards) {
            foreach (var card in cards) {
                base.AddCard(card);
            }
        }

        public override bool IsCorrectMove(Card card) {
            return (card.Value == Enums.CardValue.King);
        }
    }
}
