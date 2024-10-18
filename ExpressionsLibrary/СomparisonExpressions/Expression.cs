using ExpressionsLibrary.ArithmeticExpressions;
using System.Collections.Generic;

namespace ExpressionsLibrary.СomparisonExpressions
{
    /// <summary>
    /// Базовое выражение сравнения, объединяющее два алгебраических выражения.
    /// </summary>
    abstract class Expression
    {
        public static IBooleanExpression Create(ref Dictionary<string, ICell> cells, UnitCollection array)
        {
            if (array.Count > 2 && array.IsLogic)
            { 
                // Составное выражение.
                return CompoundExpression.Create(ref cells, array);
            }
            else
            { 
                // Два и меньше элементов является ошибкой.
                return ErrorExpression.Create(array);
            }
        }
    }
}
