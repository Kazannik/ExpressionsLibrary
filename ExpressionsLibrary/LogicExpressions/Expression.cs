﻿using System.Collections.Generic;

namespace ExpressionsLibrary.LogicExpressions
{
    /// <summary>
    /// Базовое логическое выражение, объединяющее два алгебраических выражения.
    /// </summary>
    abstract class Expression
    {  
        public static ExpressionBase Create(ref Dictionary<string, ArithmeticExpressions.ICell> cells, UnitCollection array)
        {
            if (array.Count > 2 && array.IsLogic)
            { // Составное выражение.
                return CompoundExpression.Create(ref cells, array);
            }
            else
            { // Два и меньше елементов является ошибкой.
                return ErrorExpression.Create(array);
            }
        }     
    }
}