using System;
using System.Collections.Generic;

namespace ExpressionsLibrary.ArithmeticExpressions
{
    /// <summary>
    /// Элемент положительного алгебраического выражения.
    /// </summary>
    class PositiveExpression : ExpressionBase
    {
        private ExpressionBase expression;

        public static PositiveExpression Create(ref Dictionary<string, ICell> cells, UnitCollection array)
        {
            return new PositiveExpression(ref cells, UnitCollection.Create(array));
        }

        private PositiveExpression(ref Dictionary<string, ICell> cells, UnitCollection array)
        {
            this.expression = Expression.Create(ref cells, array);
        }

        /// <summary>
        /// Признак содержания ошибки в выражении.
        /// </summary>
        public override bool IsError
        {
            get { return expression.IsError; }
        }

        /// <summary>
        /// Положительное значение алгебраического выражения.
        /// </summary>
        public override decimal Value
        {
            get { return (+1 * expression.Value); }
        }

        /// <summary>
        /// Строковое представление алгебраического выражения.
        /// </summary>
        public override string Formula()
        {
            return @"+" + expression.Formula();
        }
    }
}
