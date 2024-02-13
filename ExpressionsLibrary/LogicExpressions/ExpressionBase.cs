namespace ExpressionsLibrary.LogicExpressions
{
    /// <summary>
    /// Базовый класс логического выражений.
    /// </summary>
    abstract class ExpressionBase: ExpressionsLibrary.ExpressionBase, ILogicExpression
    {
        /// <summary>
        /// Значение логического выражения.
        /// </summary>
        public abstract bool Value { get; }
        
        public override object objValue { get { return Value; } }   
    }
}
