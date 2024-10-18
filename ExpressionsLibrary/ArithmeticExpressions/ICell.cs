namespace ExpressionsLibrary.ArithmeticExpressions
{
    /// <summary>
    /// Ячейка, используемая при расчете.
    /// </summary>
    public interface ICell : IDecimalExpression
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
        /// Ключ для доступа к ячеке расчетов.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Присвоить значение ячейке.
        /// </summary>
        /// <param name="value">Значение ячейки.</param>
        void SetValue(decimal value);

    }
}
