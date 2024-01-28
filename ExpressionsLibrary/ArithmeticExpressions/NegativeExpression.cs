using System.Collections.Generic;

namespace ExpressionsLibrary.ArithmeticExpressions
{
    /// <summary>
    /// Элемент отрицательного алгебраического выражения.
    /// </summary>
    class NegativeExpression : ExpressionBase
    {
        private ExpressionBase expression;

        private NegativeExpression(ref Dictionary<string, ICell> cells, UnitCollection array)
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
        /// Отрицательное значение алгебраического выражения.
        /// </summary>
        public override decimal Value
        {
            get { return (expression.Value * -1); }
        }

        /// <summary>
        /// Строковое представление алгебраического выражения.
        /// </summary>
        public override string Formula()
        {
            return @"-" + expression.Formula();
        }

        public static NegativeExpression Create(ref Dictionary<string, ICell> cells, UnitCollection array)
        {
            return new NegativeExpression(ref cells, UnitCollection.Create(array));
        }
    }
}
