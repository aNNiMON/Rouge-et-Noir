using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {

    /// <summary>
    /// Колода из 104 карт.
    /// </summary>
    public class Deck104 : Deck {

        public Deck104() : base() {
            // Добавляем еще 52 карты.
            Generate();
            Shuffle();
        }
    }
}
