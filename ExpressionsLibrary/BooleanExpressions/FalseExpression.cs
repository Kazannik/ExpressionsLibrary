using System.Collections.Generic;

namespace ExpressionsLibrary.BooleanExpressions
{
    /// <summary>
    /// Отрицательное логическое выражение.
    /// </summary>
    class FalseExpression : ExpressionBase, LogicExpressions.ILogicExpression
    {
        public static LogicExpressions.ILogicExpression Create()
        {
            return new FalseExpression();
        }

        private FalseExpression()
        {
            IsError = false;
            Value = false;
        }

        /// <summary>
        /// Признак содержания ошибки в выражении.
        /// </summary>
        public override bool IsError { get; }

        /// <summary>
        /// Положительное значение логического выражения.
        /// </summary>
        public override bool Value { get; }

        /// <summary>
        /// Строковое представление логического выражения.
        /// </summary>
        public override string Formula()
        {
            return BooleanExpression.SymbolFalse;
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
