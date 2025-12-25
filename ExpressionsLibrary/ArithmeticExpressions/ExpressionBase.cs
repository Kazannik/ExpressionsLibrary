namespace ExpressionsLibrary.ArithmeticExpressions
{
	/// <summary>
	/// Базовый класс алгебраического выражений.
	/// </summary>
	abstract class ExpressionBase : ExpressionsLibrary.ExpressionBase, IExpression
	{
		/// <summary>
		/// Значение алгебраического выражения.
		/// </summary>
		public abstract decimal Value { get; }

		public override object ObjValue => Value;

		public override string ToString(string format)
		{
			if (IsFormat(format: format))
				return Value.ToString(format: format);
			else
				return Value.ToString();
		}
	}
}
