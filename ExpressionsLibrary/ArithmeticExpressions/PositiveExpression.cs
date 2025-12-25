using System.Collections.Generic;

namespace ExpressionsLibrary.ArithmeticExpressions
{
	/// <summary>
	/// Элемент положительного алгебраического выражения.
	/// </summary>
	class PositiveExpression : ExpressionBase, IExpression
	{
		private new readonly IExpression expression;

		public static IExpression Create(ref Dictionary<string, ICell> cells, UnitCollection array) =>
			new PositiveExpression(ref cells, UnitCollection.Create(array));


		private PositiveExpression(ref Dictionary<string, ICell> cells, UnitCollection array)
		{
			expression = Expression.Create(ref cells, array);
		}

		/// <summary>
		/// Признак содержания ошибки в выражении.
		/// </summary>
		public override bool IsError => expression.IsError;

		/// <summary>
		/// Положительное значение алгебраического выражения.
		/// </summary>
		public override decimal Value => +1 * expression.Value;

		/// <summary>
		/// Строковое представление алгебраического выражения.
		/// </summary>
		public override string Formula() => @"+" + expression.Formula();
	}
}
