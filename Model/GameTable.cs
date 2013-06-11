using System;
using System.Collections.Generic;
using System.Linq;
using Model.Enums;

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
        public void HandOutFromStock(bool saveToHistory = true) {
            var cards = new List<Card>();
            Util.Move<Card>(stock.GetList(), cards, TABLEAUS);
            if (saveToHistory) {
                MovesManager.HandOut(cards);
            }
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

        public void Undo() {
            Move move = MovesManager.Undo();
            switch (move.Type) {
                case MoveType.TO_FOUNDATION:
                    move.ToFoundation.GetList().Remove(move.Card);
                    if (move.FaceUp) move.FromTableau.FaceDownTopCard();
                    move.FromTableau.AddCardBySystem(move.Card);
                    break;
                case MoveType.TO_TABLEAU:
                    move.ToTableau.GetList().Remove(move.Card);
                    if (move.FaceUp) move.FromTableau.FaceDownTopCard();
                    move.FromTableau.AddCardBySystem(move.Card);
                    break;
                case MoveType.FROM_STOCK:
                    foreach (var card in move.Cards) {
                        for (int i = 0; i < TABLEAUS; i++) {
                            tableau[i].GetList().Remove(card);
                        }
                        card.SetFaceDown();
                    }
                    Util.Move<Card>(move.Cards, stock.GetList(), move.Cards.Count);
                    break;
            }
        }

        public void Redo() {
            Move move = MovesManager.Redo();
            switch (move.Type) {
                case MoveType.TO_FOUNDATION:
                    MoveCard(move.Card, move.FromTableau, move.ToFoundation, false);
                    //if (move.FaceUp) move.FromTableau.FaceUpTopCard();
                    break;
                case MoveType.TO_TABLEAU:
                    MoveCard(move.Card, move.FromTableau, move.ToTableau, false);
                    //if (move.FaceUp) move.FromTableau.FaceUpTopCard();
                    break;
                case MoveType.FROM_STOCK:
                    HandOutFromStock(false);
                    break;
            }
        }

        public void MoveCard(Card card, Tableau from, Foundation to, bool saveToHistory = true) {
            var cards = new List<Card>();
            Util.Move<Card>(from.GetList(), cards, 1);

            if (saveToHistory) {
                bool faceUp = (from.GetTopCard() == null) ? false : from.GetTopCard().IsFaceDown;
                MovesManager.Move(card, from, to, faceUp);
            }

            from.FaceUpTopCard();
            to.AddCards(cards);
        }

        public void MoveCard(Card card, Tableau from, Tableau to, bool saveToHistory = true) {
            var cards = new List<Card>();
            Util.Move<Card>(from.GetList(), cards, 1);

            if (saveToHistory) {
                bool faceUp = (from.GetTopCard() == null) ? false : from.GetTopCard().IsFaceDown;
                MovesManager.Move(card, from, to, faceUp);
            }

            from.FaceUpTopCard();
            to.AddCards(cards);
        }
    }
}
