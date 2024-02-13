using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ExpressionsLibrary.ArithmeticExpressions
{
    /// <summary>
    /// Ячейка, используемая при расчете.
    /// </summary>
    class CellExpression : ExpressionBase, ICell
    {
        string format;
        string formulaformat;
        decimal value;

        List<CellRegex> cellFormat;

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

        /// <summary>
        /// Ключ для доступа к ячеке расчетов.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Значение алгебраического выражения.
        /// </summary>
        public override decimal Value { get { return value; } }
        /// <summary>
        /// Формат отображения формулы числа.
        /// </summary>
        public string Format {
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
            get { return formulaformat; }
            set
            {
                formulaformat = value;
                cellFormat.Clear();
                cellFormat.Add(new CellRegex(ArithmeticExpression.regexCellKey.Match(formulaformat), GetKey));
                cellFormat.Add(new CellRegex(ArithmeticExpression.regexCellValue.Match(formulaformat), GetValue));
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
                return formulaformat;
            else
            {
                string result=string.Empty;
                int LastLength = 0;
                for (int i = 0; i < cellFormat.Count; i++)
                {
                    result += formulaformat.Substring(LastLength, cellFormat[i].Match.Index - LastLength) + cellFormat[i].Value.Invoke();
                    LastLength = cellFormat[i].Match.Index + cellFormat[i].Match.Length;
                }
                result += formulaformat.Substring(LastLength);
                return result;
            }            
        }

        /// <summary>
        /// Присвоить значение ячейке.
        /// </summary>
        /// <param name="value">Значение ячейки.</param>
        public void SetValue(decimal value)
        {
            this.value = value;
        }
               
        public static IExpression Create(string key)
        {
            return new CellExpression(key);
        }

        private struct CellRegex
        {
            public CellRegex(Match match, GetArgDelegate fun)
            {
                this.match = match;
                value = fun;
            }
            GetArgDelegate value;
            Match match;
            public GetArgDelegate Value { get { return value; } }
            public Match Match { get { return match; } }
        }
    }
}
