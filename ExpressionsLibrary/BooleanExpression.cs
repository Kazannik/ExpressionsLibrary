using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExpressionsLibrary
{
    public class BooleanExpression : BooleanExpressions.ExpressionBase
    {
        private LogicExpressions.ILogicExpression expression;
        private Dictionary<string, ArithmeticExpressions.ICell> collection;

        #region РЕГУЛЯРНЫЕ ВЫРАЖЕНИЯ

        /// <summary>
        /// Логическое отрицание (NOT).
        /// </summary>
        private const string csNot = @"((\x21)|(not))"; // "not" / "!"
        /// <summary>
        /// Логическое сложение (AND).
        /// </summary>
        private const string csAnd = @"((\x26)|(and))"; // "and" / "&"
        /// <summary>
        /// Логическое ИЛИ (OR).
        /// </summary>
        private const string csOr = @"((\x7c)|(or))"; // "or" / "|"
        /// <summary>
        /// Исключающее ИЛИ (XOR).
        /// </summary>
        private const string csXor = @"(xor)"; // "xor"

        /// <summary>
        /// Положительное выражение (True).
        /// </summary>
        private const string csTrue = @"(true)"; // "true"

        /// <summary>
        /// Отрицательное выражение (False).
        /// </summary>
        private const string csFalse = @"(false)"; // "false"


        /// <summary>
        /// Коллекция логических знаков.
        /// </summary>
        private const string csBoolean = csAnd + @"|" + csNot + @"|" + csOr + @"|" + csXor + @"|" + csTrue + @"|" + csFalse;

        /// <summary>
        /// Регулярное выражение для поиска всех компонентов, включая ячейки.
        /// </summary>
        private static Regex regexAll;
        /// <summary>
        /// Регулярное выражение для поиска знака логического отрицания [NOT].
        /// </summary>
        internal static Regex regexNot = new Regex(csNot, ArithmeticExpression.options); // "not"
        /// <summary>
        /// Регулярное выражение для поиска знака логического сложения [AND].
        /// </summary>
        internal static Regex regexAnd = new Regex(csAnd, ArithmeticExpression.options); // "and"
        /// <summary>
        /// Регулярное выражение для поиска знака логического или [OR].
        /// </summary>
        internal static Regex regexOr = new Regex(csOr, ArithmeticExpression.options); // "or"
        /// <summary>
        /// Регулярное выражение для поиска знака исключающего или [XOR].
        /// </summary>
        internal static Regex regexXor = new Regex(csXor, ArithmeticExpression.options); // "xor"
        /// <summary>
        /// Регулярное выражение для поиска знака положительного выражения [True].
        /// </summary>
        internal static Regex regexTrue = new Regex(csTrue, ArithmeticExpression.options); // "true"
        /// <summary>
        /// Регулярное выражение для поиска знака отрицательного выражения [False].
        /// </summary>
        internal static Regex regexFalse = new Regex(csFalse, ArithmeticExpression.options); // "false"
        
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
        
        private BooleanExpression(ref Dictionary<string, ArithmeticExpressions.ICell> cells, UnitCollection array)
        {
            InitializeSymbols();
            collection = cells;
            expression = BooleanExpressions.Expression.Create(ref collection, array);
        }

        private BooleanExpression(UnitCollection array)
        {
            InitializeSymbols();
            collection = new Dictionary<string, ArithmeticExpressions.ICell>();
            expression = BooleanExpressions.Expression.Create(ref this.collection, array);
        }

        internal static void InitializeSymbols()
        {
            LogicExpression.InitializeSymbols();

            SymbolStartError = ArithmeticExpression.SymbolStartError;
            SymbolEndError = ArithmeticExpression.SymbolEndError;

            SymbolAnd = @"AND";
            SymbolOr = @"OR";
            SymbolXor = @"XOR";
            SymbolNot = @"NOT";
            SymbolTrue = "TRUE";
            SymbolFalse = @"FALSE";
        }

        public static BooleanExpression Create(string text)
        {
            string context = text.Replace(" ", "");
            regexAll = new Regex(LogicExpression.csLogic + @"|" + csBoolean + @"|" + ArithmeticExpression.csArithmetic + @"|"  +  ArithmeticExpression.csOpen + @"|" + ArithmeticExpression.csClose, ArithmeticExpression.options);
            UnitCollection collection = UnitCollection.Create(ArithmeticExpression.regexAll.Matches(text));
            return new BooleanExpression(collection);
        }

        public static BooleanExpression Create(string text, string cellpattern)
        {
            string context = text.Replace(" ", "");
            regexAll = new Regex(@"(" + cellpattern + @")|" + LogicExpression.csLogic + @"|" + csBoolean + @"|" + ArithmeticExpression.csArithmetic + @"|" + ArithmeticExpression.csOpen + @"|" + ArithmeticExpression.csClose, ArithmeticExpression.options);
            ArithmeticExpression.regexCell = new Regex(@"(" + cellpattern + @")", ArithmeticExpression.options);
            UnitCollection collection = UnitCollection.Create(regexAll.Matches(text));
            return new BooleanExpression(collection);
        }
    }
}
