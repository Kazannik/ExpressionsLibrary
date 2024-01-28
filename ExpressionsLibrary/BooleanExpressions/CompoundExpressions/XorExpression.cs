﻿using System.Collections.Generic;

namespace ExpressionsLibrary.BooleanExpressions.CompoundExpressions
{
    /// <summary>
    /// Исключающее ИЛИ (XOR).
    /// </summary>
    class XorExpression : CompoundExpression
    {
        private XorExpression(ref Dictionary<string, ArithmeticExpressions.ICell> cells, UnitCollection left, UnitCollection right) : base(ref cells, left, right) { }

        /// <summary>
        /// Положительное значение логического выражения.
        /// </summary>
        public override bool Value
        {
            get
            {
                if (LeftExpression.Value || RightExpression.Value)
                    return true;
                else if (this.LeftExpression.Value == this.RightExpression.Value)
                    return false; 
                else
                    return true; 
            }
        }

        /// <summary>
        /// Строковое представление логического выражения.
        /// </summary>
        public override string Formula()
        {
            return LeftExpression.Formula() + ArithmeticExpression.SymbolSpace + BooleanExpression.SymbolXor + ArithmeticExpression.SymbolSpace + RightExpression.Formula();
        }

        /// <summary>
        /// Короткое строковое представление логического выражения.
        /// </summary>
        /// <param name="format">Формат отображения результата алгебраического выражения.</param>
        public override string ToString(string format)
        {
            if (IsFormat(format: format))
                return GetLeftFormula(format: format) + ArithmeticExpression.SymbolSpace + BooleanExpression.SymbolXor + ArithmeticExpression.SymbolSpace + GetRightFormula(format: format);
            else
                return GetLeftFormula() + ArithmeticExpression.SymbolSpace + BooleanExpression.SymbolXor + ArithmeticExpression.SymbolSpace + GetRightFormula();
        }

        public static XorExpression Create(ref Dictionary<string, ArithmeticExpressions.ICell> cells, UnitCollection left, UnitCollection right)
        {
            return new XorExpression(ref cells, left, right);
        }
    }
}
