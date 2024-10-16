using System.Collections.Generic;

namespace ExpressionsLibrary.ArithmeticExpressions.CompoundExpressions
{
    /// <summary>
    /// Алгебраическое выражение вычитания.
    /// </summary>
    class SubtractingExpression : CompoundExpression, IExpression
    {
        private SubtractingExpression(ref Dictionary<string, ICell> cells, UnitCollection left, UnitCollection right) : base(ref cells, left, right) { }

        /// <summary>
        /// Значение алгебраического выражения.
        /// </summary>
        public override decimal Value
        {
            get { return LeftExpression.Value - RightExpression.Value; }
        }

        /// <summary>
        /// Строковое представление алгебраического выражения.
        /// </summary>
        public override string Formula()
        {
            return LeftExpression.Formula() + ArithmeticExpression.SymbolSpace + ArithmeticExpression.SymbolSubtracting + ArithmeticExpression.SymbolSpace + RightExpression.Formula();
        }

        /// <summary>
        /// Короткое строковое представление выражения.
        /// </summary>
        /// <param name="format">Формат отображения результата алгебраического выражения. Описатели стандартного формата.</param>
        public override string ToString(string format)
        {
            if (IsFormat(format: format))
                return GetLeftFormula(format: format) + ArithmeticExpression.SymbolSpace + ArithmeticExpression.SymbolSubtracting + ArithmeticExpression.SymbolSpace + GetRightFormula(format: format);
            else
                return GetLeftFormula() + ArithmeticExpression.SymbolSpace + ArithmeticExpression.SymbolSubtracting + ArithmeticExpression.SymbolSpace + GetRightFormula();
        }

        public static IExpression Create(ref Dictionary<string, ICell> cells, UnitCollection left, UnitCollection right)
        {
            return new SubtractingExpression(ref cells, left, right);
        }
    }
}
