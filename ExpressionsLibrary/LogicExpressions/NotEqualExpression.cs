using System.Collections.Generic;

namespace ExpressionsLibrary.LogicExpressions
{
	/// <summary>
	/// Логическое выражение (НЕ РАВНО).
	/// </summary>
	class NotEqualExpression : CompoundExpression, ILogicExpression
	{
		private NotEqualExpression(ref Dictionary<string, ICell> cells, UnitCollection left, UnitCollection right) : base(ref cells, left, right) { }

		/// <summary>
		/// Положительное значение логического выражения.
		/// </summary>
		public override bool Value
		{
			get { return (LeftExpression.Value != RightExpression.Value); }
		}

		/// <summary>
		/// Строковое представление логического выражения.
		/// </summary>
		public override string Formula()
		{
			return LeftExpression.Formula() + ArithmeticExpression.SymbolSpace + LogicExpression.SymbolNotEqual + ArithmeticExpression.SymbolSpace + RightExpression.Formula();
		}

		/// <summary>
		/// Короткое строковое представление логического выражения.
		/// </summary>
		/// <param name="format">Формат отображения результата алгебраического выражения.</param>
		public override string ToString(string format)
		{
			if (IsFormat(format: format))
				return GetLeftFormula(format: format) + ArithmeticExpression.SymbolSpace + LogicExpression.SymbolNotEqual + ArithmeticExpression.SymbolSpace + GetRightFormula(format: format);
			else
				return GetLeftFormula() + ArithmeticExpression.SymbolSpace + LogicExpression.SymbolNotEqual + ArithmeticExpression.SymbolSpace + GetRightFormula();
		}

		public static ILogicExpression Create(ref Dictionary<string, ICell> cells, UnitCollection left, UnitCollection right)
		{
			return new NotEqualExpression(ref cells, left, right);
		}
	}
}
