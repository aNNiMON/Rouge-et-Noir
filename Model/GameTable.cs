using System;
using System.Collections.Generic;
using System.Linq;
using Model.Enums;

namespace Model {

    /// <summary>
    /// Класс игрового поля.
    /// Содержит результирующие стопки, таблицы и запас. Управляет взаимодействием.
    /// </summary>
    public class GameTable {

        public const int Foundations = 4, Tableaus = 10;

        private readonly Stock _stock;
        private readonly LeftFoundation[] _leftFoundation;
        private readonly RightFoundation[] _rightFoundation;
        private readonly Tableau[] _tableau;

        public GameTable() {
            _stock = new Stock();
            _leftFoundation = new LeftFoundation[Foundations];
            _rightFoundation = new RightFoundation[Foundations];
            for (int i = 0; i < Foundations; i++) {
                _leftFoundation[i] = new LeftFoundation();
                _rightFoundation[i] = new RightFoundation();
            }
            _tableau = new Tableau[Tableaus];
            for (int i = 0; i < Tableaus; i++) {
                _tableau[i] = new Tableau();
            }
        }

        public Tableau GetTableau(int num) {
            if (num >= Tableaus) throw new IndexOutOfRangeException();
            return _tableau[num];
        }

        public Foundation GetFoundation(int num, bool left) {
            if (num >= Foundations) throw new IndexOutOfRangeException();
            if (left) return _leftFoundation[num];
            return _rightFoundation[num];
        }

        public Stock GetStock() {
            return _stock;
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
            int addCardsCount = 9;
            for (int i = 0; i < Tableaus - 1; i++) {
                Util.Move(cards, _tableau[i].GetList(), addCardsCount);
                addCardsCount--;
            }
            // Переворачиваем верхние карты.
            for (int i = 0; i < Tableaus - 1; i++) {
                _tableau[i].GetTopCard().SetFaceUp();
            }

            // Добавляем оставшиеся 67 карт в запас.
            _stock.AddCards(cards);
        }

        /// <summary>
        /// Перезапустить игру.
        /// </summary>
        /// <param name="clearMoves">очищать ли историю ходов и очки</param>
        public void RestartGame(bool clearMoves = true) {
            if (!clearMoves) ScoreManager.FreezeScore();

            while (MovesManager.GetMoveIndex() > 0) {
                Undo();
            }
            if (clearMoves) {
                MovesManager.NewGame();
                ScoreManager.InitNewGame();
            }
        }

        /// <summary>
        /// Завершение игры.
        /// </summary>
        /// <param name="isComplete">завершена ли игра или прервана</param>
        public void EndGame(bool isComplete) {
            ScoreManager.EndGame(isComplete);
        }

        /// <summary>
        /// Раздача карт из запаса.
        /// </summary>
        public void HandOutFromStock(bool saveToHistory = true) {
            var cards = new List<Card>();
            Util.Move(_stock.GetList(), cards, Tableaus);
            if (saveToHistory) {
                MovesManager.HandOut(new List<Card>(cards));
            }
            // Раскрываем карты.
            foreach (var card in cards) {
                card.SetFaceUp();
            }
            // Переносим в таблицы.
            for (int i = 0; i < Tableaus; i++) {
                if (cards.Count == 0) return;
                Util.Move(cards, _tableau[i].GetList(), 1);
            }
        }

        /// <summary>
        /// Отмена последнего действия.
        /// </summary>
        public void Undo() {
            Move move = MovesManager.Undo();
            switch (move.Type) {
                case MoveType.ToFoundation:
                    // Перекладываем карты обратно из стопки.
                    ScoreManager.DecreaseScore(move.Cards.Count);
                    // Удаляем карты из таблицы.
                    foreach (var card in move.Cards) {
                        move.ToFoundation.GetList().Remove(card);
                    }
                    if (move.FaceUp) move.FromTableau.FaceDownTopCard();
                    move.FromTableau.AddCardsBySystem(move.Cards);
                    break;
                case MoveType.ToTableau:
                    // Перекладываем в исходную таблицу.
                    // Удаляем карты из таблицы.
                    foreach (var card in move.Cards) {
                        move.ToTableau.GetList().Remove(card);
                    }
                    if (move.FaceUp) move.FromTableau.FaceDownTopCard();
                    move.FromTableau.AddCardsBySystem(move.Cards);
                    break;
                case MoveType.FromStock:
                    // Отмена раздачи карт из запаса.
                    var copy = new List<Card>(move.Cards);
                    foreach (var card in copy) {
                        for (int i = 0; i < Tableaus; i++) {
                            _tableau[i].GetList().Remove(card);
                        }
                        card.SetFaceDown();
                    }
                    Util.Move(copy, _stock.GetList(), move.Cards.Count);
                    break;
            }
        }

        /// <summary>
        /// Повтор отменённого действия.
        /// </summary>
        public void Redo() {
            Move move = MovesManager.Redo();
            switch (move.Type) {
                case MoveType.ToFoundation:
                    MoveCards(move.Cards, move.FromTableau, move.ToFoundation, false);
                    break;
                case MoveType.ToTableau:
                    MoveCards(move.Cards, move.FromTableau, move.ToTableau, false);
                    break;
                case MoveType.FromStock:
                    HandOutFromStock(false);
                    break;
            }
        }

        /// <summary>
        /// Перемещение карт в результирующую стопку.
        /// </summary>
        /// <param name="cards">список перемещаемых карт</param>
        /// <param name="from">из какой таблицы перемещаем</param>
        /// <param name="to">в какую стопку</param>
        /// <param name="saveToHistory">отметить ход в истории изменений</param>
        public void MoveCards(List<Card> cards, Tableau from, Foundation to, bool saveToHistory = true) {
            ScoreManager.IncreaseScore(cards.Count);

            // Удаляем карты из таблицы.
            foreach(var card in cards) {
                from.GetList().Remove(card);
            }

            if (saveToHistory) {
                bool faceUp = (from.GetTopCard() == null) ? false : from.GetTopCard().IsFaceDown;
                MovesManager.Move(new List<Card>(cards), from, to, faceUp);
            }

            from.FaceUpTopCard();
            to.AddCards(cards);
        }

        /// <summary>
        /// Перемещение карт между таблицами.
        /// </summary>
        /// <param name="cards">список перемещаемых карт</param>
        /// <param name="from">из какой таблицы перемещаем</param>
        /// <param name="to">в какую таблицу</param>
        /// <param name="saveToHistory">отметить ход в истории изменений</param>
        public void MoveCards(List<Card> cards, Tableau from, Tableau to, bool saveToHistory = true) {
            // Удаляем карты из таблицы.
            foreach (var card in cards) {
                from.GetList().Remove(card);
            }

            if (saveToHistory) {
                bool faceUp = (from.GetTopCard() == null) ? false : from.GetTopCard().IsFaceDown;
                MovesManager.Move(new List<Card>(cards), from, to, faceUp);
            }

            from.FaceUpTopCard();
            to.AddCards(cards);
        }

        /// <summary>
        /// Очистить игровое поле.
        /// </summary>
        private void Clear() {
            _stock.GetList().Clear();
            for (int i = 0; i < Foundations; i++) {
                _leftFoundation[i].GetList().Clear();
                _rightFoundation[i].GetList().Clear();
            }
            for (int i = 0; i < Tableaus; i++) {
                _tableau[i].GetList().Clear();
            }
        }
    }
}
