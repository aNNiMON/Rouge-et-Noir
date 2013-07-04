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
            set { _suit = value; }
        }
        private CardSuit _suit;

        /// <summary>
        /// Значение.
        /// </summary>
        public CardValue Value {
            get { return _cardValue; }
            set { _cardValue = value; }
        }
        private CardValue _cardValue;

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
            return (_suit == CardSuit.Diamonds) || (_suit == CardSuit.Hearts);
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
            if (_cardValue == CardValue.Ace) return "A";
            if (_cardValue == CardValue.Jack) return "J";
            if (_cardValue == CardValue.Queen) return "Q";
            if (_cardValue == CardValue.King) return "K";

            int value = 1 + (int) _cardValue;
            return Convert.ToString(value);
        }

        private char GetSuitChar() {
            return _suit.ToString()[0];
        }

        public override string ToString() {
            return GetSuitChar() + GetCardValueAsString();
        }

    }
}
