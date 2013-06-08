using System.Collections.Generic;
using System.Text;
using Model.Enums;

namespace Model {

    /// <summary>
    /// Колода из 52 карт.
    /// </summary>
    public class Deck {

        protected List<Card> Cards {
            get;
            private set;
        }

        public Deck() {
            Cards = new List<Card>();
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

        

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            foreach (var card in Cards) {
                sb.AppendLine(card.ToString());
            }
            return sb.ToString();
        }
    }
}
