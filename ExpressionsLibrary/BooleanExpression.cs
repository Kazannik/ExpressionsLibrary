using System.Text.RegularExpressions;

namespace ExpressionsLibrary
{
	class BooleanExpression : BooleanExpressions.ExpressionBase, LogicExpressions.ILogicExpression
	{
		#region РЕГУЛЯРНЫЕ ВЫРАЖЕНИЯ

		/// <summary>
		/// Логическое отрицание (NOT).
		/// </summary>
		private const string NOT = @"((\x21)|(not))"; // "not" / "!"

		/// <summary>
		/// Логическое сложение (AND).
		/// </summary>
		private const string AND = @"((\x26)|(and))"; // "and" / "&"

		/// <summary>
		/// Логическое ИЛИ (OR).
		/// </summary>
		private const string OR = @"((\x7c)|(or))"; // "or" / "|"

		/// <summary>
		/// Исключающее ИЛИ (XOR).
		/// </summary>
		private const string XOR = @"(xor)"; // "xor"

		/// <summary>
		/// Положительное выражение (True).
		/// </summary>
		private const string TRUE = @"(true)"; // "true"

		/// <summary>
		/// Отрицательное выражение (False).
		/// </summary>
		private const string FALSE = @"(false)"; // "false"

		/// <summary>
		/// Коллекция логических знаков.
		/// </summary>
		private const string csBoolean = AND + @"|" + NOT + @"|" + OR + @"|" + XOR + @"|" + TRUE + @"|" + FALSE;

		/// <summary>
		/// Регулярное выражение для поиска всех компонентов, включая ячейки.
		/// </summary>
		private static Regex regexAll;

		/// <summary>
		/// Регулярное выражение для поиска знака логического отрицания [NOT].
		/// </summary>
		internal static Regex regexNot = new Regex(NOT, ArithmeticExpression.OPTIONS); // "not"

		/// <summary>
		/// Регулярное выражение для поиска знака логического сложения [AND].
		/// </summary>
		internal static Regex regexAnd = new Regex(AND, ArithmeticExpression.OPTIONS); // "and"

		/// <summary>
		/// Регулярное выражение для поиска знака логического или [OR].
		/// </summary>
		internal static Regex regexOr = new Regex(OR, ArithmeticExpression.OPTIONS); // "or"

		/// <summary>
		/// Регулярное выражение для поиска знака исключающего или [XOR].
		/// </summary>
		internal static Regex regexXor = new Regex(XOR, ArithmeticExpression.OPTIONS); // "xor"

		/// <summary>
		/// Регулярное выражение для поиска знака положительного выражения [True].
		/// </summary>
		internal static Regex regexTrue = new Regex(TRUE, ArithmeticExpression.OPTIONS); // "true"

		/// <summary>
		/// Регулярное выражение для поиска знака отрицательного выражения [False].
		/// </summary>
		internal static Regex regexFalse = new Regex(FALSE, ArithmeticExpression.OPTIONS); // "false"

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
		public override bool Value => ((LogicExpressions.ILogicExpression)expression).Value;

		/// <summary>
		/// Признак содержания ошибки в выражении.
		/// </summary>
		public override bool IsError => expression.IsError;

		/// <summary>
		/// Строковое представление логического выражения.
		/// </summary>
		public override string Formula() => expression.Formula();

		/// <summary>
		/// Короткое строковое представление логического выражения.
		/// </summary>
		/// <param name="format">Формат отображения результата алгебраического выражения.</param>
		public override string ToString(string format) => expression.ToString(format: format);

		private BooleanExpression(UnitCollection array) : base()
		{
			InitializeSymbols();
			expression = BooleanExpressions.Expression.Create(ref collection, array);
		}

		internal static void InitializeSymbols()
		{
			LogicExpression.InitializeSymbols();

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
			regexAll = new Regex(csBoolean, ArithmeticExpression.OPTIONS);
			return regexAll.IsMatch(context);
		}

		public static LogicExpressions.ILogicExpression Create(string text)
		{
			string context = text.Replace(" ", "");
			regexAll = new Regex(LogicExpression.LOGIC + @"|" + csBoolean + @"|" + ArithmeticExpression.csArithmetic + @"|" + ArithmeticExpression.OPEN + @"|" + ArithmeticExpression.CLOSE, ArithmeticExpression.OPTIONS);
			UnitCollection collection = UnitCollection.Create(regexAll.Matches(context));
			return new BooleanExpression(collection);
		}

		public static LogicExpressions.ILogicExpression Create(string text, string cellPattern)
		{
			string context = text.Replace(" ", "");
			regexAll = new Regex(@"(" + cellPattern + @")|" + LogicExpression.LOGIC + @"|" + csBoolean + @"|" + ArithmeticExpression.csArithmetic + @"|" + ArithmeticExpression.OPEN + @"|" + ArithmeticExpression.CLOSE, ArithmeticExpression.OPTIONS);
			ArithmeticExpression.regexCell = new Regex(@"(" + cellPattern + @")", ArithmeticExpression.OPTIONS);
			UnitCollection collection = UnitCollection.Create(regexAll.Matches(context));
			return new BooleanExpression(collection);
		}
	}
}
