namespace ExpressionsLibrary.LogicExpressions
{
    public interface ILogicExpression: IExpression
    {
        /// <summary>
        /// Значение логического выражения.
        /// </summary>
        bool Value { get; }    
    }
}
