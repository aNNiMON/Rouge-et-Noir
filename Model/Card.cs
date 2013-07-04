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

        /// <summary>
        /// Карта положена картинкой вниз.
        /// </summary>
        public bool IsFaceDown {
            get;
            private set;
        }

        public Card(CardSuit suit, CardValue value)
            : this(suit, value, true) {  }

        private Card(CardSuit suit, CardValue value, bool isFaceDown) {
            Suit = suit;
            Value = value;
            IsFaceDown = isFaceDown;
        }

        /// <summary>
        /// Перевернуть карту лицевой стороной.
        /// </summary>
        public void SetFaceDown() {
            IsFaceDown = true;
        }

        /// <summary>
        /// Перевернуть карту крапом вверх.
        /// </summary>
        public void SetFaceUp() {
            IsFaceDown = false;
        }
        
        /// <summary>
        /// Карта красной масти (бубна или черва).
        /// </summary>
        /// <returns></returns>
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
