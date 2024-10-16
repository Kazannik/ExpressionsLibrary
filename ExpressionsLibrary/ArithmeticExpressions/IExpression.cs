namespace ExpressionsLibrary.ArithmeticExpressions
{
    /// <summary>
    /// Интерфейс алгебраического выражения.
    /// </summary>
    public interface IExpression : ExpressionsLibrary.IExpression
    {
        /// <summary>
        /// Значение алгебраического выражения.
        /// </summary>
        decimal Value { get; }
    }
}
