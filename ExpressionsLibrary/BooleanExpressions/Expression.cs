using System.Collections.Generic;

namespace ExpressionsLibrary.BooleanExpressions
{
	/// <summary>
	/// Базовое логическое выражение, объединяющее два алгебраических выражения.
	/// </summary>
	abstract class Expression
	{
		public static LogicExpressions.ILogicExpression Create(ref Dictionary<string, ICell> cells, UnitCollection array)
		{
			if (array.Count == 0)
			{ // Элементы отсутствуют.
				return ErrorExpression.Create(array);
			}
			else if (array.Count == 1 && array.First.UnitType == UnitCollection.MatchType.True)
			{ // Если обычное число (количество знаков составляет один, а его тип равен True).
				return TrueExpression.Create();
			}
			else if (array.Count == 1 && array.First.UnitType == UnitCollection.MatchType.False)
			{ // Если ссылка на ячейку (количество знаков составляет один, а его тип равен False).
				return FalseExpression.Create();
			}
			else if (array.Count == 1)
			{ // Количество знаков составляет один, но это не логическое условие True или False.
				return ErrorExpression.Create(array);
			}
			else if (array.Count > 1 && array.First.UnitType == UnitCollection.MatchType.Not)
			{
				return NotExpression.Create(ref cells, UnitCollection.Create(array, 1));
			}
			else if (array.IsAssociation)
			{ // Если выражение заключено в скобки.
				return AssociationExpression.Create(ref cells, UnitCollection.Create(array));
			}
			else if (array.IsNotAssociation)
			{ // Отрицательное выражение, заключенное в скобки.
				return NotExpression.Create(ref cells, UnitCollection.Create(array, 1));
			}
			else if (array.Count > 2 && array.IsBoolean)
			{ // Составное выражение.
				return CompoundExpressions.CompoundExpression.Create(ref cells, array);
			}
			else
			{ // Может это логическое выражение.
				return LogicExpressions.Expression.Create(ref cells, array);
			}
		}
	}
}
