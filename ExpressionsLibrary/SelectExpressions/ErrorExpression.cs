namespace ExpressionsLibrary.SelectExpressions
{
    /// <summary>
    /// Ошибочное выражение.
    /// </summary>
    class ErrorExpression : ExpressionBase, ISelectExpression
    {
        private string formula;

        private ErrorExpression(UnitCollection array)
        {
            IsError = true;
            IsTrue = false;
            formula = SelectExpression.SymbolStartError;
            foreach (UnitCollection.IUnit u in array)
            {
                if (formula.Length > 0) { formula += ArithmeticExpression.SymbolSpace; }
                formula += u.Value;
            }
            formula += SelectExpression.SymbolEndError;
        }

        /// <summary>
        /// Признак содержания ошибки в выражении.
        /// </summary>
        public override bool IsError { get; }

        /// <summary>
        /// Значение секции Select выражения ветвления.
        /// </summary>
        public override bool IsTrue { get; }

        /// <summary>
        /// Значение выражения ветвления.
        /// </summary>
        public override object objValue
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Строковое представление выражения ветвления.
        /// </summary>
        public override string Formula()
        {
            return formula;
        }

        public override string Formula(string format)
        {
            return formula;
        }

        /// <summary>
        /// Короткое строковое представление выражения ветвления.
        /// </summary>
        /// <param name="format">Формат отображения результата алгебраического выражения.</param>
        public override string ToString(string format)
        {
            return Formula();
        }

        public static ISelectExpression Create(UnitCollection.IUnit unit)
        {
            return new ErrorExpression(UnitCollection.Create(unit));
        }

        public static ISelectExpression Create(UnitCollection array)
        {
            return new ErrorExpression(array);
        }
    }
}
