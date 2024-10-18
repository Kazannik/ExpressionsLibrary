using ExpressionsLibrary.ArithmeticExpressions;
using System.Collections.Generic;

namespace ExpressionsLibrary.СomparisonExpressions
{ 

    /// <summary>
    /// Логическое выражение (БОЛЬШЕ).
    /// </summary>
    class MoreExpression : CompoundExpression, IBooleanExpression
    {
        private MoreExpression(ref Dictionary<string, ICell> cells, UnitCollection left, UnitCollection right) : base(ref cells, left, right) { }

        /// <summary>
        /// Положительное значение логического выражения.
        /// </summary>
        public override bool Value
        {
            get { return (LeftExpression.Value > RightExpression.Value); }
        }

        /// <summary>
        /// Строковое представление логического выражения.
        /// </summary>
        public override string Formula()
        {
            return LeftExpression.Formula() + ArithmeticExpression.SymbolSpace + СomparisonExpression.SymbolMore + ArithmeticExpression.SymbolSpace + RightExpression.Formula();
        }

        public override string Formula(string format)
        {
            return LeftExpression.Formula(format: format) + ArithmeticExpression.SymbolSpace + СomparisonExpression.SymbolMore + ArithmeticExpression.SymbolSpace + RightExpression.Formula(format: format);
        }

        /// <summary>
        /// Короткое строковое представление логического выражения.
        /// </summary>
        /// <param name="format">Формат отображения результата алгебраического выражения.</param>
        public override string ToString(string format)
        {
            if (IsFormat(format: format))
                return GetLeftFormula(format: format) + ArithmeticExpression.SymbolSpace + СomparisonExpression.SymbolMore + ArithmeticExpression.SymbolSpace + GetRightFormula(format: format);
            else
                return GetLeftFormula() + ArithmeticExpression.SymbolSpace + СomparisonExpression.SymbolMore + ArithmeticExpression.SymbolSpace + GetRightFormula();
        }

        public static IBooleanExpression Create(ref Dictionary<string, ICell> cells, UnitCollection left, UnitCollection right)
        {
            return new MoreExpression(ref cells, left, right);
        }
    }
}
