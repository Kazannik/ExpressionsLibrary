namespace ExpressionsLibrary.SelectExpressions
{
    /// <summary>
    /// Базовый класс выражения ветвления.
    /// </summary>
    abstract class ExpressionBase : ExpressionsLibrary.ExpressionBase
    {
        /// <summary>
        /// Значение секции Select (IF) выражения ветвления.
        /// </summary>
        public abstract bool IsTrue { get; }
    }
}
