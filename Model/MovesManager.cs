using System.Collections.Generic;
using Model.Enums;

namespace Model {

    /// <summary>
    /// Контроль за ходами. История ходов.
    /// </summary>
    public class MovesManager {

        private static int moveIndex;
        private static readonly List<Move> moves = new List<Move>();

        public static void NewGame() {
            moves.Clear();
            moveIndex = 0;
        }

        public static Move Undo() {
            if (moveIndex <= 0) return new Move { Type = MoveType.NONE };

            moveIndex--;
            return moves[moveIndex];
        }

        public static Move Redo() {
            if (moveIndex >= moves.Count) return new Move { Type = MoveType.NONE };

            return moves[moveIndex++];
        }

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

        public static void HandOut(List<Card> list) {
            var move = new Move {
                Cards = list,
                Type = MoveType.FROM_STOCK
            };
            AddToHistory(move);
        }

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
