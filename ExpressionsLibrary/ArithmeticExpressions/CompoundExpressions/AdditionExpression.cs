using System.Collections.Generic;

namespace ExpressionsLibrary.ArithmeticExpressions.CompoundExpressions
{
    /// <summary>
    /// Алгебраическое выражение сложения.
    /// </summary>
    class AdditionExpression : Expression, IDecimalExpression
    {
        private AdditionExpression(ref Dictionary<string, ICell> cells, UnitCollection left, UnitCollection right) : base(ref cells, left, right) { }

        /// <summary>
        /// Значение алгебраического выражения.
        /// </summary>
        public override decimal Value
        {
            get { return LeftExpression.Value + RightExpression.Value; }
        }

        /// <summary>
        /// Строковое представление алгебраического выражения.
        /// </summary>
        public override string Formula()
        {
            return LeftExpression.Formula() + ArithmeticExpression.SymbolSpace + ArithmeticExpression.SymbolAddition + ArithmeticExpression.SymbolSpace + RightExpression.Formula();
        }

        public override string Formula(string format)
        {
            return LeftExpression.Formula(format: format) + ArithmeticExpression.SymbolSpace + ArithmeticExpression.SymbolAddition + ArithmeticExpression.SymbolSpace + RightExpression.Formula(format: format);
        }

        /// <summary>
        /// Короткое строковое представление выражения.
        /// </summary>
        /// <param name="format">Формат отображения результата алгебраического выражения. Описатели стандартного формата.</param>
        public override string ToString(string format)
        {
            if (IsFormat(format: format))
            {
                return GetLeftFormula(format: format) + ArithmeticExpression.SymbolSpace + ArithmeticExpression.SymbolAddition + ArithmeticExpression.SymbolSpace + GetRightFormula(format: format);
            }
            else
            {
                return GetLeftFormula() + ArithmeticExpression.SymbolSpace + ArithmeticExpression.SymbolAddition + ArithmeticExpression.SymbolSpace + GetRightFormula();
            }
        }

        public static IDecimalExpression Create(ref Dictionary<string, ICell> cells, UnitCollection left, UnitCollection right)
        {
            return new AdditionExpression(ref cells, left, right);
        }
    }
}
