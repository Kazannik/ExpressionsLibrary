using System;

namespace ExpressionsLibrary
{
    /// <summary>
    /// Математическое выражение.
    /// </summary>
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

        /// <summary>
        /// Значение выражения.
        /// </summary>
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


        /// <summary>
        /// Определяет содержится ли ячейка с указанным ключем в выражении.
        /// </summary>
        /// <param name="key">Ключ ячейки.</param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            return expression.Contains(key);
        }

        /// <summary>
        /// Коллекция ключей ячеек.
        /// </summary>
        public string[] Keys
        {
            get { return expression.Keys; }
        }

        /// <summary>
        /// Ячейка, используемая при расчете.
        /// </summary>
        /// <param name="key">Ключ ячейки.</param>
        /// <returns></returns>
        public ArithmeticExpressions.ICell this[string key]
        {
            get { return expression[key]; }
        }

        /// <summary>
        /// Количество ячеек, используемых при расчете.
        /// </summary>
        public int Count
        {
            get { return expression.Count; }
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
