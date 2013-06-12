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

        public bool IsFaceDown {
            get;
            set;
        }

        public Card(CardSuit suit, CardValue value)
            : this(suit, value, true) {  }

        public Card(CardSuit suit, CardValue value, bool isFaceDown) {
            Suit = suit;
            Value = value;
            IsFaceDown = isFaceDown;
        }

        public void SetFaceDown() {
            IsFaceDown = true;
        }

        public void SetFaceUp() {
            IsFaceDown = false;
        }
        
        public bool IsRedSuit() {
            return (suit == CardSuit.Diamonds) || (suit == CardSuit.Hearts);
        }

        /// <summary>
        /// Получить путь к картинке-ресурсу
        /// в зависимости от значения карты.
        /// </summary>
        /// <returns></returns>
        public string GetImageResourcePath() {
            if (IsFaceDown) return "back";
            return ToString().ToLower();
        }
        
        private string GetCardValueAsString() {
            if (cardValue == CardValue.Ace) return "A";
            if (cardValue == CardValue.Jack) return "J";
            if (cardValue == CardValue.Queen) return "Q";
            if (cardValue == CardValue.King) return "K";

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
