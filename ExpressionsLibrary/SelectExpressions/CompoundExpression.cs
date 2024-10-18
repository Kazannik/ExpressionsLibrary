using ExpressionsLibrary.ArithmeticExpressions;
using System.Collections.Generic;

namespace ExpressionsLibrary.SelectExpressions
{
    /// <summary>
    /// Составное выражение ветвления.
    /// </summary>
    abstract class CompoundExpression : ExpressionBase
    {
        protected CompoundExpression(ref Dictionary<string, ICell> cells, UnitCollection select, UnitCollection thenAction) : base()
        {

            SelectExpression = (IBooleanExpression)ArithmeticExpressions.Expression.Create(ref cells, select);
            //ThenExpression = ExpressionsLibrary.Expression.Create(ref cells, thenAction);            
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
                
        /// <summary>
        /// Признак содержания ошибки в выражении.
        /// </summary>
        public override bool IsError
        {
            get { return SelectExpression.IsError || ThenExpression.IsError; }
        }

        public IBooleanExpression SelectExpression { get; }

        public IExpression ThenExpression { get; }
         

        public static IExpression Create(ref Dictionary<string, ICell> cells, UnitCollection array)
        {
            int i = array.GetLastIndex();
            int ifIndex = array.GetLastIndex(19);
            int thenIndex = array.GetLastIndex(18);
            int elseIndex = array.GetLastIndex(17);

            if (ifIndex == 0 && thenIndex > 3)
            {
                if (elseIndex > 0)
                {
                    return СomplexExpression.Create(ref cells, UnitCollection.Create(array, ifIndex, thenIndex - 1), UnitCollection.Create(array, thenIndex + 1, elseIndex - thenIndex), UnitCollection.Create(array, elseIndex + 1));
                }
                else
                {
                    return SimpleExpression.Create(ref cells, UnitCollection.Create(array, ifIndex + 1, thenIndex - 1), UnitCollection.Create(array, thenIndex + 1));
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
