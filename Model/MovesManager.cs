using System.Collections.Generic;
using Model.Enums;

namespace Model {

    /// <summary>
    /// Контроль за ходами. История ходов.
    /// </summary>
    public class MovesManager {

        private static int moveIndex = 0;
        private static List<Move> moves = new List<Move>();

        public static Move Undo() {
            if (moveIndex <= 0) return new Move { Type = MoveType.NONE };

            moveIndex--;
            return moves[moveIndex];
        }

        public static Move Redo() {
            if (moveIndex >= moves.Count) return new Move { Type = MoveType.NONE };

            moveIndex++;
            return moves[moveIndex];
        }

        public static void Move(Card card, Tableau from, Foundation to, bool faceUp) {
            Move move = new Move() {
                Card = card,
                FromTableau = from,
                ToFoundation = to,
                FaceUp = faceUp,
                Type = MoveType.TO_FOUNDATION
            };
            AddToHistory(move);
        }

        public static void Move(Card card, Tableau from, Tableau to, bool faceUp) {
            Move move = new Move() {
                Card = card,
                FromTableau = from,
                ToTableau = to,
                FaceUp = faceUp,
                Type = MoveType.TO_TABLEAU
            };
            AddToHistory(move);
        }

        public static void HandOut(List<Card> list) {
            Move move = new Move() {
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
