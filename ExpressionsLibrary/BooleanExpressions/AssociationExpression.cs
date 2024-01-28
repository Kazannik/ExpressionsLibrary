using System.Collections.Generic;

namespace ExpressionsLibrary.BooleanExpressions
{
    /// <summary>
    /// Логическое выражение, заключенное в скобки.
    /// </summary>
    class AssociationExpression : ExpressionBase
    {
        private LogicExpressions.ILogicExpression expression;

        private AssociationExpression(ref Dictionary<string, ArithmeticExpressions.ICell> cells, UnitCollection array)
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
        /// Значение логического выражения.
        /// </summary>
        public override bool Value
        {
            get { return (expression.Value); }
        }

        /// <summary>
        /// Строковое представление логического выражения.
        /// </summary>
        public override string Formula()
        {
            return @"(" + expression.Formula() + @")";
        }

        public static AssociationExpression Create(ref Dictionary<string, ArithmeticExpressions.ICell> cells, UnitCollection array)
        {
            return new AssociationExpression(ref cells, UnitCollection.Create(array, 1, array.Count - 2));
        }

        /// <summary>
        /// Короткое строковое представление логического выражения.
        /// </summary>
        /// <param name="format">Формат отображения результата алгебраического выражения.</param>
        public override string ToString(string format)
        {
            return Formula();
        }
    }
}
