namespace ExpressionsLibrary
{
    /// <summary>
    /// Базовый класс выражений.
    /// </summary>
    abstract class ExpressionBase: IExpression
    {
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
    }
}
