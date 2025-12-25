using System.Collections.Generic;

namespace ExpressionsLibrary.BooleanExpressions
{
	/// <summary>
	/// Логическое выражение, заключенное в скобки.
	/// </summary>
	class AssociationExpression : ExpressionBase, LogicExpressions.ILogicExpression
	{
		private new readonly LogicExpressions.ILogicExpression expression;

		private AssociationExpression(ref Dictionary<string, ICell> cells, UnitCollection array) =>
			expression = Expression.Create(ref cells, array);
		

		/// <summary>
		/// Признак содержания ошибки в выражении.
		/// </summary>
		public override bool IsError => expression.IsError;

		/// <summary>
		/// Значение логического выражения.
		/// </summary>
		public override bool Value => expression.Value;

		/// <summary>
		/// Строковое представление логического выражения.
		/// </summary>
		public override string Formula() => @"(" + expression.Formula() + @")";

		public static LogicExpressions.ILogicExpression Create(ref Dictionary<string, ICell> cells, UnitCollection array) =>
			new AssociationExpression(ref cells, UnitCollection.Create(array, 1, array.Count - 2));
		

		/// <summary>
		/// Короткое строковое представление логического выражения.
		/// </summary>
		/// <param name="format">Формат отображения результата алгебраического выражения.</param>
		public override string ToString(string format) => Formula();
	}
}
