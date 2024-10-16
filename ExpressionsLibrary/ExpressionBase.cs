using ExpressionsLibrary.ArithmeticExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ExpressionsLibrary
{
    /// <summary>
    /// Базовый класс выражений.
    /// </summary>
    abstract class ExpressionBase : IExpression
    {
        protected IExpression expression;

        protected Dictionary<string, ICell> collection;

        protected ExpressionBase()
        {
            collection = new Dictionary<string, ICell>();
        }

        /// <summary>
        /// Признак содержания ошибки в выражении.
        /// </summary>
        public abstract bool IsError { get; }

        /// <summary>
        /// Значение выражения.
        /// </summary>
        public abstract object objValue { get; }

        /// <summary>
        /// Строковое представление выражения.
        /// </summary>
        public abstract string Formula();

        /// <summary>
        /// Строковое представление выражения.
        /// </summary>
        /// <param name="format">Строка, описывающая формат отображения результата алгебраического выражения.</param>
        public abstract string Formula(string format);

        /// <summary>
        /// Короткое строковое представление выражения.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString(format: string.Empty);
        }

        /// <summary>
        /// Короткое строковое представление выражения.
        /// </summary>
        /// <param name="format">Строка, описывающая формат отображения результата алгебраического выражения.</param>
        /// <returns></returns>
        public abstract string ToString(string format);

        /// <summary>
        /// Признак применения формата.
        /// </summary>
        /// <param name="format">Строка, описывающая формат отображения результата алгебраического выражения.</param>
        /// <returns></returns>
        protected static bool IsFormat(string format)
        {
            return !string.IsNullOrWhiteSpace(format);
        }

        /// <summary>
        /// Определяет содержится ли ячейка с указанным ключем в выражении.
        /// </summary>
        /// <param name="key">Ключ ячейки.</param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            return collection.ContainsKey(key);
        }

        /// <summary>
        /// Коллекция ключей ячеек.
        /// </summary>
        public string[] Keys
        {
            get { return collection.Keys.ToArray(); }
        }

        /// <summary>
        /// Ячейка, используемая при расчете.
        /// </summary>
        /// <param name="key">Ключ ячейки.</param>
        /// <returns></returns>
        public ICell this[string key]
        {
            get { return collection[key]; }
        }

        /// <summary>
        /// Количество ячеек, используемых при расчете.
        /// </summary>
        public int Count
        {
            get { return collection.Count; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<ICell> GetEnumerator()
        {
            return collection.Values.GetEnumerator();
        }
    }
}
