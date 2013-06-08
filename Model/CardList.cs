using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {

    /// <summary>
    /// Структура данных для хранения карт.
    /// </summary>
    public class CardList {

        protected List<Card> Cards {
            get;
            set;
        }

        protected CardList() {
            Cards = new List<Card>();
        }

        public virtual void AddCard(Card card) {
            Cards.Add(card);
        }

        public virtual void AddCards(IEnumerable<Card> cards) {
            Cards.AddRange(cards);
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
