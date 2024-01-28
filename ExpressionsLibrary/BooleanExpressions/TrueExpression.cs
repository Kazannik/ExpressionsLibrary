namespace ExpressionsLibrary.BooleanExpressions
{
    /// <summary>
    /// Положительное логическое выражение.
    /// </summary>
    class TrueExpression : ExpressionBase
    {
        public static TrueExpression Create()
        {
            return new TrueExpression();
        }

        private TrueExpression()
        {
            IsError = false;
            Value = true;
        }

        /// <summary>
        /// Признак содержания ошибки в выражении.
        /// </summary>
        public override bool IsError { get; }

        /// <summary>
        /// Положительное значение логического выражения.
        /// </summary>
        public override bool Value { get; }

        /// <summary>
        /// Строковое представление логического выражения.
        /// </summary>
        public override string Formula()
        {
            return BooleanExpression.SymbolTrue;
        }
        /// <summary>
        /// Короткое строковое представление логического выражения.
        /// </summary>
        /// <param name="format">Формат отображения результата алгебраического выражения.</param>
        public override string ToString(string format)
        {
            return Formula();
        }
    }
}
