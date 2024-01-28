namespace ExpressionsLibrary.ArithmeticExpressions
{
    /// <summary>
    /// Ячейка с фиксированным значением
    /// </summary>
    class ValueExpression : ExpressionBase
    {
        private ValueExpression(decimal value)
        {
            this.IsError = false;
            this.Value = value;
        }

        /// <summary>
        /// Признак содержания ошибки в выражении.
        /// </summary>
        public override bool IsError { get; }

        /// <summary>
        /// Значение алгебраического выражения.
        /// </summary>
        public override decimal Value { get; }

        /// <summary>
        /// Строковое представление алгебраического выражения.
        /// </summary>
        public override string Formula()
        {
            return this.Value.ToString();
        }

        public static ValueExpression Create(decimal value)
        {
            return new ValueExpression(value);
        }

        public static ValueExpression Create(string stringVal)
        {
            decimal decimalVal = 0;
            try
            {
                decimalVal = System.Convert.ToDecimal(stringVal);
            }
            catch (System.OverflowException) { }
            return new ValueExpression(decimalVal);
        }
    }
}
