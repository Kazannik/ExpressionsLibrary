using System.Collections.Generic;

namespace ExpressionsLibrary.ArithmeticExpressions
{
	/// <summary>
	/// Элемент отрицательного алгебраического выражения.
	/// </summary>
	class NegativeExpression : ExpressionBase, IExpression
	{
		private new readonly IExpression expression;

		private NegativeExpression(ref Dictionary<string, ICell> cells, UnitCollection array) =>
			expression = Expression.Create(ref cells, array);


		/// <summary>
		/// Признак содержания ошибки в выражении.
		/// </summary>
		public override bool IsError => expression.IsError;

		/// <summary>
		/// Отрицательное значение алгебраического выражения.
		/// </summary>
		public override decimal Value => expression.Value * -1;

		/// <summary>
		/// Строковое представление алгебраического выражения.
		/// </summary>
		public override string Formula() => @"-" + expression.Formula();

		public static IExpression Create(ref Dictionary<string, ICell> cells, UnitCollection array) =>
			new NegativeExpression(ref cells, UnitCollection.Create(array));
	}
}
