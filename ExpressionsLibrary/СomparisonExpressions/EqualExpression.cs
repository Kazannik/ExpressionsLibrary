using ExpressionsLibrary.ArithmeticExpressions;
using System.Collections.Generic;

namespace ExpressionsLibrary.СomparisonExpressions
{
    /// <summary>
    /// Выражение равенства (РАВНО).
    /// </summary>
    class EqualExpression : CompoundExpression, IBooleanExpression
    {
        private EqualExpression(ref Dictionary<string, ICell> cells, UnitCollection left, UnitCollection right) : base(ref cells, left, right) { }

        /// <summary>
        /// Значение сравнения равенства.
        /// </summary>
        public override bool Value
        {
            get { return (LeftExpression.Value == RightExpression.Value); }
        }

        /// <summary>
        /// Строковое представление выражения сравнения.
        /// </summary>
        public override string Formula()
        {
            return LeftExpression.Formula() + ArithmeticExpression.SymbolSpace + СomparisonExpression.SymbolEqual + ArithmeticExpression.SymbolSpace + RightExpression.Formula();
        }

        public override string Formula(string format)
        {
            return LeftExpression.Formula(format: format) + ArithmeticExpression.SymbolSpace + СomparisonExpression.SymbolEqual + ArithmeticExpression.SymbolSpace + RightExpression.Formula(format: format);
        }

        /// <summary>
        /// Короткое строковое представление выражения сравнения.
        /// </summary>
        /// <param name="format">Формат отображения результата алгебраического выражения.</param>
        public override string ToString(string format)
        {
            if (IsFormat(format: format))
                return GetLeftFormula(format: format) + ArithmeticExpression.SymbolSpace + СomparisonExpression.SymbolEqual + ArithmeticExpression.SymbolSpace + GetRightFormula(format: format);
            else
                return GetLeftFormula() + ArithmeticExpression.SymbolSpace + СomparisonExpression.SymbolEqual + ArithmeticExpression.SymbolSpace + GetRightFormula();
        }

        public static IBooleanExpression Create(ref Dictionary<string, ICell> cells, UnitCollection left, UnitCollection right)
        {
            return new EqualExpression(ref cells, left, right);
        }
    }
}
