using System.Collections.Generic;

namespace ExpressionsLibrary.ArithmeticExpressions
{
    /// <summary>
    /// Элемент положительного алгебраического выражения.
    /// </summary>
    class PositiveExpression : ExpressionBase, IExpression
    {
        private new IExpression expression;

        public static IExpression Create(ref Dictionary<string, ICell> cells, UnitCollection array)
        {
            return new PositiveExpression(ref cells, UnitCollection.Create(array));
        }

        private PositiveExpression(ref Dictionary<string, ICell> cells, UnitCollection array)
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

        public override string Formula(string format)
        {
            return @"+" + expression.Formula(format: format);
        }
    }
}
