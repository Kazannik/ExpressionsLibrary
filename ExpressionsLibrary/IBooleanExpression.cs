namespace ExpressionsLibrary
{
    /// <summary>
    /// Интерфейс логического выражения.
    /// </summary>
    public interface IBooleanExpression : IExpression
    {
        /// <summary>
        /// Значение логического выражения.
        /// </summary>
        bool Value { get; }
    }
}
