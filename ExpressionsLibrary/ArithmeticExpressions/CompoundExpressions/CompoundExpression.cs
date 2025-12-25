using System.Collections.Generic;

namespace ExpressionsLibrary.ArithmeticExpressions.CompoundExpressions
{
	/// <summary>
	/// Составное алгебраическое выражение (Например, сложение, вычитание, умножение и т.д.)
	/// </summary>
	abstract class CompoundExpression : ExpressionBase, IExpression
	{
		protected CompoundExpression(ref Dictionary<string, ICell> cells, UnitCollection left, UnitCollection right) : base()
		{
			LeftExpression = Expression.Create(ref cells, left);
			RightExpression = Expression.Create(ref cells, right);
		}

		/// <summary>
		/// Формула левого алгебраического выражения.
		/// </summary>
		/// <returns></returns>
		protected string GetLeftFormula()
		{
			if (LeftExpression.IsError)
				return LeftExpression.ToString();
			else
				return LeftExpression.Value.ToString();
		}

		/// <summary>
		/// Формула левого алгебраического выражения.
		/// </summary>
		/// <param name="format">Описатели стандартного формата.</param>
		/// <returns></returns>
		protected string GetLeftFormula(string format)
		{
			if (LeftExpression.IsError)
				return LeftExpression.ToString(format: format);
			else
				return LeftExpression.Value.ToString(format: format);
		}

		/// <summary>
		/// Формула правого алгебраического выражения.
		/// </summary>
		/// <param name="format">Описатели стандартного формата.</param>
		/// <returns></returns>
		protected string GetRightFormula(string format)
		{
			if (RightExpression.IsError)
				return RightExpression.Formula();
			else
				return RightExpression.Value.ToString(format: format);
		}

		/// <summary>
		/// Формула правого алгебраического выражения.
		/// </summary>
		/// <returns></returns>
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
		public override bool IsError => LeftExpression.IsError || RightExpression.IsError;

		/// <summary>
		/// Левая часть алгебраического выражения.
		/// </summary>
		public IExpression LeftExpression { get; }

		/// <summary>
		/// Правая часть алгебраического выражения.
		/// </summary>
		public IExpression RightExpression { get; }

		public static IExpression Create(ref Dictionary<string, ICell> cells, UnitCollection array)
		{
			int i = array.GetLastIndex();
			if (i > 0)
			{
				switch (array[i].UnitType)
				{
					case UnitCollection.MatchType.Addition:
						return AdditionExpression.Create(ref cells, UnitCollection.Create(array, 0, i), UnitCollection.Create(array, i + 1));
					case UnitCollection.MatchType.Division:
						return DivisionExpression.Create(ref cells, UnitCollection.Create(array, 0, i), UnitCollection.Create(array, i + 1));
					case UnitCollection.MatchType.Fix:
						return FixExpression.Create(ref cells, UnitCollection.Create(array, 0, i), UnitCollection.Create(array, i + 1));
					case UnitCollection.MatchType.Mod:
						return ModExpression.Create(ref cells, UnitCollection.Create(array, 0, i), UnitCollection.Create(array, i + 1));
					case UnitCollection.MatchType.Multiplication:
						return MultiplicationExpression.Create(ref cells, UnitCollection.Create(array, 0, i), UnitCollection.Create(array, i + 1));
					case UnitCollection.MatchType.Subtracting:
						return SubtractingExpression.Create(ref cells, UnitCollection.Create(array, 0, i), UnitCollection.Create(array, i + 1));
					case UnitCollection.MatchType.Power:
						return PowerExpression.Create(ref cells, UnitCollection.Create(array, 0, i), UnitCollection.Create(array, i + 1));
					case UnitCollection.MatchType.Sqrt:
						return SqrtExpression.Create(ref cells, UnitCollection.Create(array, 0, i), UnitCollection.Create(array, i + 1));
					default:
						return ErrorExpression.Create(array);
				}
			}
			else if (i == 0)
			{
				if (array.Count > 1)
				{
					switch (array.First.UnitType)
					{
						case UnitCollection.MatchType.Addition:
							return PositiveExpression.Create(ref cells, UnitCollection.Create(array, 1));
						case UnitCollection.MatchType.Subtracting:
							return NegativeExpression.Create(ref cells, UnitCollection.Create(array, 1));
						default:
							return ErrorExpression.Create(array);
					}
				}
				else { return ErrorExpression.Create(array); }
			}
			else if (i < 0)
			{
				return ErrorExpression.Create(array);
			}
			else
			{
				return Expression.Create(ref cells, array);
			}
		}
	}
}
