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
		internal const string DECIMAL = @"((\d{1,}[.]\d{1,})|(\d{1,}[,]\d{1,})|(\d{1,}))";
		/// <summary>
		/// Возведение в степень [pow],[^].
		/// </summary>
		internal const string POWER = @"((\x5e)|(pow))"; // "pow" / "^"
		/// <summary>
		/// Корень [sqrt].
		/// </summary>
		internal const string SQRT = @"((\u221a)|(sqrt))"; // "sqrt" / "√"
		/// <summary>
		/// Умножение [*].
		/// </summary>
		internal const string MULTIPLICATION = @"((\x78)|(\x2a))"; // "*" / "x"
		/// <summary>
		/// Деление [/].
		/// </summary>
		internal const string DIVISION = @"(\x2f)"; // "/"
		/// <summary>
		/// Целочисленное деление [fix],[\].
		/// </summary>
		internal const string FIX = @"((\x5c)|(fix))";// "fix" / "\"
		/// <summary>
		/// Остаток от деления [mod],[%].
		/// </summary>
		internal const string MOD = @"((\x25)|(mod))"; // "mod" / "%"
		/// <summary>
		/// Сложение [+].
		/// </summary>
		internal const string ADDITION = @"(\x2b)"; // "+"
		/// <summary>
		/// Вычитание [-].
		/// </summary>
		internal const string SUBTRACTION = @"(\x2d)"; // "-"
		/// <summary>
		/// Открывающаяся скобка [(].
		/// </summary>
		internal const string OPEN = @"(\x28)"; // "("
		/// <summary>
		/// Закрывающаяся скобка [)].
		/// </summary>
		internal const string CLOSE = @"(\x29)"; // ")"        
		/// <summary>
		/// Ключ ячейки в формате формулы.
		/// </summary>
		private const string CELL_KEY = @"\x7b(key)\x7d"; // "{key}"
		/// <summary>
		/// Значение ячейки в формате формулы.
		/// </summary>
		private const string CELL_VALUE = @"\x7b(value)\x7d"; // "{value}"

		/// <summary>
		/// Коллекция арифметических знаков.
		/// </summary>
		internal const string csArithmetic = DECIMAL + @"|" + ADDITION + @"|" + DIVISION + @"|" + FIX + @"|" + MOD + @"|" + MULTIPLICATION + @"|" + POWER + @"|" + SQRT + @"|" + SUBTRACTION;

		internal const RegexOptions OPTIONS = RegexOptions.IgnoreCase | RegexOptions.Compiled;

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
		internal static Regex regexDecimal = new Regex(DECIMAL, OPTIONS);
		/// <summary>
		/// Регулярное выражение для поиска знака pow (возведение в степень).
		/// </summary>
		internal static Regex regexPower = new Regex(POWER, OPTIONS); // "pow" / "^"
		/// <summary>
		/// Регулярное выражение для поиска знака sqrt (корень).
		/// </summary>
		internal static Regex regexSqrt = new Regex(SQRT, OPTIONS); // "sqrt / "√"
		/// <summary>
		/// Регулярное выражение для поиска знака умножения [*].
		/// </summary>
		internal static Regex regexMultiplication = new Regex(MULTIPLICATION, OPTIONS); // "*"
		/// <summary>
		/// Регулярное выражение для поиска знака деления [/].
		/// </summary>
		internal static Regex regexDivision = new Regex(DIVISION, OPTIONS); // "/"
		/// <summary>
		/// Регулярное выражение для поиска знака fix (целое от деления).
		/// </summary>
		internal static Regex regexFix = new Regex(FIX, OPTIONS);// "fix" / "\"
		/// <summary>
		/// Регулярное выражение для поиска знака mod (остаток от деления).
		/// </summary>
		internal static Regex regexMod = new Regex(MOD, OPTIONS); // "mod" / "%"
		/// <summary>
		/// Регулярное выражение для поиска знака сложения [+].
		/// </summary>
		internal static Regex regexAddition = new Regex(ADDITION, OPTIONS); // "+"
		/// <summary>
		/// Регулярное выражение для поиска знака вычитания [-].
		/// </summary>
		internal static Regex regexSubtracting = new Regex(SUBTRACTION, OPTIONS); // "-"
		/// <summary>
		/// Регулярное выражение для поиска знака открывающейся скобки [(].
		/// </summary>
		internal static Regex regexOpen = new Regex(OPEN, OPTIONS); // "("
		/// <summary>
		/// Регулярное выражение для поиска знака закрывающейся скобки [)].
		/// </summary>
		internal static Regex regexClose = new Regex(CLOSE, OPTIONS); // ")"
		/// <summary>
		/// Ключ ячейки в формате формулы.
		/// </summary>
		internal static Regex regexCellKey = new Regex(CELL_KEY, OPTIONS); // "{key}"
		/// <summary>
		/// Значение ячейки в формате формулы.
		/// </summary>
		internal static Regex regexCellValue = new Regex(CELL_VALUE, OPTIONS); // "{value}"
		#endregion

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
			get { return ((ArithmeticExpressions.IExpression)expression).Value; }
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

		private ArithmeticExpression(UnitCollection array) : base()
		{
			InitializeSymbols();
			expression = ArithmeticExpressions.Expression.Create(ref collection, array);
		}

		internal static void InitializeSymbols()
		{
			SymbolSpace = string.IsNullOrWhiteSpace(SymbolSpace) ? @" " : SymbolSpace;

			SymbolStartError = string.IsNullOrWhiteSpace(SymbolStartError) ? @"[Error:" : SymbolStartError;
			SymbolEndError = string.IsNullOrWhiteSpace(SymbolEndError) ? @"]" : SymbolEndError;

			SymbolAddition = string.IsNullOrWhiteSpace(SymbolAddition) ? @"+" : SymbolAddition;
			SymbolDivision = string.IsNullOrWhiteSpace(SymbolDivision) ? @"/" : SymbolDivision;
			SymbolFix = string.IsNullOrWhiteSpace(SymbolFix) ? @"\" : SymbolFix;
			SymbolMod = string.IsNullOrWhiteSpace(SymbolMod) ? @"%" : SymbolMod;
			SymbolMultiplication = string.IsNullOrWhiteSpace(SymbolMultiplication) ? "x" : SymbolMultiplication;
			SymbolPower = string.IsNullOrWhiteSpace(SymbolPower) ? @"^" : SymbolPower;
			SymbolSqrt = string.IsNullOrWhiteSpace(SymbolSqrt) ? @"√" : SymbolSqrt;
			SymbolSubtracting = string.IsNullOrWhiteSpace(SymbolSubtracting) ? @"-" : SymbolSubtracting;
		}

		public static bool IsExpression(string text)
		{
			string context = text.Replace(" ", "");
			regexAll = new Regex(csArithmetic + @"|" + OPEN + @"|" + CLOSE, OPTIONS);
			return regexAll.IsMatch(context);
		}

		public static IExpression Create(string text)
		{
			string context = text.Replace(" ", "");
			regexAll = new Regex(csArithmetic + @"|" + OPEN + @"|" + CLOSE, OPTIONS);
			UnitCollection collection = UnitCollection.Create(regexAll.Matches(context));
			return new ArithmeticExpression(collection);
		}

		public static IExpression Create(string text, string cellpattern)
		{
			string context = text.Replace(" ", "");
			regexAll = new Regex(@"(" + cellpattern + @")|" + csArithmetic + @"|" + OPEN + @"|" + CLOSE, OPTIONS);
			regexCell = new Regex(@"(" + cellpattern + @")", OPTIONS);
			UnitCollection collection = UnitCollection.Create(regexAll.Matches(context));
			return new ArithmeticExpression(collection);
		}
	}
}