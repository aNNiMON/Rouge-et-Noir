using System.Collections.Generic;
using Model.Enums;

namespace Model {

    /// <summary>
    /// Структура данных для учёта перемещений.
    /// </summary>
    public struct Move {
        // Карты
        public List<Card> Cards;

        // Тип перемещения
        public MoveType Type;

        // Откуда перемещено
        public Tableau FromTableau;

        // Куда перемещено
        public Foundation ToFoundation;
        public Tableau ToTableau;

        // Открывать ли последнюю карту?
        public bool FaceUp;
    }
}
