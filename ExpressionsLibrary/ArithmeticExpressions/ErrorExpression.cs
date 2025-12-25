namespace ExpressionsLibrary.ArithmeticExpressions
{
	/// <summary>
	/// Ошибочное выражение.
	/// </summary>
	class ErrorExpression : ExpressionBase, IExpression
	{
		private readonly string formula;

		private ErrorExpression(UnitCollection array)
		{
			IsError = true;
			Value = 0;
			formula = ArithmeticExpression.SymbolStartError;
			foreach (UnitCollection.IUnit u in array)
			{
				if (formula.Length > 0) { formula += ArithmeticExpression.SymbolSpace; }
				formula += u.Value;
			}
			formula += ArithmeticExpression.SymbolEndError;
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
		public override string Formula() => formula;

		public static IExpression Create(UnitCollection.IUnit unit) => new ErrorExpression(UnitCollection.Create(unit));
		
		public static IExpression Create(UnitCollection array) => new ErrorExpression(array);
		
	}
}
