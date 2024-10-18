namespace ExpressionsLibrary.SelectExpressions
{
    /// <summary>
    /// Интерфейс выражения ветвления.
    /// </summary>
    interface ISelectExpression : IExpression
    {
        /// <summary>
        /// Значение секции Select (IF) выражения ветвления.
        /// </summary>
        bool IsTrue { get; }
    }
}
