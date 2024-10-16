using System.Collections.Generic;

namespace ExpressionsLibrary.ArithmeticExpressions
{
    /// <summary>
    /// Элемент отрицательного алгебраического выражения.
    /// </summary>
    class NegativeExpression : ExpressionBase, IExpression
    {
        private new IExpression expression;

        private NegativeExpression(ref Dictionary<string, ICell> cells, UnitCollection array)
        {
            expression = Expression.Create(ref cells, array);
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

        public static IExpression Create(ref Dictionary<string, ICell> cells, UnitCollection array)
        {
            return new NegativeExpression(ref cells, UnitCollection.Create(array));
        }
    }
}
