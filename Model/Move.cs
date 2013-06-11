using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Enums;

namespace Model {

    /// <summary>
    /// Структура данных для учёта перемещений.
    /// </summary>
    public struct Move {
        // Карта / карты
        public Card Card;
        public List<Card> Cards;

        // Тип перемещения
        public MoveType Type;

        // Откуда перемещено
        public Tableau FromTableau;

        // Куда перемещено
        public Foundation ToFoundation;
        public Tableau ToTableau;
    }
}
