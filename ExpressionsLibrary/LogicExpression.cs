using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExpressionsLibrary
{
    /// <summary>
    /// Логическое выражение.
    /// </summary>
    class LogicExpression : LogicExpressions.ExpressionBase, LogicExpressions.ILogicExpression
    {
        #region РЕГУЛЯРНЫЕ ВЫРАЖЕНИЯ
                
        /// <summary>
        /// Равно [=].
        /// </summary>
        internal const string csEqual = @"\x3d"; // "="
        /// <summary>
        /// Не равно [&lt;>],[!=].
        /// </summary>
        internal const string csNotEqual = @"((\x3c\x3e)|(\x21\x3d))"; // "<>" / "!="
        /// <summary>
        /// Меньше [&lt;].
        /// </summary>
        internal const string csLess = @"\x3c"; // "<"
        /// <summary>
        /// Больше (>).
        /// </summary>
        internal const string csMore = @"\x3e"; // ">"
        /// <summary>
        /// Меньше или равно(&lt;=).
        /// </summary>
        internal const string csLessOrEqual = @"\x3c\x3d"; // "<="
        /// <summary>
        /// Больше или равно (>=).
        /// </summary>
        internal const string csMoreOrEqual = @"\x3e\x3d"; // ">="

        /// <summary>
        /// Коллекция логических знаков.
        /// </summary>
        internal const string csLogic = csLessOrEqual + @"|" + csMoreOrEqual + @"|" + csNotEqual + @"|" + csEqual + @"|" + csLess  + @"|" + csMore ;

        /// <summary>
        /// Регулярное выражение для поиска всех компонентов, включая ячейки.
        /// </summary>
        internal static Regex regexAll;
         
        /// <summary>
        /// Регулярное выражение для поиска знака равно [=].
        /// </summary>
        internal static Regex regexEqual = new Regex(csEqual, ArithmeticExpression.options); // "="
        /// <summary>
        /// Регулярное выражение для поиска знака не равно [&lt;>].
        /// </summary>
        internal static Regex regexNotEqual = new Regex(csNotEqual, ArithmeticExpression.options); // "<>" / "!="
        /// <summary>
        /// Регулярное выражение для поиска знака меньше [&lt;].
        /// </summary>
        internal static Regex regexLess = new Regex(csLess, ArithmeticExpression.options); // "<"
        /// <summary>
        /// Регулярное выражение для поиска знака больше [>].
        /// </summary>
        internal static Regex regexMore = new Regex(csMore, ArithmeticExpression.options); // ">"
        /// <summary>
        /// Регулярное выражение для поиска знака меньше или равно [&lt;=].
        /// </summary>
        internal static Regex regexLessOrEqual = new Regex(csLessOrEqual, ArithmeticExpression.options); // "<="
        /// <summary>
        /// Регулярное выражение для поиска знака больше или равно [>=].
        /// </summary>
        internal static Regex regexMoreOrEqual = new Regex(csMoreOrEqual, ArithmeticExpression.options); // ">="

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
            get { return ((LogicExpressions.ILogicExpression) expression).Value; }
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

        private LogicExpression(UnitCollection array): base()
        {
            InitializeSymbols();
            expression = LogicExpressions.Expression.Create(ref this.collection, array);
        }

        internal static void InitializeSymbols()
        {
            ArithmeticExpression.InitializeSymbols();

            SymbolStartError = ArithmeticExpression.SymbolStartError;
            SymbolEndError = ArithmeticExpression.SymbolEndError;

            SymbolEqual = string.IsNullOrWhiteSpace(SymbolEqual) ? @"=": SymbolEqual;
            SymbolNotEqual = string.IsNullOrWhiteSpace(SymbolNotEqual) ? @"<>": SymbolNotEqual;
            SymbolLess = string.IsNullOrWhiteSpace(SymbolLess) ? @"<": SymbolLess;
            SymbolMore = string.IsNullOrWhiteSpace(SymbolMore) ? @">": SymbolMore;
            SymbolLessOrEqual = string.IsNullOrWhiteSpace(SymbolLessOrEqual) ? "<=": SymbolLessOrEqual;
            SymbolMoreOrEqual = string.IsNullOrWhiteSpace(SymbolMoreOrEqual) ? @">=": SymbolMoreOrEqual;
        }


        public static bool IsExpression(string text)
        {
            string context = text.Replace(" ", "");
            regexAll = new Regex(csLogic, ArithmeticExpression.options);
            return regexAll.IsMatch(text);
        }

        public static LogicExpressions.ILogicExpression Create(string text)
        {
            string context = text.Replace(" ", "");
            regexAll = new Regex(csLogic + @"|" + ArithmeticExpression.csArithmetic + @"|" +  ArithmeticExpression.csOpen + @"|" + ArithmeticExpression.csClose, ArithmeticExpression.options);
            UnitCollection collection = UnitCollection.Create(regexAll.Matches(text));
            return new LogicExpression(collection);
        }

        public static LogicExpressions.ILogicExpression Create(string text, string cellpattern)
        {
            string context = text.Replace(" ", "");
            regexAll = new Regex(@"(" + cellpattern + @")|" + csLogic + @"|" + ArithmeticExpression.csArithmetic + @"|"  + ArithmeticExpression.csOpen + @"|" + ArithmeticExpression.csClose, ArithmeticExpression.options);
            ArithmeticExpression.regexCell = new Regex(@"(" + cellpattern + @")", ArithmeticExpression.options);
            UnitCollection collection = UnitCollection.Create(regexAll.Matches(text));
            return new LogicExpression(collection);
        }
    }
}
