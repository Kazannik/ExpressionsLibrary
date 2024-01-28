namespace ExpressionsLibrary.ArithmeticExpressions
{
    /// <summary>
    /// Ячейка, используемая при расчете.
    /// </summary>
    public interface ICell
    {
        /// <summary>
        /// Формат отображения формулы числа.
        /// </summary>
        string Format { get; set; }

        /// <summary>
        /// Формат отображения формулы ячейки.
        /// </summary>
        string FormulaFormat { get; set; }

        /// <summary>
        /// Строковое представление алгебраического выражения.
        /// </summary>
        string Formula();

        /// <summary>
        /// Ключ для доступа к ячеке расчетов.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Присвоить значение ячейке.
        /// </summary>
        /// <param name="value">Значение ячейки.</param>
        void SetValue(decimal value);

        /// <summary>
        /// Значение алгебраического выражения.
        /// </summary>
        decimal Value { get; }

        /// <summary>
        /// Определяемые пользователем данные, связанные с этим объектом.
        /// </summary>
        object Tag { get; set; }
    }
}
