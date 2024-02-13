using System;

namespace ExpressionsLibrary
{
    public class Expression : IExpression
    {
        private IExpression expression;
        
        private Expression(IExpression expression)
        {
            this.expression = expression;
        }

        public bool IsError
        {
            get
            {
                return expression.IsError;
            }
        }

        /// <summary>
        /// Определяемые пользователем данные, связанные с этим объектом.
        /// </summary>
        public object Tag { get; set; }

        public object objValue
        {
            get
            {
                return expression.objValue;
            }
        }

        public string Formula()
        {
            return expression.Formula();
        }

        public string ToString(string format)
        {
            return expression.ToString(format: format);
        }

        public static IExpression Create(string text)
        {
            bool isArithmetic = ArithmeticExpression.IsExpression(text: text);
            bool isBoolean = BooleanExpression.IsExpression(text: text);
            bool isLogic = LogicExpression.IsExpression(text: text);

            if (isBoolean)
                return new Expression(BooleanExpression.Create(text));
            else if (isLogic)
                return new Expression(LogicExpression.Create(text));
            else if (isArithmetic)
                return new Expression(ArithmeticExpression.Create(text));
            else
                throw new ArgumentException("Текстовая строка не содержит элементов выражения.");
            
        }

        public static IExpression Create(string text, string cellpattern)
        {
            bool isArithmetic = ArithmeticExpression.IsExpression(text: text);
            bool isBoolean = BooleanExpression.IsExpression(text: text);
            bool isLogic = LogicExpression.IsExpression(text: text);

            if (isBoolean)
                return new Expression(BooleanExpression.Create(text, cellpattern: cellpattern));
            else if (isLogic)
                return new Expression(LogicExpression.Create(text, cellpattern: cellpattern));
            else if (isArithmetic)
                return new Expression(ArithmeticExpression.Create(text, cellpattern: cellpattern));
            else
                throw new ArgumentException("Текстовая строка не содержит элементов выражения.");
        }        
    }
}
