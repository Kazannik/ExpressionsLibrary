namespace ExpressionsLibrary
{
    public interface IExpression
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
    }
}
