using System;
using System.Collections.Generic;

namespace ExpressionsLibrary.ArithmeticExpressions.CompoundExpressions
{
    /// <summary>
    /// Корень числа.
    /// </summary>
    class SqrtExpression : CompoundExpression, IExpression
    {
        private SqrtExpression(ref Dictionary<string, ICell> cells, UnitCollection left, UnitCollection right) : base(ref cells, left, right) { }

        /// <summary>
        /// Значение алгебраического выражения.
        /// </summary>
        public override decimal Value
        {
            get
            {
                decimal right = RightExpression.Value;
                if (right != 0)
                    return (decimal)Math.Pow((double)LeftExpression.Value, (double)(1 / RightExpression.Value));
                else
                    return 0;
            }
        }
        /// <summary>
        /// Признак содержания ошибки в выражении.
        /// </summary>
        public override bool IsError
        {
            get { return (RightExpression.Value == 0) || LeftExpression.IsError || RightExpression.IsError; }
        }
        /// <summary>
        /// Строковое представление алгебраического выражения.
        /// </summary>
        public override string Formula()
        {
            if (RightExpression.Value == 0 && !RightExpression.IsError)
                return LeftExpression.Formula() + ArithmeticExpression.SymbolSpace + ArithmeticExpression.SymbolSqrt + ArithmeticExpression.SymbolSpace + ArithmeticExpression.SymbolStartError + RightExpression.Formula() + ArithmeticExpression.SymbolEndError;
            else
                return LeftExpression.Formula() + ArithmeticExpression.SymbolSpace + ArithmeticExpression.SymbolSqrt + ArithmeticExpression.SymbolSpace + RightExpression.Formula();
        }

        /// <summary>
        /// Короткое строковое представление выражения.
        /// </summary>
        /// <param name="format">Формат отображения результата алгебраического выражения. Описатели стандартного формата.</param>
        public override string ToString(string format)
        {
            if (IsFormat(format: format))
                return GetLeftFormula(format: format) + ArithmeticExpression.SymbolSpace + ArithmeticExpression.SymbolSqrt + ArithmeticExpression.SymbolSpace + GetRightFormula(format: format);
            else
                return GetLeftFormula() + ArithmeticExpression.SymbolSpace + ArithmeticExpression.SymbolSqrt + ArithmeticExpression.SymbolSpace + GetRightFormula();
        }

        public static IExpression Create(ref Dictionary<string, ICell> cells, UnitCollection left, UnitCollection right)
        {
            return new SqrtExpression(ref cells, left, right);
        }
    }
}
