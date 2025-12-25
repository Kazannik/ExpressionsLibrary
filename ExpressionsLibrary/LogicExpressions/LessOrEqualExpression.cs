using System.Collections.Generic;

namespace ExpressionsLibrary.LogicExpressions
{
	/// <summary>
	/// Логическое выражение (МЕНЬШЕ ИЛИ РАВНО).
	/// </summary>
	class LessOrEqualExpression : CompoundExpression, ILogicExpression
	{
		private LessOrEqualExpression(ref Dictionary<string, ICell> cells, UnitCollection left, UnitCollection right) : base(ref cells, left, right) { }

		/// <summary>
		/// Положительное значение логического выражения.
		/// </summary>
		public override bool Value => LeftExpression.Value <= RightExpression.Value;

		/// <summary>
		/// Строковое представление логического выражения.
		/// </summary>
		public override string Formula() =>
			LeftExpression.Formula() + ArithmeticExpression.SymbolSpace + LogicExpression.SymbolLessOrEqual +
			ArithmeticExpression.SymbolSpace + RightExpression.Formula();


		/// <summary>
		/// Короткое строковое представление логического выражения.
		/// </summary>
		/// <param name="format">Формат отображения результата алгебраического выражения.</param>
		public override string ToString(string format)
		{
			if (IsFormat(format: format))
				return GetLeftFormula(format: format) + ArithmeticExpression.SymbolSpace + LogicExpression.SymbolLessOrEqual + ArithmeticExpression.SymbolSpace + GetRightFormula(format: format);
			else
				return GetLeftFormula() + ArithmeticExpression.SymbolSpace + LogicExpression.SymbolLessOrEqual + ArithmeticExpression.SymbolSpace + GetRightFormula();
		}

		public static ILogicExpression Create(ref Dictionary<string, ICell> cells, UnitCollection left, UnitCollection right) =>
			new LessOrEqualExpression(ref cells, left, right);
	}
}
