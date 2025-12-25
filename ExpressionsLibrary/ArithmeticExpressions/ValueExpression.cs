namespace ExpressionsLibrary.ArithmeticExpressions
{
	/// <summary>
	/// Ячейка с фиксированным значением
	/// </summary>
	class ValueExpression : ExpressionBase, IExpression
	{
		private ValueExpression(decimal value)
		{
			IsError = false;
			Value = value;
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
		public override string Formula() => Value.ToString();

		public static IExpression Create(decimal value) => new ValueExpression(value);

		public static IExpression Create(string stringVal)
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
