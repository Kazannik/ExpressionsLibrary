namespace ExpressionsLibrary
{
    /// <summary>
    /// Интерфейс алгебраического выражения.
    /// </summary>
    public interface IDecimalExpression : IExpression
    {
        /// <summary>
        /// Значение алгебраического выражения.
        /// </summary>
        decimal Value { get; }
    }
}
