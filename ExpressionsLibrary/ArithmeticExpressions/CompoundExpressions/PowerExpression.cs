using System;
using System.Collections.Generic;

namespace ExpressionsLibrary.ArithmeticExpressions.CompoundExpressions
{
    /// <summary>
    /// Число, возведенное в степень.
    /// </summary>
    class PowerExpression : CompoundExpression, IExpression
    {
        private PowerExpression(ref Dictionary<string, ICell> cells, UnitCollection left, UnitCollection right) : base(ref cells, left, right) { }

        /// <summary>
        /// Значение алгебраического выражения.
        /// </summary>
        public override decimal Value
        {
            get { return (decimal)Math.Pow((double)LeftExpression.Value, (double)RightExpression.Value); }
        }

        /// <summary>
        /// Строковое представление алгебраического выражения.
        /// </summary>
        public override string Formula()
        {
            return LeftExpression.Formula() + ArithmeticExpression.SymbolSpace + ArithmeticExpression.SymbolPower + ArithmeticExpression.SymbolSpace + RightExpression.Formula();
        }

        /// <summary>
        /// Короткое строковое представление выражения.
        /// </summary>
        /// <param name="format">Формат отображения результата алгебраического выражения. Описатели стандартного формата.</param>
        public override string ToString(string format)
        {
            if (IsFormat(format: format))
                return GetLeftFormula(format: format) + ArithmeticExpression.SymbolSpace + ArithmeticExpression.SymbolPower + ArithmeticExpression.SymbolSpace + GetRightFormula(format: format);
            else
                return GetLeftFormula() + ArithmeticExpression.SymbolSpace + ArithmeticExpression.SymbolPower + ArithmeticExpression.SymbolSpace + GetRightFormula();
        }

        public static IExpression Create(ref Dictionary<string, ICell> cells, UnitCollection left, UnitCollection right)
        {
            return new PowerExpression(ref cells, left, right);
        }
    }
}
