﻿using System.Collections.Generic;

namespace ExpressionsLibrary.BooleanExpressions.CompoundExpressions
{
	/// <summary>
	/// Логическое "И" (AND).
	/// </summary>
	class AndExpression : CompoundExpression
	{
		private AndExpression(ref Dictionary<string, ICell> cells, UnitCollection left, UnitCollection right) : base(ref cells, left, right) { }

		/// <summary>
		/// Положительное значение логического выражения.
		/// </summary>
		public override bool Value
		{
			get { return (LeftExpression.Value & RightExpression.Value); }
		}

		/// <summary>
		/// Строковое представление логического выражения.
		/// </summary>
		public override string Formula()
		{
			return LeftExpression.Formula() + ArithmeticExpression.SymbolSpace + BooleanExpression.SymbolAnd + ArithmeticExpression.SymbolSpace + RightExpression.Formula();
		}

		/// <summary>
		/// Короткое строковое представление логического выражения.
		/// </summary>
		/// <param name="format">Формат отображения результата алгебраического выражения.</param>
		public override string ToString(string format)
		{
			if (IsFormat(format: format))
				return GetLeftFormula(format: format) + ArithmeticExpression.SymbolSpace + BooleanExpression.SymbolAnd + ArithmeticExpression.SymbolSpace + GetRightFormula(format: format);
			else
				return GetLeftFormula() + ArithmeticExpression.SymbolSpace + BooleanExpression.SymbolAnd + ArithmeticExpression.SymbolSpace + GetRightFormula();
		}

		public static AndExpression Create(ref Dictionary<string, ICell> cells, UnitCollection left, UnitCollection right)
		{
			return new AndExpression(ref cells, left, right);
		}
	}
}
