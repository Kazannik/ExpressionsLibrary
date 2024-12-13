namespace ExpressionsLibrary.ArithmeticExpressions
{
	/// <summary>
	/// Интерфейс алгебраического выражения.
	/// </summary>
	interface IExpression : ExpressionsLibrary.IExpression
	{
		/// <summary>
		/// Значение алгебраического выражения.
		/// </summary>
		decimal Value { get; }
	}
}
