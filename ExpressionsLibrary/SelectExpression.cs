using ExpressionsLibrary.SelectExpressions;
using System.Text.RegularExpressions;

namespace ExpressionsLibrary
{
    class SelectExpression : SelectExpressions.ExpressionBase, ISelectExpression
    {
        #region РЕГУЛЯРНЫЕ ВЫРАЖЕНИЯ

        /// <summary>
        /// Если [if].
        /// </summary>
        private const string IF = @"(if)"; // "if"

        /// <summary>
        /// Тогда [then].
        /// </summary>
        private const string THEN = @"(then)"; // "then"

        /// <summary>
        /// Иначе [else].
        /// </summary>
        private const string ELSE = @"(else)"; // "else"        

        /// <summary>
        /// Коллекция логических знаков.
        /// </summary>
        internal const string SELECT = IF + @"|" + THEN + @"|" + ELSE;

        /// <summary>
        /// Регулярное выражение для поиска всех компонентов, включая ячейки.
        /// </summary>
        internal static Regex regexAll;

        /// <summary>
        /// Регулярное выражение для поиска знака ЕСЛИ [if].
        /// </summary>
        internal static readonly Regex regexIf = new Regex(IF, ArithmeticExpression.OPTIONS); // "if"
                                                                                                   
        /// <summary>
        /// Регулярное выражение для поиска знака ТОГДА [then].
        /// </summary>
        internal static readonly Regex regexThen = new Regex(THEN, ArithmeticExpression.OPTIONS); // "then"
                                                                                                           
        /// <summary>
        /// Регулярное выражение для поиска знака ИНАЧЕ [else].
        /// </summary>
        internal static readonly Regex regexElse = new Regex(ELSE, ArithmeticExpression.OPTIONS); // "else"
    
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
        /// Если.
        /// </summary>
        public static string SymbolIf { get; set; }
        /// <summary>
        /// Тогда.
        /// </summary>
        public static string SymbolThen { get; set; }
        /// <summary>
        /// Иначе.
        /// </summary>
        public static string SymbolElse { get; set; }
        
        /// <summary>
        /// Значение логического выражения.
        /// </summary>
        public override bool IsTrue
        {
            get { return ((ISelectExpression)expression).IsTrue; }
        }

        /// <summary>
        /// Значение выражения.
        /// </summary>
        public override object objValue
        {
            get
            {
                return ((ISelectExpression)expression).objValue;
            }
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

        private SelectExpression(UnitCollection array) : base()
        {
            InitializeSymbols();
            expression = SelectExpressions.Expression.Create(ref cells, array);
        }

        internal static void InitializeSymbols()
        {
            ArithmeticExpression.InitializeSymbols();

            SymbolStartError = ArithmeticExpression.SymbolStartError;
            SymbolEndError = ArithmeticExpression.SymbolEndError;

            SymbolIf = string.IsNullOrWhiteSpace(SymbolIf) ? @"IF" : SymbolIf;
            SymbolThen = string.IsNullOrWhiteSpace(SymbolThen) ? @"THEN" : SymbolThen;
            SymbolElse = string.IsNullOrWhiteSpace(SymbolElse) ? @"ELSE" : SymbolElse;
        }


        public static bool IsExpression(string text)
        {
            string context = text.Replace(" ", "");
            Match matchIf = regexIf.Match(context);
            return (matchIf.Length > 0 && matchIf.Index == 0);            
        }

        public static ISelectExpression Create(string text)
        {
            UnitCollection collection = UnitCollection.Create(text: text);
            return Create(collection: collection);
        }

        public static ISelectExpression Create(string text, string cellpattern)
        {
            UnitCollection collection = UnitCollection.Create(text: text, cellpattern: cellpattern);
            return Create(collection: collection);
        }

        public static ISelectExpression Create(UnitCollection collection)
        {
            return new SelectExpression(collection);
        }
    }
}