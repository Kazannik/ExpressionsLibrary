namespace ExpressionsLibrary.ArithmeticExpressions
{
    /// <summary>
    /// Базовый класс алгебраического выражений.
    /// </summary>
    public abstract class ExpressionBase: ExpressionsLibrary.ExpressionBase
    {
        /// <summary>
        /// Значение алгебраического выражения.
        /// </summary>
        public abstract decimal Value { get; }

        public override string ToString(string format)
        {
            if (IsFormat(format: format))
                return Value.ToString(format: format);
            else
                return Value.ToString();
        }
    }
}
