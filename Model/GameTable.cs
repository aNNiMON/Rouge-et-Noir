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

        public const int FOUNDATIONS = 4, TABLEAUS = 10;

        private Stock Stock;
        private LeftFoundation[] leftFoundation;
        private RightFoundation[] rightFoundation;
        private Tableau[] tableau;

        public GameTable() {
            Stock = new Stock();
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

            leftFoundation[2].AddCard(new Card(Enums.CardSuit.Clubs, Enums.CardValue.Ace));

            // Добавляем оставшиеся 67 карт в запас.
            Stock.AddCards(cards);
        }
    }
}
