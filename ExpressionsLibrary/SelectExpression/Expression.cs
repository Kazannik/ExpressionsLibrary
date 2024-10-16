using ExpressionsLibrary.ArithmeticExpressions;
using System.Collections.Generic;

namespace ExpressionsLibrary.SelectExpression
{
    /// <summary>
    /// Базовое выражение ветвения.
    /// </summary>
    abstract class Expression
    {
        public static IExpression Create(ref Dictionary<string, ICell> cells, UnitCollection array)
        {
            if (array.Count > 2 && array.IsLogic)
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
