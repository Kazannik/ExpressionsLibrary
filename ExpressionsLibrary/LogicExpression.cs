using ExpressionsLibrary.LogicExpressions;
using System.Text.RegularExpressions;

namespace ExpressionsLibrary
{
    /// <summary>
    /// Логическое выражение.
    /// </summary>
    class LogicExpression : LogicExpressions.ExpressionBase, ILogicExpression
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
        /// Коллекция логических знаков.
        /// </summary>
        internal const string LOGIC = LESS_OR_EQUAL + @"|" + MORE_OR_EQUAL + @"|" + NOT_EQUAL + @"|" + EQUAL + @"|" + LESS + @"|" + MORE;

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
            get { return ((ILogicExpression)expression).Value; }
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

        /// <summary>
        /// Короткое строковое представление логического выражения.
        /// </summary>
        /// <param name="format">Формат отображения результата алгебраического выражения.</param>
        public override string ToString(string format)
        {
            return expression.ToString(format: format);
        }

        private LogicExpression(UnitCollection array) : base()
        {
            InitializeSymbols();
            expression = LogicExpressions.Expression.Create(ref collection, array);
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
                regexAll = new Regex(LOGIC, ArithmeticExpression.OPTIONS);
                return regexAll.IsMatch(text);
            }
        }

        public static ILogicExpression Create(string text)
        {
            string context = text.Replace(" ", "");
            regexAll = new Regex(LOGIC + @"|" + ArithmeticExpression.ARITHMETIC + @"|" + ArithmeticExpression.OPEN + @"|" + ArithmeticExpression.CLOSE, ArithmeticExpression.OPTIONS);
            UnitCollection collection = UnitCollection.Create(regexAll.Matches(text));
            return new LogicExpression(collection);
        }

        public static ILogicExpression Create(string text, string cellpattern)
        {
            string context = text.Replace(" ", "");
            regexAll = new Regex(@"(" + cellpattern + @")|" + LOGIC + @"|" + ArithmeticExpression.ARITHMETIC + @"|" + ArithmeticExpression.OPEN + @"|" + ArithmeticExpression.CLOSE, ArithmeticExpression.OPTIONS);
            ArithmeticExpression.regexCell = new Regex(@"(" + cellpattern + @")", ArithmeticExpression.OPTIONS);
            UnitCollection collection = UnitCollection.Create(regexAll.Matches(text));
            return new LogicExpression(collection);
        }
    }
}
