using ExpressionsLibrary.ArithmeticExpressions;
using System.Collections.Generic;

namespace ExpressionsLibrary.SelectExpression
{
    /// <summary>
    /// Составное выражение ветвления.
    /// </summary>
    abstract class CompoundExpression : ExpressionBase
    {
        protected CompoundExpression(ref Dictionary<string, ICell> cells, UnitCollection select, UnitCollection thenAction, UnitCollection elseAction) : base()
        {
            SelectExpression = ArithmeticExpressions.Expression.Create(ref cells, select);
            ThenExpression = ArithmeticExpressions.Expression.Create(ref cells, thenAction);
            ElseExpression = ArithmeticExpressions.Expression.Create(ref cells, elseAction);
        }

        protected string GetSelectFormula()
        {
            return SelectExpression.ToString();           
        }

        protected string GetSelectFormula(string format)
        {
            return SelectExpression.ToString(format: format);            
        }

        protected string GetThenFormula(string format)
        {
            return ThenExpression.Formula();
        }

        protected string GetThenFormula()
        {
            return ThenExpression.Formula();
        }

        protected string GetElseFormula(string format)
        {
            return ElseExpression.Formula();
        }

        protected string GetElseFormula()
        {
            return ElseExpression.Formula();
        }

        /// <summary>
        /// Признак содержания ошибки в выражении.
        /// </summary>
        public override bool IsError
        {
            get { return SelectExpression.IsError || ThenExpression.IsError || ElseExpression.IsError; }
        }

        /// <summary>
        /// Левая часть алгебраического выражения.
        /// </summary>
        public IExpression SelectExpression { get; }

        /// <summary>
        /// Правая часть алгебраического выражения.
        /// </summary>
        public IExpression ThenExpression { get; }

        public IExpression ElseExpression { get; }

        public static IExpression Create(ref Dictionary<string, ICell> cells, UnitCollection array)
        {
            int i = array.GetLastIndex();
            if (i > 0)
            {
                switch (array[i].UnitType)
                {
                    //case UnitCollection.MatchType.Equal:
                    //    return EqualExpression.Create(ref cells, UnitCollection.Create(array, 0, i), UnitCollection.Create(array, i + 1));
                    //case UnitCollection.MatchType.Less:
                    //    return LessExpression.Create(ref cells, UnitCollection.Create(array, 0, i), UnitCollection.Create(array, i + 1));
                    //case UnitCollection.MatchType.LessOrEqual:
                    //    return LessOrEqualExpression.Create(ref cells, UnitCollection.Create(array, 0, i), UnitCollection.Create(array, i + 1));
                    //case UnitCollection.MatchType.More:
                    //    return MoreExpression.Create(ref cells, UnitCollection.Create(array, 0, i), UnitCollection.Create(array, i + 1));
                    //case UnitCollection.MatchType.MoreOrEqual:
                    //    return MoreOrEqualExpression.Create(ref cells, UnitCollection.Create(array, 0, i), UnitCollection.Create(array, i + 1));
                    //case UnitCollection.MatchType.NotEqual:
                    //    return NotEqualExpression.Create(ref cells, UnitCollection.Create(array, 0, i), UnitCollection.Create(array, i + 1));
                    default:
                        return ErrorExpression.Create(array);
                }
            }
            else if (i == -2)
            {
                return ErrorExpression.Create(array);
            }
            else
            {
                return Expression.Create(ref cells, array);
            }
        }
    }
}
