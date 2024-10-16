using System.Collections.Generic;

namespace ExpressionsLibrary.BooleanExpressions
{
    /// <summary>
    /// Противоположное логическое выражение.
    /// </summary>
    class NotExpression : ExpressionBase, LogicExpressions.ILogicExpression
    {
        private new LogicExpressions.ILogicExpression expression;

        public static LogicExpressions.ILogicExpression Create(ref Dictionary<string, ArithmeticExpressions.ICell> cells, UnitCollection array)
        {
            return new NotExpression(ref cells, UnitCollection.Create(array));
        }

        private NotExpression(ref Dictionary<string, ArithmeticExpressions.ICell> cells, UnitCollection array)
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
