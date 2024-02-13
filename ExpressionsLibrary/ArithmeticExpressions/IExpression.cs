namespace ExpressionsLibrary.ArithmeticExpressions
{
    public interface IExpression: ExpressionsLibrary.IExpression
    {
        /// <summary>
        /// Значение алгебраического выражения.
        /// </summary>
        decimal Value { get; }
    }
}
