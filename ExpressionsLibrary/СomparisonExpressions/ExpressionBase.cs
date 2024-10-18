namespace ExpressionsLibrary.СomparisonExpressions
{
    /// <summary>
    /// Базовый класс выражений сравнения.
    /// </summary>
    abstract class ExpressionBase : ExpressionsLibrary.ExpressionBase, IBooleanExpression
    {
        /// <summary>
        /// Значение выражения сравнения.
        /// </summary>
        public abstract bool Value { get; }

        public override object objValue { get { return Value; } }
    }
}
