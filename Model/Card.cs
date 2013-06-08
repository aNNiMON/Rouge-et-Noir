using System;
using Model.Enums;

namespace Model {

    /// <summary>
    /// Карта.
    /// </summary>
    public class Card {
        
        /// <summary>
        /// Масть.
        /// </summary>
        public CardSuit Suit {
            get { return suit; }
            set { suit = value; }
        }
        private CardSuit suit;

        /// <summary>
        /// Значение.
        /// </summary>
        public CardValue Value {
            get { return cardValue; }
            set { cardValue = value; }
        }
        private CardValue cardValue;

        public Card(CardSuit suit, CardValue value) {
            Suit = suit;
            Value = value;
        }


        private string GetCardValueAsString() {
            if (cardValue == CardValue.Ace) return "A";
            else if (cardValue == CardValue.Jack) return "J";
            else if (cardValue == CardValue.Queen) return "Q";
            else if (cardValue == CardValue.King) return "K";

            int value = 1 + (int) cardValue;
            return Convert.ToString(value);
        }

        private char GetSuitChar() {
            return suit.ToString()[0];
        }

        public override string ToString() {
            return GetSuitChar() + GetCardValueAsString();
        }
    }
}
