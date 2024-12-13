using System.Collections.Generic;

namespace ExpressionsLibrary.BooleanExpressions.CompoundExpressions
{
	/// <summary>
	/// Составное логическое выражение (Например, равенство, сравнение и т.д.)
	/// </summary>
	abstract class CompoundExpression : ExpressionBase, LogicExpressions.ILogicExpression
	{
		protected CompoundExpression(ref Dictionary<string, ICell> cells, UnitCollection left, UnitCollection right) : base()
		{
			LeftExpression = Expression.Create(ref cells, left);
			RightExpression = Expression.Create(ref cells, right);
		}

		protected string GetLeftFormula()
		{
			if (LeftExpression.IsError)
				return LeftExpression.Formula();
			else
				return LeftExpression.Value.ToString();
		}

		protected string GetLeftFormula(string format)
		{
			if (LeftExpression.IsError)
				return LeftExpression.Formula();
			else
				return LeftExpression.ToString(format: format);
		}

		protected string GetRightFormula(string format)
		{
			if (RightExpression.IsError)
				return RightExpression.Formula();
			else
				return RightExpression.ToString(format: format);
		}

		protected string GetRightFormula()
		{
			if (RightExpression.IsError)
				return RightExpression.Formula();
			else
				return RightExpression.Value.ToString();
		}

		/// <summary>
		/// Признак содержания ошибки в выражении.
		/// </summary>
		public override bool IsError
		{
			get { return LeftExpression.IsError || RightExpression.IsError; }
		}

		/// <summary>
		/// Левая часть алгебраического выражения.
		/// </summary>
		public LogicExpressions.ILogicExpression LeftExpression { get; }

		/// <summary>
		/// Правая часть алгебраического выражения.
		/// </summary>
		public LogicExpressions.ILogicExpression RightExpression { get; }

		public static LogicExpressions.ILogicExpression Create(ref Dictionary<string, ICell> cells, UnitCollection array)
		{
			int i = array.GetLastIndex();
			if (i > 0)
			{
				switch (array[i].UnitType)
				{
					case UnitCollection.MatchType.And:
						return AndExpression.Create(ref cells, UnitCollection.Create(array, 0, i), UnitCollection.Create(array, i + 1));
					case UnitCollection.MatchType.Or:
						return OrExpression.Create(ref cells, UnitCollection.Create(array, 0, i), UnitCollection.Create(array, i + 1));
					case UnitCollection.MatchType.Xor:
						return XorExpression.Create(ref cells, UnitCollection.Create(array, 0, i), UnitCollection.Create(array, i + 1));
					case UnitCollection.MatchType.Not:
						return NotExpression.Create(ref cells, UnitCollection.Create(array, 1));
					case UnitCollection.MatchType.True:
						return TrueExpression.Create();
					case UnitCollection.MatchType.False:
						return FalseExpression.Create();
					default:
						return ErrorExpression.Create(array);
				}
			}
			else if (i == -2)
				return ErrorExpression.Create(array);
			else
				return Expression.Create(ref cells, array);
		}
	}
}
