﻿using System.Collections.Generic;
using System.Linq;
using Model.Enums;

namespace Model {

    /// <summary>
    /// Класс таблиц с исходными картами.
    /// </summary>
    public class Tableau : CardList {

        public Tableau() : base() {
        }

        /// <summary>
        /// Добавление карты пользователем.
        /// </summary>
        /// <param name="card"></param>
        public override void AddCard(Card card) {
            if (IsCorrectMove(card)) {
                base.AddCard(card);
            }
        }

        /// <summary>
        /// Добавление карты системой (из запаса).
        /// </summary>
        /// <param name="card"></param>
        public void AddCardBySystem(Card card) {
            base.AddCard(card);
        }

        /// <summary>
        /// Добавление карт пользователем.
        /// </summary>
        /// <param name="cards"></param>
        public override void AddCards(IEnumerable<Card> cards) {
            if (IsCorrectMove(cards.First())) {
                base.AddCards(cards);
            }
        }

        /// <summary>
        /// Добавление карт системой (из запаса).
        /// </summary>
        /// <param name="cards"></param>
        public void AddCardsBySystem(IEnumerable<Card> cards) {
            base.AddCards(cards);
        }

        public List<Card> GetList() {
            return Cards;
        }

        public bool IsCorrectMove(Card card) {
            Card top = GetTopCard();
            return IsCorrectMove(card, top);
        }

        private bool IsCorrectMove(Card card, Card top) {
            // На пустую область можно ложить только короля.
            if (top == null) {
                return (card.Value == CardValue.King);
            }

            bool isAlternatingColor = (top.IsRedSuit() ^ card.IsRedSuit());
            bool isNextCard = (card.Value - top.Value) == -1;

            return (isAlternatingColor && isNextCard);
        }

        public void FaceUpTopCard() {
            Card top = GetTopCard();
            if (top != null) top.SetFaceUp();
        }

        public void FaceDownTopCard() {
            Card top = GetTopCard();
            if (top != null) top.SetFaceDown();
        }

        public Card GetTopCard() {
            if (base.Cards.Count == 0) return null;
            return base.Cards[base.Cards.Count - 1];
        }

        public List<Card> GetDraggableTopCards() {
            if (base.Cards.Count == 0) return null;

            var cards = new List<Card>();
            Card top = base.Cards[base.Cards.Count - 1];
            cards.Add(top);
            if (base.Cards.Count == 1) return cards;

            /*Card beforeTop = base.Cards[base.Cards.Count - 2];
            if (IsCorrectMove(top, beforeTop)) {
                cards.Add(beforeTop);
            }*/

            for (int i = base.Cards.Count - 2; i >= 0; i--) {
                Card beforeTop = base.Cards[i];
                if (IsCorrectMove(top, beforeTop)) {
                    cards.Add(beforeTop);
                }
                top = beforeTop;
            }

            return cards;
        }
    }
}
