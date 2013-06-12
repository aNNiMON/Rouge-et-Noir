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

        public const int FOUNDATIONS = 4, TABLEAUS = 10;

        private readonly Stock stock;
        private readonly LeftFoundation[] leftFoundation;
        private readonly RightFoundation[] rightFoundation;
        private readonly Tableau[] tableau;

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
            return rightFoundation[num];
        }

        public Stock GetStock() {
            return stock;
        }

        /// <summary>
        /// Новая игра.
        /// </summary>
        public void NewGame() {
            Clear();
            MovesManager.NewGame();
            ScoreManager.InitNewGame();
            Deck deck = new Deck104();
            List<Card> cards = deck.Cast<Card>().ToList();
            
            // Добавляем карты в таблицы.
            int addCardsCount = 8;
            for (int i = 0; i < TABLEAUS - 2; i++) {
                Util.Move(cards, tableau[i].GetList(), addCardsCount);
                addCardsCount--;
            }
            Util.Move(cards, tableau[TABLEAUS - 2].GetList(), 1);
            // Переворачиваем верхние карты.
            for (int i = 0; i < TABLEAUS - 1; i++) {
                tableau[i].GetTopCard().SetFaceUp();
            }

            // Добавляем оставшиеся 67 карт в запас.
            stock.AddCards(cards);
        }

        /// <summary>
        /// Завершение игры.
        /// </summary>
        /// <param name="name">имя игрока</param>
        /// <param name="isComplete">завершена ли игра или прервана</param>
        public void EndGame(string name, bool isComplete) {
            ScoreManager.EndGame(name, isComplete);
        }

        /// <summary>
        /// Раздача карт из запаса.
        /// </summary>
        public void HandOutFromStock(bool saveToHistory = true) {
            var cards = new List<Card>();
            Util.Move(stock.GetList(), cards, TABLEAUS);
            if (saveToHistory) {
                MovesManager.HandOut(new List<Card>(cards));
            }
            // Раскрываем карты.
            foreach (var card in cards) {
                card.SetFaceUp();
            }
            // Переносим в таблицы.
            for (int i = 0; i < TABLEAUS; i++) {
                if (cards.Count == 0) return;
                Util.Move(cards, tableau[i].GetList(), 1);
            }
        }

        public void Undo() {
            Move move = MovesManager.Undo();
            switch (move.Type) {
                case MoveType.TO_FOUNDATION:
                    ScoreManager.DecreaseScore(move.Cards.Count);
                    // Удаляем карты из таблицы.
                    foreach (var _card in move.Cards) {
                        move.ToFoundation.GetList().Remove(_card);
                    }
                    if (move.FaceUp) move.FromTableau.FaceDownTopCard();
                    move.FromTableau.AddCardsBySystem(move.Cards);
                    break;
                case MoveType.TO_TABLEAU:
                    // Удаляем карты из таблицы.
                    foreach (var _card in move.Cards) {
                        move.ToTableau.GetList().Remove(_card);
                    }
                    if (move.FaceUp) move.FromTableau.FaceDownTopCard();
                    move.FromTableau.AddCardsBySystem(move.Cards);
                    break;
                case MoveType.FROM_STOCK:
                    foreach (var card in move.Cards) {
                        for (int i = 0; i < TABLEAUS; i++) {
                            tableau[i].GetList().Remove(card);
                        }
                        card.SetFaceDown();
                    }
                    Util.Move(move.Cards, stock.GetList(), move.Cards.Count);
                    break;
            }
        }

        public void Redo() {
            Move move = MovesManager.Redo();
            switch (move.Type) {
                case MoveType.TO_FOUNDATION:
                    MoveCards(move.Cards, move.FromTableau, move.ToFoundation, false);
                    //if (move.FaceUp) move.FromTableau.FaceUpTopCard();
                    break;
                case MoveType.TO_TABLEAU:
                    MoveCards(move.Cards, move.FromTableau, move.ToTableau, false);
                    //if (move.FaceUp) move.FromTableau.FaceUpTopCard();
                    break;
                case MoveType.FROM_STOCK:
                    HandOutFromStock(false);
                    break;
            }
        }

        /// <summary>
        ////Перемещение карт в результирующую стопку.
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="saveToHistory"></param>
        public void MoveCards(List<Card> cards, Tableau from, Foundation to, bool saveToHistory = true) {
            ScoreManager.IncreaseScore(cards.Count);

            // Удаляем карты из таблицы.
            foreach(var _card in cards) {
                from.GetList().Remove(_card);
            }

            if (saveToHistory) {
                bool faceUp = (from.GetTopCard() == null) ? false : from.GetTopCard().IsFaceDown;
                MovesManager.Move(new List<Card>(cards), from, to, faceUp);
            }

            from.FaceUpTopCard();
            to.AddCards(cards);
        }

        public void MoveCards(List<Card> cards, Tableau from, Tableau to, bool saveToHistory = true) {
            // Удаляем карты из таблицы.
            foreach (var _card in cards) {
                from.GetList().Remove(_card);
            }

            if (saveToHistory) {
                bool faceUp = (from.GetTopCard() == null) ? false : from.GetTopCard().IsFaceDown;
                MovesManager.Move(new List<Card>(cards), from, to, faceUp);
            }

            from.FaceUpTopCard();
            to.AddCards(cards);
        }

        private void Clear() {
            stock.GetList().Clear();
            for (int i = 0; i < FOUNDATIONS; i++) {
                leftFoundation[i].GetList().Clear();
                rightFoundation[i].GetList().Clear();
            }
            for (int i = 0; i < TABLEAUS; i++) {
                tableau[i].GetList().Clear();
            }
        }
    }
}
