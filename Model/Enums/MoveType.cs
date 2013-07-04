namespace Model.Enums {

    /// <summary>
    /// Тип перемещения карты.
    /// </summary>
    public enum MoveType {
        None,
        FromStock, // из запаса в таблицы
        ToFoundation, // из таблицы в стопку
        ToTableau // из таблицы в таблицу
    }
}
