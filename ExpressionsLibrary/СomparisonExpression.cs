using System.Text.RegularExpressions;

namespace ExpressionsLibrary
{
    /// <summary>
    /// Логическое выражение.
    /// </summary>
    class СomparisonExpression : СomparisonExpressions.ExpressionBase, IBooleanExpression
    {
        #region РЕГУЛЯРНЫЕ ВЫРАЖЕНИЯ

        /// <summary>
        /// Равно [=].
        /// </summary>
        internal const string EQUAL = @"\x3d"; // "="
        /// <summary>
        /// Не равно [&lt;>],[!=].
        /// </summary>
        internal const string NOT_EQUAL = @"((\x3c\x3e)|(\x21\x3d))"; // "<>" / "!="
        /// <summary>
        /// Меньше [&lt;].
        /// </summary>
        internal const string LESS = @"\x3c"; // "<"
        /// <summary>
        /// Больше (>).
        /// </summary>
        internal const string MORE = @"\x3e"; // ">"
        /// <summary>
        /// Меньше или равно(&lt;=).
        /// </summary>
        internal const string LESS_OR_EQUAL = @"\x3c\x3d"; // "<="
        /// <summary>
        /// Больше или равно (>=).
        /// </summary>
        internal const string MORE_OR_EQUAL = @"\x3e\x3d"; // ">="

        /// <summary>
        /// Коллекция знаков сравнения.
        /// </summary>
        internal const string COMPARISON = LESS_OR_EQUAL + @"|" + MORE_OR_EQUAL + @"|" + NOT_EQUAL + @"|" + EQUAL + @"|" + LESS + @"|" + MORE;

        /// <summary>
        /// Регулярное выражение для поиска всех компонентов, включая ячейки.
        /// </summary>
        internal static Regex regexAll;

        /// <summary>
        /// Регулярное выражение для поиска знака равно [=].
        /// </summary>
        internal static readonly Regex regexEqual = new Regex(EQUAL, ArithmeticExpression.OPTIONS); // "="
        /// <summary>
        /// Регулярное выражение для поиска знака не равно [&lt;>].
        /// </summary>
        internal static readonly Regex regexNotEqual = new Regex(NOT_EQUAL, ArithmeticExpression.OPTIONS); // "<>" / "!="
        /// <summary>
        /// Регулярное выражение для поиска знака меньше [&lt;].
        /// </summary>
        internal static readonly Regex regexLess = new Regex(LESS, ArithmeticExpression.OPTIONS); // "<"
        /// <summary>
        /// Регулярное выражение для поиска знака больше [>].
        /// </summary>
        internal static readonly Regex regexMore = new Regex(MORE, ArithmeticExpression.OPTIONS); // ">"
        /// <summary>
        /// Регулярное выражение для поиска знака меньше или равно [&lt;=].
        /// </summary>
        internal static readonly Regex regexLessOrEqual = new Regex(LESS_OR_EQUAL, ArithmeticExpression.OPTIONS); // "<="
        /// <summary>
        /// Регулярное выражение для поиска знака больше или равно [>=].
        /// </summary>
        internal static readonly Regex regexMoreOrEqual = new Regex(MORE_OR_EQUAL, ArithmeticExpression.OPTIONS); // ">="

        #endregion

        /// <summary>
        /// Начало сообщение об ошибке "[Error:".
        /// </summary>
        public static string SymbolStartError { get; set; }
        /// <summary>
        /// Окончание сообщения об ошибке "]".
        /// </summary>
        public static string SymbolEndError { get; set; }
        /// <summary>
        /// Равно.
        /// </summary>
        public static string SymbolEqual { get; set; }
        /// <summary>
        /// Не равно.
        /// </summary>
        public static string SymbolNotEqual { get; set; }
        /// <summary>
        /// Меньше.
        /// </summary>
        public static string SymbolLess { get; set; }
        /// <summary>
        /// Больше.
        /// </summary>
        public static string SymbolMore { get; set; }
        /// <summary>
        /// Меньше или равно.
        /// </summary>
        public static string SymbolLessOrEqual { get; set; }
        /// <summary>
        /// Больше или равно.
        /// </summary>
        public static string SymbolMoreOrEqual { get; set; }

        /// <summary>
        /// Значение логического выражения.
        /// </summary>
        public override bool Value
        {
            get { return ((IBooleanExpression)expression).Value; }
        }

        /// <summary>
        /// Признак содержания ошибки в выражении.
        /// </summary>
        public override bool IsError
        {
            get { return expression.IsError; }
        }

        /// <summary>
        /// Строковое представление логического выражения.
        /// </summary>
        public override string Formula()
        {
            return expression.Formula();
        }

        public override string Formula(string format)
        {
            return expression.Formula(format: format);
        }

        /// <summary>
        /// Короткое строковое представление логического выражения.
        /// </summary>
        /// <param name="format">Формат отображения результата алгебраического выражения.</param>
        public override string ToString(string format)
        {
            return expression.ToString(format: format);
        }

        private СomparisonExpression(UnitCollection array) : base()
        {
            InitializeSymbols();
            expression = СomparisonExpressions.Expression.Create(ref cells, array);
        }

        internal static void InitializeSymbols()
        {
            ArithmeticExpression.InitializeSymbols();

            SymbolStartError = ArithmeticExpression.SymbolStartError;
            SymbolEndError = ArithmeticExpression.SymbolEndError;

            SymbolEqual = string.IsNullOrWhiteSpace(SymbolEqual) ? @"=" : SymbolEqual;
            SymbolNotEqual = string.IsNullOrWhiteSpace(SymbolNotEqual) ? @"<>" : SymbolNotEqual;
            SymbolLess = string.IsNullOrWhiteSpace(SymbolLess) ? @"<" : SymbolLess;
            SymbolMore = string.IsNullOrWhiteSpace(SymbolMore) ? @">" : SymbolMore;
            SymbolLessOrEqual = string.IsNullOrWhiteSpace(SymbolLessOrEqual) ? "<=" : SymbolLessOrEqual;
            SymbolMoreOrEqual = string.IsNullOrWhiteSpace(SymbolMoreOrEqual) ? @">=" : SymbolMoreOrEqual;
        }


        public static bool IsExpression(string text)
        {
            string context = text.Replace(" ", "");
            Match matchNot = BooleanExpression.regexNot.Match(context);
            if (matchNot.Length > 0 && matchNot.Index == 0)
            {
                return false;
            }
            else
            {
                regexAll = new Regex(COMPARISON, ArithmeticExpression.OPTIONS);
                return regexAll.IsMatch(text);
            }
        }

        public static IBooleanExpression Create(string text)
        {
            UnitCollection collection = UnitCollection.Create(text: text);
            return Create(collection: collection);
        }

        public static IBooleanExpression Create(string text, string cellpattern)
        {
            UnitCollection collection = UnitCollection.Create(text: text, cellpattern: cellpattern);
            return Create(collection: collection);
        }

        public static IBooleanExpression Create(UnitCollection collection)
        {
            return new СomparisonExpression(collection);
        }
    }
}
