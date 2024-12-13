using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ExpressionsLibrary.ArithmeticExpressions
{
	
	/// <include file='XmlDocs/CommonXmlDocComments.xml' path='CommonXmlDocComments/Cells/Member[@name="Cell"]/*' />
	class CellExpression : ExpressionBase, ICell
	{
		string format;
		string formulaFormat;
		decimal value;

		readonly List<CellRegex> cellFormat;

		CellExpression(string key, string formula) : this(key: key, formula: formula, value: 0, format: string.Empty) { }
		CellExpression(string key) : this(key: key, formula: "[{key}:{value}]", value: 0, format: string.Empty) { }
		CellExpression(string key, string formula, decimal value, string format)
		{
			cellFormat = new List<CellRegex>();
			Key = key;
			IsError = false;

			this.value = value;
			Format = format;
			FormulaFormat = formula;
		}

		delegate string GetArgDelegate();

		string GetKey()
		{
			return Key;
		}
		string GetValue()
		{
			return GetStringValue.Invoke(Value);
		}

		delegate string GetValueDelegate(decimal value);

		GetValueDelegate GetStringValue;

		string ClearFormat(decimal value)
		{
			return value.ToString();
		}
		string DecimalFormat(decimal value)
		{
			return value.ToString(this.Format);
		}

		/// <summary>
		/// Признак содержания ошибки в выражении.
		/// </summary>
		public override bool IsError { get; }

		/// <include file='XmlDocs/CommonXmlDocComments.xml' path='CommonXmlDocComments/Cells/Member[@name="Key"]/*' />
		public string Key { get; }

		/// <summary>
		/// Значение алгебраического выражения.
		/// </summary>
		public override decimal Value { get { return value; } }

		/// <include file='XmlDocs/CommonXmlDocComments.xml' path='CommonXmlDocComments/Cells/Member[@name="Format"]/*' />
		public string Format
		{
			get { return format; }
			set
			{
				format = value;
				if (string.IsNullOrWhiteSpace(format))
					GetStringValue = ClearFormat;
				else
					GetStringValue = DecimalFormat;
			}
		}

		/// <summary>
		/// Формат отображения формулы ячейки.
		/// </summary>
		public string FormulaFormat
		{
			get { return formulaFormat; }
			set
			{
				formulaFormat = value;
				cellFormat.Clear();
				cellFormat.Add(new CellRegex(ArithmeticExpression.regexCellKey.Match(formulaFormat), GetKey));
				cellFormat.Add(new CellRegex(ArithmeticExpression.regexCellValue.Match(formulaFormat), GetValue));
				cellFormat.Sort(delegate (CellRegex x, CellRegex y)
				{
					return x.Match.Index.CompareTo(y.Match.Index);
				});
			}
		}

		/// <summary>
		/// Строковое представление алгебраического выражения.
		/// </summary>
		public override string Formula()
		{
			if (cellFormat.Count == 0)
				return formulaFormat;
			else
			{
				string result = string.Empty;
				int LastLength = 0;
				for (int i = 0; i < cellFormat.Count; i++)
				{
					result += formulaFormat.Substring(LastLength, cellFormat[i].Match.Index - LastLength) + cellFormat[i].Value.Invoke();
					LastLength = cellFormat[i].Match.Index + cellFormat[i].Match.Length;
				}
				result += formulaFormat.Substring(LastLength);
				return result;
			}
		}

		/// <include file='XmlDocs/CommonXmlDocComments.xml' path='CommonXmlDocComments/Cells/Member[@name="SetValue"]/*' />
		public void SetValue(decimal value)
		{
			this.value = value;
		}

		public static IExpression Create(string key)
		{
			return new CellExpression(key);
		}

		private readonly struct CellRegex
		{
			public CellRegex(Match match, GetArgDelegate fun)
			{
				this.match = match;
				value = fun;
			}

			readonly GetArgDelegate value;
			
			readonly Match match;
			public GetArgDelegate Value { get { return value; } }
			public Match Match { get { return match; } }
		}
	}
}
