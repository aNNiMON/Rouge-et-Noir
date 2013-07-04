using System.Collections.Generic;
using Model.Enums;

namespace Model {

    /// <summary>
    /// Контроль за ходами. История ходов.
    /// </summary>
    public static class MovesManager {

        private static int _moveIndex;
        private static readonly List<Move> Moves = new List<Move>();

        /// <summary>
        /// Начата новая игра.
        /// </summary>
        public static void NewGame() {
            Moves.Clear();
            _moveIndex = 0;
        }

        /// <summary>
        /// Отмена последнего действия.
        /// </summary>
        /// <returns>состояние игры после отмены действия</returns>
        public static Move Undo() {
            if (_moveIndex <= 0) return new Move { Type = MoveType.None };

            _moveIndex--;
            return Moves[_moveIndex];
        }

        /// <summary>
        /// Повтор последнего действия.
        /// </summary>
        /// <returns>состояние игры после повтора действия</returns>
        public static Move Redo() {
            if (_moveIndex >= Moves.Count) return new Move { Type = MoveType.None };

            return Moves[_moveIndex++];
        }

        /// <summary>
        /// Возвращает текущий указатель на ход.
        /// </summary>
        /// <returns></returns>
        public static int GetMoveIndex() {
            return _moveIndex;
        }

        /// <summary>
        /// Слхранение записи о перемещении карт в стопку.
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="faceUp"></param>
        public static void Move(List<Card> cards, Tableau from, Foundation to, bool faceUp) {
            var move = new Move {
                Cards = cards,
                FromTableau = from,
                ToFoundation = to,
                FaceUp = faceUp,
                Type = MoveType.ToFoundation
            };
            AddToHistory(move);
        }

        /// <summary>
        /// Сохранение записи о перемещении карт между таблицами.
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="faceUp"></param>
        public static void Move(List<Card> cards, Tableau from, Tableau to, bool faceUp) {
            var move = new Move {
                Cards = cards,
                FromTableau = from,
                ToTableau = to,
                FaceUp = faceUp,
                Type = MoveType.ToTableau
            };
            AddToHistory(move);
        }

        /// <summary>
        /// Сохранение записи о раздаче карт из запаса.
        /// </summary>
        /// <param name="list"></param>
        public static void HandOut(List<Card> list) {
            var move = new Move {
                Cards = list,
                Type = MoveType.FromStock
            };
            AddToHistory(move);
        }

        /// <summary>
        /// Добавить запись в историю изменений.
        /// </summary>
        /// <param name="move"></param>
        private static void AddToHistory(Move move) {
            if (_moveIndex != Moves.Count) {
                int removeLength = Moves.Count - _moveIndex;
                Moves.RemoveRange(_moveIndex, removeLength);
            }
            Moves.Add(move);
            _moveIndex++;
        }
    }
}
