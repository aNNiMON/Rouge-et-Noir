using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {

    /// <summary>
    /// Класс игрового окна.
    /// Содержит результирующие стопки, таблицы и запас.
    /// </summary>
    public class GameTable {

        /*private static GameTable instance;

        public static GameTable GetInstance() {
            if (instance == null) {
                instance = new GameTable();
            }
            return instance;
        }*/


        public const int FOUNDATIONS = 4, TABLEAUS = 10;

        private Stock stock;
        private LeftFoundation[] leftFoundation;
        private RightFoundation[] rightFoundation;
        private Tableau[] tableau;

        public GameTable() {
            stock = new Stock();
            leftFoundation = new LeftFoundation[FOUNDATIONS];
            rightFoundation = new RightFoundation[FOUNDATIONS];
            for (int i = 0; i < FOUNDATIONS; i++) {
                leftFoundation[i] = new LeftFoundation();
                rightFoundation[i] = new RightFoundation();
            }
            tableau = new Tableau[TABLEAUS];
            for (int i = 0; i < TABLEAUS; i++) {
                tableau[i] = new Tableau();
            }
        }

        public Tableau GetTableau(int num) {
            if (num >= TABLEAUS) throw new IndexOutOfRangeException();
            return tableau[num];
        }

        public Foundation GetFoundation(int num, bool left) {
            if (num >= FOUNDATIONS) throw new IndexOutOfRangeException();
            if (left) return leftFoundation[num];
            else return rightFoundation[num];
        }

        public Stock GetStock() {
            return stock;
        }

        /// <summary>
        /// Новая игра.
        /// </summary>
        public void NewGame() {
            Deck deck = new Deck104();
            List<Card> cards = deck.Cast<Card>().ToList();
            
            // Добавляем карты в таблицы.
            int addCardsCount = 8;
            for (int i = 0; i < TABLEAUS - 2; i++) {
                Util.Move<Card>(cards, tableau[i].GetList(), addCardsCount);
                addCardsCount--;
            }
            Util.Move<Card>(cards, tableau[TABLEAUS - 2].GetList(), 1);
            // Переворачиваем верхние карты.
            for (int i = 0; i < TABLEAUS - 1; i++) {
                tableau[i].GetTopCard().SetFaceUp();
            }

            // Добавляем оставшиеся 67 карт в запас.
            stock.AddCards(cards);
        }

        /// <summary>
        /// Раздача карт из запаса.
        /// </summary>
        public void HandOutFromStock() {
            var cards = new List<Card>();
            Util.Move<Card>(stock.GetList(), cards, TABLEAUS);
            // Раскрываем карты.
            foreach (var card in cards) {
                card.SetFaceUp();
            }
            // Переносим в таблицы.
            for (int i = 0; i < TABLEAUS; i++) {
                if (cards.Count == 0) return;
                Util.Move<Card>(cards, tableau[i].GetList(), 1);
            }
        }
    }
}
