using ExpressionsLibrary.ArithmeticExpressions;
using System.Collections.Generic;

namespace ExpressionsLibrary.SelectExpressions
{
    class СomplexExpression : SimpleExpression, ISelectExpression
    {
        private СomplexExpression(ref Dictionary<string, ICell> cells, UnitCollection select, UnitCollection thenAction, UnitCollection elseAction) : base(ref cells, select, thenAction)
        {
            ElseExpression = ArithmeticExpressions.Expression.Create(ref cells, elseAction);
        }

        public IExpression ElseExpression { get; }

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
        /// Значение выражения ветвления.
        /// </summary>
        public override object objValue
        {
            get
            {
                if (IsTrue)
                    return ThenExpression.objValue;
                else
                    return ElseExpression.objValue;
            }
        }

        /// <summary>
        /// Строковое представление логического выражения.
        /// </summary>
        public override string Formula()
        {
            return base.Formula() + ArithmeticExpression.SymbolSpace + ElseExpression.Formula();
        }

        public override string Formula(string format)
        {
            return base.Formula(format: format) + ArithmeticExpression.SymbolSpace + ElseExpression.Formula(format: format);
        }

        /// <summary>
        /// Короткое строковое представление логического выражения.
        /// </summary>
        /// <param name="format">Формат отображения результата алгебраического выражения.</param>
        public override string ToString(string format)
        {
            if (IsFormat(format: format))
                return base.ToString(format: format) + ArithmeticExpression.SymbolSpace + GetElseFormula(format: format);
            else
                return base.ToString() + ArithmeticExpression.SymbolSpace + GetElseFormula();
        }

        public static ISelectExpression Create(ref Dictionary<string, ICell> cells, UnitCollection select, UnitCollection thenAction, UnitCollection elseAction)
        {
            return new СomplexExpression(ref cells, select, thenAction, elseAction);
        }
    }
}
