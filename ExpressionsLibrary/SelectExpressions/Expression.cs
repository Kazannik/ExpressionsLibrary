using ExpressionsLibrary.ArithmeticExpressions;
using System.Collections.Generic;

namespace ExpressionsLibrary.SelectExpressions
{
    /// <summary>
    /// Базовое выражение ветвения.
    /// </summary>
    abstract class Expression
    {
        public static IExpression Create(ref Dictionary<string, ICell> cells, UnitCollection array)
        {
            if (array.Count >= 8 && array.IsSelect)
            { 
                return CompoundExpression.Create(ref cells, array);
            }
            else
            { 
                return ErrorExpression.Create(array);
            }
        }
    }
}
