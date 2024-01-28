namespace ExpressionsLibrary
{
    /// <summary>
    /// Базовый класс выражений.
    /// </summary>
    public abstract class ExpressionBase
    {
        /// <summary>
        /// Признак содержания ошибки в выражении.
        /// </summary>
        public abstract bool IsError { get; }

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
        /// Определяемые пользователем данные, связанные с этим объектом.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Признак применения формата.
        /// </summary>
        /// <param name="format">Строка, описывающая формат отображения результата алгебраического выражения.</param>
        /// <returns></returns>
        protected static bool IsFormat(string format)
        {
            return !string.IsNullOrWhiteSpace(format);
        }
    }
}
