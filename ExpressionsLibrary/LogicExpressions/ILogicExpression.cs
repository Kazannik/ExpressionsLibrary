namespace ExpressionsLibrary.LogicExpressions
{
	/// <summary>
	/// Интерфейс логического выражения.
	/// </summary>
	public interface ILogicExpression : IExpression
	{
		/// <summary>
		/// Значение логического выражения.
		/// </summary>
		bool Value { get; }
	}
}
