using System;

namespace ExpressionsLibrary
{
	/// <summary>
	/// Математическое выражение.
	/// </summary>
	public class Expression : IExpression
	{
		private readonly IExpression expression;

		private Expression(IExpression expression) => this.expression = expression;

		public bool IsError => expression.IsError;

		/// <summary>
		/// Определяемые пользователем данные, связанные с этим объектом.
		/// </summary>
		public object Tag { get; set; }

		/// <summary>
		/// Значение выражения.
		/// </summary>
		public object ObjValue => expression.ObjValue;

		public string Formula() => expression.Formula();

		public string ToString(string format) => expression.ToString(format: format);

		/// <summary>
		/// Определяет содержится ли ячейка с указанным ключем в выражении.
		/// </summary>
		/// <param name="key">Ключ ячейки.</param>
		/// <returns></returns>
		public bool Contains(string key) => expression.Contains(key);

		/// <summary>
		/// Коллекция ключей ячеек.
		/// </summary>
		public string[] Keys => expression.Keys;

		/// <summary>
		/// Ячейка, используемая при расчете.
		/// </summary>
		/// <param name="key">Ключ ячейки.</param>
		/// <returns></returns>
		public ICell this[string key] => expression[key];

		/// <summary>
		/// Количество ячеек, используемых при расчете.
		/// </summary>
		public int Count => expression.Count;

		public static IExpression Create(string text)
		{
			bool isArithmetic = ArithmeticExpression.IsExpression(text: text);
			bool isBoolean = BooleanExpression.IsExpression(text: text);
			bool isLogic = LogicExpression.IsExpression(text: text);

			if (isBoolean)
				return new Expression(BooleanExpression.Create(text));
			else if (isLogic)
				return new Expression(LogicExpression.Create(text));
			else if (isArithmetic)
				return new Expression(ArithmeticExpression.Create(text));
			else
				throw new ArgumentException("Текстовая строка не содержит элементов выражения.");

		}

		public static IExpression Create(string text, string cellPattern)
		{
			bool isArithmetic = ArithmeticExpression.IsExpression(text: text);
			bool isBoolean = BooleanExpression.IsExpression(text: text);
			bool isLogic = LogicExpression.IsExpression(text: text);

			if (isBoolean)
				return new Expression(BooleanExpression.Create(text, cellPattern: cellPattern));
			else if (isLogic)
				return new Expression(LogicExpression.Create(text, cellPattern: cellPattern));
			else if (isArithmetic)
				return new Expression(ArithmeticExpression.Create(text, cellPattern: cellPattern));
			else
				throw new ArgumentException("Текстовая строка не содержит элементов выражения.");
		}
	}
}
