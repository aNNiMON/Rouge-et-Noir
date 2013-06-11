namespace Model {

    /// <summary>
    /// Колода из 104 карт.
    /// </summary>
    public class Deck104 : Deck {

        public Deck104() : base() {
            // Добавляем еще 52 карты.
            Generate();
            for (int i = 0; i < 10; i++)
                Shuffle();
        }
    }
}
