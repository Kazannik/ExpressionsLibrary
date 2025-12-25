using System.Collections.Generic;

namespace ExpressionsLibrary.ArithmeticExpressions
{
	/// <summary>
	/// Алгебраическое выражение, заключенное в скобки.
	/// </summary>
	class AssociationExpression : ExpressionBase, IExpression
	{
		private new readonly IExpression expression;

		private AssociationExpression(ref Dictionary<string, ICell> cells, UnitCollection array) =>
			expression = Expression.Create(ref cells, array);


		/// <summary>
		/// Признак содержания ошибки в выражении.
		/// </summary>
		public override bool IsError => expression.IsError;

		/// <summary>
		/// Значение алгебраического выражения.
		/// </summary>
		public override decimal Value => expression.Value;

		/// <summary>
		/// Строковое представление алгебраического выражения.
		/// </summary>
		public override string Formula() => @"(" + expression.Formula() + @")";

		public static IExpression Create(ref Dictionary<string, ICell> cells, UnitCollection array) =>
			new AssociationExpression(ref cells, UnitCollection.Create(array, 1, array.Count - 2));
	}
}
