namespace ExpressionsLibrary.SelectExpression
{
    /// <summary>
    /// Базовый класс выражения ветвления.
    /// </summary>
    abstract class ExpressionBase : ExpressionsLibrary.ExpressionBase
    {
        public override object objValue { get; }
    }
}
