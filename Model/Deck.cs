using System;
using Model.Enums;

namespace Model {

    /// <summary>
    /// Колода из 52 карт.
    /// </summary>
    public class Deck : CardList {

        public Deck() : base() {
            Generate();
        }

        /// <summary>
        /// Сгенерировать колоду карт.
        /// </summary>
        protected void Generate() {
            foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit))) {
                foreach (CardValue value in Enum.GetValues(typeof(CardValue))) {
                    Cards.Add(new Card(suit, value));
                }
            }
        }

        /// <summary>
        /// Перемешать карты в колоде.
        /// </summary>
        protected void Shuffle() {
            Cards.Shuffle();
        }
    }
}
