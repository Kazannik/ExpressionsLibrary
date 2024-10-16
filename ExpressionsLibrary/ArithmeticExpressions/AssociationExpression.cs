using System.Collections.Generic;

namespace ExpressionsLibrary.ArithmeticExpressions
{
    /// <summary>
    /// Алгебраическое выражение, заключенное в скобки.
    /// </summary>
    class AssociationExpression : ExpressionBase, IExpression
    {
        private new IExpression expression;

        private AssociationExpression(ref Dictionary<string, ICell> cells, UnitCollection array)
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
        /// Значение алгебраического выражения.
        /// </summary>
        public override decimal Value
        {
            get { return (expression.Value); }
        }

        /// <summary>
        /// Строковое представление алгебраического выражения.
        /// </summary>
        public override string Formula()
        {
            return @"(" + expression.Formula() + @")";
        }

        public static IExpression Create(ref Dictionary<string, ICell> cells, UnitCollection array)
        {
            return new AssociationExpression(ref cells, UnitCollection.Create(array, 1, array.Count - 2));
        }
    }
}
