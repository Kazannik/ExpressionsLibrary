namespace ExpressionsLibrary.BooleanExpressions
{
    /// <summary>
    /// Ошибочное выражение.
    /// </summary>
    class ErrorExpression : ExpressionBase
    {
        private string formula;

        private ErrorExpression(UnitCollection array)
        {
            IsError = true;
            Value = false;
            formula = BooleanExpression.SymbolStartError;
            foreach (UnitCollection.BaseUnit u in array)
            {
                if (formula.Length > 0) { this.formula += ArithmeticExpression.SymbolSpace; }
                formula += u.Value;
            }
            formula += BooleanExpression.SymbolEndError;
        }

        /// <summary>
        /// Признак содержания ошибки в выражении.
        /// </summary>
        public override bool IsError { get; }

        /// <summary>
        /// Значение логического выражения.
        /// </summary>
        public override bool Value { get; }

        /// <summary>
        /// Строковое представление логического выражения.
        /// </summary>
        public override string Formula()
        {
            return formula;
        }

        /// <summary>
        /// Короткое строковое представление логического выражения.
        /// </summary>
        /// <param name="format">Формат отображения результата алгебраического выражения.</param>
        public override string ToString(string format)
        {
            return Formula();
        }

        public static ErrorExpression Create(UnitCollection.BaseUnit unit)
        {
            return new ErrorExpression(UnitCollection.Create(unit));
        }

        public static ErrorExpression Create(UnitCollection array)
        {
            return new ErrorExpression(array);
        }
    }
}
