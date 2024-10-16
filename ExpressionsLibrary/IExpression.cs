using ExpressionsLibrary.ArithmeticExpressions;
using System.Collections.Generic;

namespace ExpressionsLibrary
{
    /// <summary>
    /// Интерфейс математического выражения.
    /// </summary>
    public interface IExpression : IEnumerable<ICell>
    {
        /// <summary>
        /// Признак содержания ошибки в выражении.
        /// </summary>
        bool IsError { get; }

        /// <summary>
        /// Строковое представление выражения.
        /// </summary>
        string Formula();

        /// <summary>
        /// Строковое представление выражения.
        /// </summary>
        /// <param name="format">Строка, описывающая формат отображения результата алгебраического выражения.</param>

        string Formula(string format);

        /// <summary>
        /// Короткое строковое представление выражения.
        /// </summary>
        /// <returns></returns>
        string ToString();

        /// <summary>
        /// Короткое строковое представление выражения.
        /// </summary>
        /// <param name="format">Строка, описывающая формат отображения результата алгебраического выражения.</param>
        /// <returns></returns>
        string ToString(string format);

        /// <summary>
        /// Значение выражения.
        /// </summary>
        object objValue { get; }

        /// <summary>
        /// Определяет содержится ли ячейка с указанным ключем в выражении.
        /// </summary>
        /// <param name="key">Ключ ячейки.</param>
        /// <returns></returns>
        bool Contains(string key);

        /// <summary>
        /// Коллекция ключей ячеек.
        /// </summary>
        string[] Keys { get; }

        /// <summary>
        /// Ячейка, используемая при расчете.
        /// </summary>
        /// <param name="key">Ключ ячейки.</param>
        /// <returns></returns>
        ICell this[string key] { get; }

        /// <summary>
        /// Количество ячеек, используемых при расчете.
        /// </summary>
        int Count { get; }
    }
}
