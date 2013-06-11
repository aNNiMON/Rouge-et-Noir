namespace Model.Enums {

    /// <summary>
    /// Тип перемещения карты.
    /// </summary>
    public enum MoveType {
        NONE,
        FROM_STOCK, // из запаса в таблицы
        TO_FOUNDATION, // из таблицы в стопку
        TO_TABLEAU // из таблицы в таблицу
    }
}
