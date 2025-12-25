namespace ExpressionsLibrary.LogicExpressions
{
	/// <summary>
	/// Интерфейс логического выражения.
	/// </summary>
	interface ILogicExpression : IExpression
	{
		/// <summary>
		/// Значение логического выражения.
		/// </summary>
		bool Value { get; }
	}
}
