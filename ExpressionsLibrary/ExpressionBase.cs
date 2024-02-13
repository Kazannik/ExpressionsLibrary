using System.Collections.Generic;
using System.Linq;

namespace ExpressionsLibrary
{
    /// <summary>
    /// Базовый класс выражений.
    /// </summary>
    abstract class ExpressionBase: IExpression
    {
        protected IExpression expression;

        protected Dictionary<string, ArithmeticExpressions.ICell> collection;

        protected ExpressionBase()
        {
            collection = new Dictionary<string, ArithmeticExpressions.ICell>();
        }

        /// <summary>
        /// Признак содержания ошибки в выражении.
        /// </summary>
        public abstract bool IsError { get; }

        public abstract object objValue { get; }

        /// <summary>
        /// Строковое представление выражения.
        /// </summary>
        public abstract string Formula();

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
        public ArithmeticExpressions.ICell this[string key]
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
    }
}
