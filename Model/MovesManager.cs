using System.Collections.Generic;
using Model.Enums;

namespace Model {

    /// <summary>
    /// Контроль за ходами. История ходов.
    /// </summary>
    public static class MovesManager {

        private static int moveIndex;
        private static readonly List<Move> moves = new List<Move>();

        /// <summary>
        /// Начата новая игра.
        /// </summary>
        public static void NewGame() {
            moves.Clear();
            moveIndex = 0;
        }

        /// <summary>
        /// Отмена последнего действия.
        /// </summary>
        /// <returns>состояние игры после отмены действия</returns>
        public static Move Undo() {
            if (moveIndex <= 0) return new Move { Type = MoveType.NONE };

            moveIndex--;
            return moves[moveIndex];
        }

        /// <summary>
        /// Повтор последнего действия.
        /// </summary>
        /// <returns>состояние игры после повтора действия</returns>
        public static Move Redo() {
            if (moveIndex >= moves.Count) return new Move { Type = MoveType.NONE };

            return moves[moveIndex++];
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
                Type = MoveType.TO_FOUNDATION
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
                Type = MoveType.TO_TABLEAU
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
                Type = MoveType.FROM_STOCK
            };
            AddToHistory(move);
        }

        /// <summary>
        /// Добавить запись в историю изменений.
        /// </summary>
        /// <param name="move"></param>
        private static void AddToHistory(Move move) {
            if (moveIndex != moves.Count) {
                int removeLength = moves.Count - moveIndex;
                moves.RemoveRange(moveIndex, removeLength);
            }
            moves.Add(move);
            moveIndex++;
        }
    }
}
