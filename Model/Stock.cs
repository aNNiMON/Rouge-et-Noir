namespace Model {

    /// <summary>
    /// Запас.
    /// </summary>
    public class Stock : CardList {

        public Card GetTopCard() {
            if (base.Cards.Count == 0) return null;
            return base.Cards[base.Cards.Count - 1];
        }
    }
}
