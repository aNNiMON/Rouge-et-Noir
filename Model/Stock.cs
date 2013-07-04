namespace Model {

    /// <summary>
    /// Запас.
    /// </summary>
    public class Stock : CardList {

        /// <summary>
        /// Взять верхнюю карту.
        /// </summary>
        /// <returns></returns>
        public Card GetTopCard() {
            if (Cards.Count == 0) return null;
            return Cards[Cards.Count - 1];
        }
    }
}
