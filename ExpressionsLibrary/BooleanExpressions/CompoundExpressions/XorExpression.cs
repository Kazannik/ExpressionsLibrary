using System.Collections.Generic;

namespace ExpressionsLibrary.BooleanExpressions.CompoundExpressions
{
	/// <summary>
	/// Исключающее ИЛИ (XOR).
	/// </summary>
	class XorExpression : CompoundExpression, LogicExpressions.ILogicExpression
	{
		private XorExpression(ref Dictionary<string, ICell> cells, UnitCollection left, UnitCollection right) : base(ref cells, left, right) { }

		/// <summary>
		/// Положительное значение логического выражения.
		/// </summary>
		public override bool Value
		{
			get
			{
				if (LeftExpression.Value || RightExpression.Value)
					return true;
				else if (LeftExpression.Value == RightExpression.Value)
					return false;
				else
					return true;
			}
		}

		/// <summary>
		/// Строковое представление логического выражения.
		/// </summary>
		public override string Formula() =>
			LeftExpression.Formula() + ArithmeticExpression.SymbolSpace + BooleanExpression.SymbolXor + 
			ArithmeticExpression.SymbolSpace + RightExpression.Formula();
		

		/// <summary>
		/// Короткое строковое представление логического выражения.
		/// </summary>
		/// <param name="format">Формат отображения результата алгебраического выражения.</param>
		public override string ToString(string format)
		{
			if (IsFormat(format: format))
				return GetLeftFormula(format: format) + ArithmeticExpression.SymbolSpace + BooleanExpression.SymbolXor + ArithmeticExpression.SymbolSpace + GetRightFormula(format: format);
			else
				return GetLeftFormula() + ArithmeticExpression.SymbolSpace + BooleanExpression.SymbolXor + ArithmeticExpression.SymbolSpace + GetRightFormula();
		}

		public static LogicExpressions.ILogicExpression Create(ref Dictionary<string, ICell> cells, UnitCollection left, UnitCollection right) =>
			new XorExpression(ref cells, left, right);
	}
}
