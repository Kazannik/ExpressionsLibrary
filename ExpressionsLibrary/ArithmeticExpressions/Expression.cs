using System.Collections.Generic;

namespace ExpressionsLibrary.ArithmeticExpressions
{
    /// <summary>
    /// Конструктор алгебраических выражений.
    /// </summary>
    abstract class Expression
    {
        public static IDecimalExpression Create(ref Dictionary<string, ICell> cells, UnitCollection array)
        {
            if (array.Count == 0 || array.IsError)
            { 
                // Элементы отсуствуют.
                return ErrorExpression.Create(array);
            }
            else if (array.Count == 1 && array.First.UnitType == UnitCollection.MatchType.Decimal)
            { 
                // Если обычное число (количество знаков составляет один, а его тип равен Decimal).
                return ValueExpression.Create(array[0].Value);
            }
            else if (array.Count == 1 && array.First.UnitType == UnitCollection.MatchType.Cell)
            { 
                // Если ссылка на ячейку (количество знаков составляет один, а его тип равен Cell).
                string key = array[0].Value;
                if (cells.ContainsKey(key))
                {
                    return cells[key];
                }
                else
                {
                    IDecimalExpression cell = CellExpression.Create(key);
                    cells.Add(key, (ICell)cell);
                    return cell;
                }
            }
            else if (array.Count == 1)
            { 
                // Количество знаков составляет один, но это не число.
                return ErrorExpression.Create(array);
            }
            else if (array.Count == 2 && array.Last.UnitType == UnitCollection.MatchType.Decimal)
            { 
                // Проверяем, что коллекция состоит из двух знаков, второй из которых число.
                if (array.First.UnitType == UnitCollection.MatchType.Addition)
                { 
                    // Если первый знак является плюсом.
                    return PositiveExpression.Create(ref cells, UnitCollection.Create(array, 1));
                }
                else if (array.First.UnitType == UnitCollection.MatchType.Subtracting)
                { 
                    // Если первый знак является минусом.
                    return NegativeExpression.Create(ref cells, UnitCollection.Create(array, 1));
                }
                else
                { 
                    // Если первый знак не является ни плюсом ни минусом.
                    return ErrorExpression.Create(array);
                }
            }
            else if (array.IsAssociation)
            { 
                // Если выражение заключено в скобки.
                return AssociationExpression.Create(ref cells, UnitCollection.Create(array));
            }
            else if (array.IsPositiveAssociation)
            { 
                // Положительное выражение, заключенное в скобки.
                return PositiveExpression.Create(ref cells, UnitCollection.Create(array, 1));
            }
            else if (array.IsNegativeAssociation)
            { 
                // Отрицательное выражение, заключенное в скобки.
                return NegativeExpression.Create(ref cells, UnitCollection.Create(array, 1));
            }
            else
            { 
                // Составное выражение.
                return CompoundExpressions.Expression.Create(ref cells, array);
            }
        }
    }
}
