using System.Collections.Generic;
using System.Linq;

namespace ExpressionsLibrary
{
	/// <summary>
	/// Базовый класс выражений.
	/// </summary>
	abstract class ExpressionBase : IExpression
	{
		protected IExpression expression;

		protected Dictionary<string, ICell> collection;

		protected ExpressionBase()
		{
			collection = new Dictionary<string, ICell>();
		}

		/// <summary>
		/// Признак содержания ошибки в выражении.
		/// </summary>
		public abstract bool IsError { get; }

		/// <summary>
		/// Значение выражения.
		/// </summary>
		public abstract object ObjValue { get; }

		/// <summary>
		/// Строковое представление выражения.
		/// </summary>
		public abstract string Formula();

		/// <summary>
		/// Короткое строковое представление выражения.
		/// </summary>
		/// <returns></returns>
		public override string ToString() => ToString(format: string.Empty);

		/// <summary>
		/// Короткое строковое представление выражения.
		/// </summary>
		/// <param name="format">Строка, описывающая формат отображения результата алгебраического выражения.</param>
		/// <returns></returns>
		public abstract string ToString(string format);

		/// <summary>
		/// Признак применения формата.
		/// </summary>
		/// <param name="format">Строка, описывающая формат отображения результата алгебраического выражения.</param>
		/// <returns></returns>
		protected static bool IsFormat(string format) => !string.IsNullOrWhiteSpace(format);

		/// <summary>
		/// Определяет содержится ли ячейка с указанным ключем в выражении.
		/// </summary>
		/// <param name="key">Ключ ячейки.</param>
		/// <returns></returns>
		public bool Contains(string key) => collection.ContainsKey(key);

		/// <summary>
		/// Коллекция ключей ячеек.
		/// </summary>
		public string[] Keys => collection.Keys.ToArray();

		/// <summary>
		/// Ячейка, используемая при расчете.
		/// </summary>
		/// <param name="key">Ключ ячейки.</param>
		/// <returns></returns>
		public ICell this[string key] => collection[key];

		/// <summary>
		/// Количество ячеек, используемых при расчете.
		/// </summary>
		public int Count => collection.Count;

		protected static string ClearText(string text)
		{
			if (text.IndexOf((char)32) >= 0)
				return text.Replace(" ", "");
			else
				return text;
		}
	}
}
