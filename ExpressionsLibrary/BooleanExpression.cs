using System.Text.RegularExpressions;

namespace ExpressionsLibrary
{
    /// <summary>
    /// Булевое логическое выражение.
    /// </summary>
    class BooleanExpression : BooleanExpressions.ExpressionBase, IBooleanExpression
    {
        #region РЕГУЛЯРНЫЕ ВЫРАЖЕНИЯ

        /// <summary>
        /// Логическое отрицание (NOT).
        /// </summary>
        internal const string NOT = @"((\x21)|(not))"; // "not" / "!"
        /// <summary>
        /// Логическое сложение (AND).
        /// </summary>
        internal const string AND = @"((\x26)|(and))"; // "and" / "&"
        /// <summary>
        /// Логическое ИЛИ (OR).
        /// </summary>
        internal const string OR = @"((\x7c)|(or))"; // "or" / "|"
        /// <summary>
        /// Исключающее ИЛИ (XOR).
        /// </summary>
        internal const string XOR = @"(xor)"; // "xor"

        /// <summary>
        /// Положительное выражение (True).
        /// </summary>
        internal const string TRUE = @"(true)"; // "true"

        /// <summary>
        /// Отрицательное выражение (False).
        /// </summary>
        internal const string FALSE = @"(false)"; // "false"
        
        /// <summary>
        /// Коллекция логических знаков.
        /// </summary>
        internal const string BOOLEAN = AND + @"|" + NOT + @"|" + OR + @"|" + XOR + @"|" + TRUE + @"|" + FALSE;

        /// <summary>
        /// Регулярное выражение для поиска всех компонентов, включая ячейки.
        /// </summary>
        private static Regex regexAll;
        /// <summary>
        /// Регулярное выражение для поиска знака логического отрицания [NOT].
        /// </summary>
        internal static readonly Regex regexNot = new Regex(NOT, ArithmeticExpression.OPTIONS); // "not"
        /// <summary>
        /// Регулярное выражение для поиска знака логического сложения [AND].
        /// </summary>
        internal static readonly Regex regexAnd = new Regex(AND, ArithmeticExpression.OPTIONS); // "and"
        /// <summary>
        /// Регулярное выражение для поиска знака логического или [OR].
        /// </summary>
        internal static readonly Regex regexOr = new Regex(OR, ArithmeticExpression.OPTIONS); // "or"
        /// <summary>
        /// Регулярное выражение для поиска знака исключающего или [XOR].
        /// </summary>
        internal static readonly Regex regexXor = new Regex(XOR, ArithmeticExpression.OPTIONS); // "xor"
        /// <summary>
        /// Регулярное выражение для поиска знака положительного выражения [True].
        /// </summary>
        internal static readonly Regex regexTrue = new Regex(TRUE, ArithmeticExpression.OPTIONS); // "true"
        /// <summary>
        /// Регулярное выражение для поиска знака отрицательного выражения [False].
        /// </summary>
        internal static readonly Regex regexFalse = new Regex(FALSE, ArithmeticExpression.OPTIONS); // "false"

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
        /// AND.
        /// </summary>
        public static string SymbolAnd { get; set; }
        /// <summary>
        /// OR.
        /// </summary>
        public static string SymbolOr { get; set; }
        /// <summary>
        /// XOR.
        /// </summary>
        public static string SymbolXor { get; set; }
        /// <summary>
        /// NOT.
        /// </summary>
        public static string SymbolNot { get; set; }
        /// <summary>
        /// True.
        /// </summary>
        public static string SymbolTrue { get; set; }
        /// <summary>
        /// False.
        /// </summary>
        public static string SymbolFalse { get; set; }

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

        private BooleanExpression(UnitCollection array) : base()
        {
            InitializeSymbols();
            expression = BooleanExpressions.Expression.Create(ref cells, array);
        }

        internal static void InitializeSymbols()
        {
            СomparisonExpression.InitializeSymbols();

            SymbolStartError = ArithmeticExpression.SymbolStartError;
            SymbolEndError = ArithmeticExpression.SymbolEndError;

            SymbolAnd = string.IsNullOrWhiteSpace(SymbolAnd) ? @"AND" : SymbolAnd;
            SymbolOr = string.IsNullOrWhiteSpace(SymbolOr) ? @"OR" : SymbolOr;
            SymbolXor = string.IsNullOrWhiteSpace(SymbolXor) ? @"XOR" : SymbolXor;
            SymbolNot = string.IsNullOrWhiteSpace(SymbolNot) ? @"NOT" : SymbolNot;
            SymbolTrue = string.IsNullOrWhiteSpace(SymbolTrue) ? "TRUE" : SymbolTrue;
            SymbolFalse = string.IsNullOrWhiteSpace(SymbolFalse) ? @"FALSE" : SymbolFalse;
        }


        public static bool IsExpression(string text)
        {
            string context = text.Replace(" ", "");
            regexAll = new Regex(BOOLEAN, ArithmeticExpression.OPTIONS);
            return regexAll.IsMatch(text);
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
            return new BooleanExpression(collection);
        }
    }
}
