using System;
using System.Collections;
using System.Collections.Generic;
using ExpressionsLibrary.ArithmeticExpressions;
using static ExpressionsLibrary.UnitCollection;

namespace ExpressionsLibrary
{
    /// <summary>
    /// Выражение.
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

        public string Formula(string format)
        {
            return expression.Formula(format: format);
        }

        public string ToString(string format)
        {
            return expression.ToString(format: format);
        }

        /// <summary>
        /// Определяет содержится ли ячейка с указанным ключем в выражении.
        /// </summary>
        /// <param name="key">Ключ ячейки.</param>
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
        public ICell this[string key]
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

        IEnumerator<ICell> IEnumerable<ICell>.GetEnumerator()
        {
            return expression.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return expression.GetEnumerator();
        }

        public static IExpression Create(string text)
        {
            UnitCollection collection = UnitCollection.Create(text: text);
            return Create(collection: collection);                
        }

        public static IExpression Create(string text, string cellpattern)
        {
            UnitCollection collection = UnitCollection.Create(text: text, cellpattern: cellpattern);
            return Create(collection: collection);
        }

        internal static IExpression Create(UnitCollection collection)
        {
            int i = collection.GetLastIndex();
            ActionType action = collection[i].Action;
            switch (action)
            {
                case ActionType.Arithmetic:
                    return new Expression(ArithmeticExpression.Create(collection: collection));
                case ActionType.Сomparison:
                    return new Expression(СomparisonExpression.Create(collection: collection));
                case ActionType.Boolean:
                    return new Expression(BooleanExpression.Create(collection: collection));
                case ActionType.Select:
                    return new Expression(SelectExpression.Create(collection: collection));
                default:
                    throw new ArgumentException("Текстовая строка не содержит элементов выражения.");
            }
        }
    }
}
