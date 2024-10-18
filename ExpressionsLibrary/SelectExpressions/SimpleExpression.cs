using ExpressionsLibrary.ArithmeticExpressions;
using System.Collections.Generic;

namespace ExpressionsLibrary.SelectExpressions
{
    class SimpleExpression : CompoundExpression, ISelectExpression
    {
        protected SimpleExpression(ref Dictionary<string, ICell> cells, UnitCollection select, UnitCollection thenAction) : base(ref cells, select, thenAction) { }

        /// <summary>
        /// Значение секции Select (IF) выражения ветвления.
        /// </summary>
        public override bool IsTrue
        {
            get
            {
                return SelectExpression.Value;
            }
        }


        /// <summary>
        /// Положительное значение логического выражения.
        /// </summary>
        public override object objValue
        {
            get
            {
                if (IsTrue)
                    return ThenExpression.objValue;
                else
                    return string.Empty;
            }
        }

        /// <summary>
        /// Строковое представление выражения ветвления.
        /// </summary>
        public override string Formula()
        {
            return SelectExpression.Formula() + ArithmeticExpression.SymbolSpace + ThenExpression.Formula();
        }

        public override string Formula(string format)
        {
            return SelectExpression.Formula(format: format) + ArithmeticExpression.SymbolSpace + ThenExpression.Formula(format: format);
        }

        /// <summary>
        /// Короткое строковое представление логического выражения.
        /// </summary>
        /// <param name="format">Формат отображения результата алгебраического выражения.</param>
        public override string ToString(string format)
        {
            if (IsFormat(format: format))
                return GetSelectFormula(format: format) + ArithmeticExpression.SymbolSpace + GetThenFormula(format: format);
            else
                return GetSelectFormula() + ArithmeticExpression.SymbolSpace + GetThenFormula();
        }

        public static ISelectExpression Create(ref Dictionary<string, ICell> cells, UnitCollection select, UnitCollection thenAction)
        {
            return new SimpleExpression(ref cells, select, thenAction);
        }
    }
}
