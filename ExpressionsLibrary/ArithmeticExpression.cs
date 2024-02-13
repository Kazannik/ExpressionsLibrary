using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace ExpressionsLibrary
{
    /// <summary>
    /// Алгебраическое выражение.
    /// </summary>
    class ArithmeticExpression : ArithmeticExpressions.ExpressionBase, ArithmeticExpressions.IExpression
    {
        #region РЕГУЛЯРНЫЕ ВЫРАЖЕНИЯ

        /// <summary>
        /// Цифры.
        /// </summary>
        internal const string csDecimal = @"((\d{1,}[.]\d{1,})|(\d{1,}[,]\d{1,})|(\d{1,}))";
        /// <summary>
        /// Возведение в степень [pow],[^].
        /// </summary>
        internal const string csPower = @"((\x5e)|(pow))"; // "pow" / "^"
        /// <summary>
        /// Корень [sqrt].
        /// </summary>
        internal const string csSqrt = @"((\u221a)|(sqrt))"; // "sqrt" / "√"
        /// <summary>
        /// Умножение [*].
        /// </summary>
        internal const string csMultiplication = @"((\x78)|(\x2a))"; // "*" / "x"
        /// <summary>
        /// Деление [/].
        /// </summary>
        internal const string csDivision = @"(\x2f)"; // "/"
        /// <summary>
        /// Целочисленное деление [fix],[\].
        /// </summary>
        internal const string csFix = @"((\x5c)|(fix))";// "fix" / "\"
        /// <summary>
        /// Остаток от деления [mod],[%].
        /// </summary>
        internal const string csMod = @"((\x25)|(mod))"; // "mod" / "%"
        /// <summary>
        /// Сложение [+].
        /// </summary>
        internal const string csAddition = @"(\x2b)"; // "+"
        /// <summary>
        /// Вычитание [-].
        /// </summary>
        internal const string csSubtracting = @"(\x2d)"; // "-"
        /// <summary>
        /// Открывающаяся скобка [(].
        /// </summary>
        internal const string csOpen = @"(\x28)"; // "("
        /// <summary>
        /// Закрывающаяся скобка [)].
        /// </summary>
        internal const string csClose = @"(\x29)"; // ")"        
        /// <summary>
        /// Ключ ячейки в формате формулы.
        /// </summary>
        private const string csCellKey = @"\x7b(key)\x7d"; // "{key}"
        /// <summary>
        /// Значение ячейки в формате формулы.
        /// </summary>
        private const string csCellValue = @"\x7b(value)\x7d"; // "{value}"

        /// <summary>
        /// Коллекция арифметических знаков.
        /// </summary>
        internal const string csArithmetic = csDecimal + @"|" + csAddition + @"|" + csDivision + @"|" + csFix + @"|" + csMod + @"|" + csMultiplication + @"|" + csPower + @"|" + csSqrt + @"|" + csSubtracting;

        internal const RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Compiled;

        /// <summary>
        /// Регулярное выражение для поиска всех компонентов, включая ячейки.
        /// </summary>
        internal static Regex regexAll;
        /// <summary>
        /// Регулярное выражение для поиска ячеек.
        /// </summary>
        internal static Regex regexCell;
        /// <summary>
        /// Регулярное выражение для поиска цифр.
        /// </summary>
        internal static Regex regexDecimal = new Regex(csDecimal, options);
        /// <summary>
        /// Регулярное выражение для поиска знака pow (возведение в степень).
        /// </summary>
        internal static Regex regexPower = new Regex(csPower, options); // "pow" / "^"
        /// <summary>
        /// Регулярное выражение для поиска знака sqrt (корень).
        /// </summary>
        internal static Regex regexSqrt = new Regex(csSqrt, options); // "sqrt / "√"
        /// <summary>
        /// Регулярное выражение для поиска знака умножения [*].
        /// </summary>
        internal static Regex regexMultiplication = new Regex(csMultiplication, options); // "*"
        /// <summary>
        /// Регулярное выражение для поиска знака деления [/].
        /// </summary>
        internal static Regex regexDivision = new Regex(csDivision, options); // "/"
        /// <summary>
        /// Регулярное выражение для поиска знака fix (целое от деления).
        /// </summary>
        internal static Regex regexFix = new Regex(csFix, options);// "fix" / "\"
        /// <summary>
        /// Регулярное выражение для поиска знака mod (остаток от деления).
        /// </summary>
        internal static Regex regexMod = new Regex(csMod, options); // "mod" / "%"
        /// <summary>
        /// Регулярное выражение для поиска знака сложения [+].
        /// </summary>
        internal static Regex regexAddition = new Regex(csAddition, options); // "+"
        /// <summary>
        /// Регулярное выражение для поиска знака вычитания [-].
        /// </summary>
        internal static Regex regexSubtracting = new Regex(csSubtracting, options); // "-"
        /// <summary>
        /// Регулярное выражение для поиска знака открывающейся скобки [(].
        /// </summary>
        internal static Regex regexOpen = new Regex(csOpen, options); // "("
        /// <summary>
        /// Регулярное выражение для поиска знака закрывающейся скобки [)].
        /// </summary>
        internal static Regex regexClose = new Regex(csClose, options); // ")"
        /// <summary>
        /// Ключ ячейки в формате формулы.
        /// </summary>
        internal static Regex regexCellKey = new Regex(csCellKey, options); // "{key}"
        /// <summary>
        /// Значение ячейки в формате формулы.
        /// </summary>
        internal static Regex regexCellValue = new Regex(csCellValue, options); // "{value}"
        #endregion

        private ArithmeticExpressions.IExpression expression;
        private Dictionary<string, ArithmeticExpressions.ICell> collection;
        
        /// <summary>
        /// Пробел ' '.
        /// </summary>
        public static string SymbolSpace { get; set; }

        /// <summary>
        /// Начало сообщение об ошибке "[Error:".
        /// </summary>
        public static string SymbolStartError { get; set; }
        /// <summary>
        /// Окончание сообщения об ошибке "]".
        /// </summary>
        public static string SymbolEndError { get; set; }

        /// <summary>
        /// Сложение.
        /// </summary>
        public static string SymbolAddition { get; set; }
        /// <summary>
        /// Вычитание.
        /// </summary>
        public static string SymbolSubtracting { get; set; }
        /// <summary>
        /// Умножение.
        /// </summary>
        public static string SymbolMultiplication { get; set; }
        /// <summary>
        /// Деление.
        /// </summary>    
        public static string SymbolDivision { get; set; }
        /// <summary>
        /// Целая часть в результате деления.
        /// </summary>    
        public static string SymbolFix { get; set; }
        /// <summary>
        /// Остаток от деления.
        /// </summary>    
        public static string SymbolMod { get; set; }
        /// <summary>
        /// Возведение в степень.
        /// </summary>    
        public static string SymbolPower { get; set; }
        /// <summary>
        /// Извлечение корня.
        /// </summary>  
        public static string SymbolSqrt { get; set; }

        /// <summary>
        /// Значение алгебраического выражения.
        /// </summary>
        public override decimal Value
        {
            get { return expression.Value; }
        }

        /// <summary>
        /// Признак содержания ошибки в выражении.
        /// </summary>
        public override bool IsError
        {
            get { return expression.IsError; }
        }

        /// <summary>
        /// Строковое представление алгебраического выражения.
        /// </summary>
        public override string Formula()
        {
            return expression.Formula();
        }

        public bool Contains(string key)
        {
            return collection.ContainsKey(key);
        }

        public string[] Keys
        {
            get { return collection.Keys.ToArray(); }
        }

        public ArithmeticExpressions.ICell this[string key]
        {
            get { return collection[key]; }
        }

        public int Count
        {
            get { return collection.Count; }
        }

        private ArithmeticExpression(ref Dictionary<string, ArithmeticExpressions.ICell> cells, UnitCollection array)
        {
            InitializeSymbols();
            collection = cells;
            expression = ArithmeticExpressions.Expression.Create(ref collection, array);
        }

        private ArithmeticExpression(UnitCollection array)
        {
            InitializeSymbols();
            collection = new Dictionary<string, ArithmeticExpressions.ICell>();
            expression = ArithmeticExpressions.Expression.Create(ref collection, array);
        }

        internal static void InitializeSymbols()
        {
            SymbolSpace = string.IsNullOrWhiteSpace(SymbolSpace) ? @" ": SymbolSpace;

            SymbolStartError = string.IsNullOrWhiteSpace(SymbolStartError) ? @"[Error:": SymbolStartError;
            SymbolEndError = string.IsNullOrWhiteSpace(SymbolEndError) ? @"]": SymbolEndError;

            SymbolAddition = string.IsNullOrWhiteSpace(SymbolAddition) ? @"+": SymbolAddition;
            SymbolDivision = string.IsNullOrWhiteSpace(SymbolDivision) ? @"/": SymbolDivision;
            SymbolFix = string.IsNullOrWhiteSpace(SymbolFix) ? @"\": SymbolFix;
            SymbolMod = string.IsNullOrWhiteSpace(SymbolMod) ? @"%": SymbolMod;
            SymbolMultiplication = string.IsNullOrWhiteSpace(SymbolMultiplication) ? "x": SymbolMultiplication;
            SymbolPower = string.IsNullOrWhiteSpace(SymbolPower) ? @"^": SymbolPower;
            SymbolSqrt = string.IsNullOrWhiteSpace(SymbolSqrt) ? @"√": SymbolSqrt;
            SymbolSubtracting = string.IsNullOrWhiteSpace(SymbolSubtracting) ? @"-": SymbolSubtracting;
        }

        public static bool IsExpression(string text)
        {
            string context = text.Replace(" ", "");
            regexAll = new Regex(csArithmetic + @"|" + csOpen + @"|" + csClose, options);
            return regexAll.IsMatch(text);
        }

        public static IExpression Create(string text)
        {
            string context = text.Replace(" ", "");
            regexAll = new Regex(csArithmetic + @"|" + csOpen + @"|" + csClose, options);
            UnitCollection collection = UnitCollection.Create(regexAll.Matches(text));
            return new ArithmeticExpression(collection);
        }

        public static IExpression Create(string text, string cellpattern)
        {
            string context = text.Replace(" ", "");
            regexAll = new Regex(@"(" + cellpattern + @")|" + csArithmetic + @"|" + csOpen + @"|" + csClose, options);
            regexCell = new Regex(@"(" + cellpattern + @")", options);
            UnitCollection collection = UnitCollection.Create(regexAll.Matches(text));
            return new ArithmeticExpression(collection);
        }
    }
}