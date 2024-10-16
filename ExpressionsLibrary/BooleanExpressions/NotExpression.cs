using ExpressionsLibrary.ArithmeticExpressions;
using ExpressionsLibrary.LogicExpressions;
using System.Collections.Generic;

namespace ExpressionsLibrary.BooleanExpressions
{
    /// <summary>
    /// Противоположное логическое выражение.
    /// </summary>
    class NotExpression : ExpressionBase, ILogicExpression
    {
        private new ILogicExpression expression;

        public static ILogicExpression Create(ref Dictionary<string, ICell> cells, UnitCollection array)
        {
            return new NotExpression(ref cells, UnitCollection.Create(array));
        }

        private NotExpression(ref Dictionary<string, ICell> cells, UnitCollection array)
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
        /// Положительное значение логического выражения.
        /// </summary>
        public override bool Value
        {
            get { return (!expression.Value); }
        }

        /// <summary>
        /// Строковое представление логического выражения.
        /// </summary>
        public override string Formula()
        {
            return BooleanExpression.SymbolNot + ArithmeticExpression.SymbolSpace + expression.Formula();
        }

        public override string Formula(string format)
        {
            return BooleanExpression.SymbolNot + ArithmeticExpression.SymbolSpace + expression.Formula(format: format);
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
