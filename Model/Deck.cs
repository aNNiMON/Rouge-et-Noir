using System.Collections;
using System.Collections.Generic;
using System.Text;
using Model.Enums;

namespace Model {

    /// <summary>
    /// Колода из 52 карт.
    /// </summary>
    public class Deck : CardList, IEnumerable {

        public Deck() : base() {
            Generate();
        }

        protected void Generate() {
            foreach (CardSuit suit in CardSuit.GetValues(typeof(CardSuit))) {
                foreach (CardValue value in CardValue.GetValues(typeof(CardValue))) {
                    Cards.Add(new Card(suit, value));
                }
            }
        }

        protected void Shuffle() {
            Util.Shuffle(Cards);
        }


        public IEnumerator GetEnumerator() {
            return Cards.GetEnumerator();
        }
    }
}
