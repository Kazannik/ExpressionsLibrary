// Ignore Spelling: Sqrt

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExpressionsLibrary
{
	/// <summary>
	/// Коллекция элементов алгебраического действия.
	/// </summary>
	class UnitCollection : IEnumerable<UnitCollection.IUnit>
	{
		private readonly List<IUnit> List;

		public UnitCollection() => List = new List<IUnit>();

		public static UnitCollection Create(IUnit unit)
		{
			UnitCollection result = new UnitCollection();
			result.List.Add(BaseUnit.Create(result, unit));
			return result;
		}

		public static UnitCollection Create(UnitCollection array)
		{
			UnitCollection result = new UnitCollection();
			foreach (IUnit u in array)
			{
				result.List.Add(BaseUnit.Create(result, u));
			}
			return result;
		}

		public static UnitCollection Create(UnitCollection array, int start) => 
			Create(array, start, array.List.Count - start);
		
		public static UnitCollection Create(UnitCollection array, int start, int length)
		{
			UnitCollection result = new UnitCollection();
			for (int i = 0; i < length; i++)
			{
				result.List.Add(BaseUnit.Create(result, array.List[start + i]));
			}
			return result;
		}

		public static UnitCollection Create(MatchCollection array)
		{
			UnitCollection result = new UnitCollection();
			foreach (Match m in array)
			{
				result.List.Add(BaseUnit.Create(result, m));
			}
			return result;
		}

		public IUnit First => List.First(); 

		public IUnit Last => List.Last(); 

		public IUnit this[int index] => List[index]; 

		/// <summary>
		/// Выражение содержит ошибку.
		/// </summary>
		public bool IsError
		{
			get
			{
				bool first = IsFirstError();
				if (first) { return true; }
				bool last = IsLastError();
				if (last) { return true; }
				return false;
			}
		}

		public bool IsContainsError() => List.Any(x => x.Action < 0);
			
		public bool IsFirstError() => List.Any() && First.IsArithmetic || First.IsBoolean || First.IsLogic;
			
		public bool IsLastError() => List.Any() && Last.IsArithmetic || Last.IsBoolean || Last.IsLogic;
		
		/// <summary>
		/// Алгебраическое выражение.
		/// </summary>
		public bool IsArithmetic => !IsLogic && List.Any(x => x.Action >= 1 && x.Action <= 6);
				
		/// <summary>
		/// Логическое выражение.
		/// </summary>
		public bool IsLogic => !IsBoolean && List.Any(x => x.Action >= 7 && x.Action <= 12);
			
		/// <summary>
		/// Булево логическое выражение.
		/// </summary>
		public bool IsBoolean => List.Any(x => x.Action >= 13 && x.Action <= 16);
			

		/// <summary>
		/// Выражение целиком заключено в скобки (X+Y).
		/// </summary>
		public bool IsAssociation => OpenIndex() == 0; 

		/// <summary>
		/// Отрицательное выражение заключенное в скобки -(X+Y)
		/// </summary>
		public bool IsNegativeAssociation => OpenIndex() == 1 && First.UnitType == MatchType.Subtracting; 

		/// <summary>
		/// Отрицательное выражение заключенное в скобки Not(... )
		/// </summary>
		public bool IsNotAssociation => OpenIndex() == 1 && First.UnitType == MatchType.Not; 

		/// <summary>
		/// Положительное выражение заключенное в скобки +(X+Y)
		/// </summary>
		public bool IsPositiveAssociation => OpenIndex() == 1 && First.UnitType == MatchType.Addition; 

		public bool IsAssociationError
		{
			get
			{
				int A = 0;
				bool association = false;
				for (int i = List.Count - 1; i >= 0; i--)
				{
					if (List[i].UnitType == MatchType.Close) { A--; }
					if (List[i].UnitType == MatchType.Open) { A++; }
					if (A == 0) { association = true; }
				}
				return association & (A != 0);
			}
		}

		public int Count => List.Count; 

		private int OpenIndex()
		{
			if (Last.UnitType == MatchType.Close)
			{
				int A = 0;
				for (int i = List.Count - 1; i >= 0; i--)
				{
					if (List[i].UnitType == MatchType.Close) { A--; }
					if (List[i].UnitType == MatchType.Open) { A++; }
					if (A == 0) { return i; }
				}
				return -1;
			}
			else { return -1; }
		}

		public int GetLastIndex()
		{
			IEnumerable<int> array = from IUnit U in List
									 where U.Action > 0
									 orderby U.Action descending
									 group U by U.Action into G
									 select G.First().Action;
			bool association = false;
			foreach (int i in array)
			{
				int F = GetLastIndex(i);
				if (F == -2) { association = true; }
				if (F >= 0) { return F; }
			}
			if (association) { return -2; }
			else { return -1; }
		}

		public int GetLastIndex(int action)
		{
			int A = 0;
			bool association = false;
			for (int i = Count - 1; i >= 0; i--)
			{
				if (List[i].UnitType == MatchType.Close) { A--; association = true; }
				if (List[i].UnitType == MatchType.Open) { A++; association = true; }
				if (A == 0 && List[i].Action == action) { return i; }
			}
			if (association) { return -2; }
			else { return -1; }
		}

		IEnumerator<IUnit> IEnumerable<IUnit>.GetEnumerator() => List.GetEnumerator();
		
		public IEnumerator GetEnumerator() => List.GetEnumerator();
		
		/// <summary>
		/// Интерфейс базового алгебраического действия.
		/// </summary>
		public interface IUnit
		{
			/// <summary>
			/// Тип элемента.
			/// </summary>
			MatchType UnitType { get; }

			/// <summary>
			/// Приоритет математического действия.
			/// </summary>
			int Action { get; }

			/// <summary>
			/// Признак оператора арифметического выражения.
			/// </summary>
			bool IsArithmetic { get; }

			/// <summary>
			/// Признак оператора булевого выражения.
			/// </summary>
			bool IsBoolean { get; }

			/// <summary>
			/// Признак оператора логического выражения.
			/// </summary>
			bool IsLogic { get; }

			/// <summary>
			/// Строковое значение элемента.
			/// </summary>
			string Value { get; }

			/// <summary>
			/// Индекс элемента в родительской коллекции.
			/// </summary>
			int Index { get; }
		}

		/// <summary>
		/// Базовый элемент алгебраического действия.
		/// </summary>
		private abstract class BaseUnit : IUnit
		{
			private readonly UnitCollection parent;

			/// <summary>
			/// Тип элемента.
			/// </summary>
			public MatchType UnitType { get; }

			/// <summary>
			/// Приоритет математического действия.
			/// </summary>
			public int Action { get; }

			/// <summary>
			/// Признак оператора арифметического выражения.
			/// </summary>
			public bool IsArithmetic { get; }

			/// <summary>
			/// Признак оператора булевого выражения.
			/// </summary>
			public bool IsBoolean { get; }

			/// <summary>
			/// Признак оператора логического выражения.
			/// </summary>
			public bool IsLogic { get; }

			/// <summary>
			/// Строковое значение элемента.
			/// </summary>
			public string Value { get; }

			/// <summary>
			/// Индекс элемента в родительской коллекции.
			/// </summary>
			public int Index => parent.List.IndexOf(this);
			
			/// <summary>
			/// Конструктор базового элемента алгебраического действия
			/// </summary>
			/// <param name="parent">Родительская коллекция элементов.</param>
			/// <param name="value">Строковое значение элемента.</param>
			/// <param name="type">Тип элемента.</param>
			/// <param name="action">Приоритет математического действия.</param>
			/// <param name="arithmetic">Признак оператора арифметического выражения.</param>
			/// <param name="boolean">Признак оператора булевого выражения.</param>
			/// <param name="logic">Признак оператора логического выражения.</param>
			protected BaseUnit(UnitCollection parent, string value, MatchType type, int action, bool arithmetic, bool boolean, bool logic)
			{
				this.parent = parent;
				Value = value;
				UnitType = type;
				Action = action;
				IsArithmetic = arithmetic;
				IsBoolean = boolean;
				IsLogic = logic;
			}

			public static BaseUnit Create(UnitCollection parent, IUnit unit)
			{
				string value = unit.Value;
				switch (unit.UnitType)
				{
					case MatchType.Addition:
						return new AdditionUnit(parent: parent, value: value);
					case MatchType.And:
						return new AndUnit(parent: parent, value: value);
					case MatchType.Cell:
						return new CellUnit(parent: parent, value: value);
					case MatchType.Close:
						return new CloseUnit(parent: parent, value: value);
					case MatchType.Decimal:
						return new DecimalUnit(parent: parent, value: value);
					case MatchType.Division:
						return new DivisionUnit(parent: parent, value: value);
					case MatchType.Equal:
						return new EqualUnit(parent: parent, value: value);
					case MatchType.False:
						return new FalseUnit(parent: parent, value: value);
					case MatchType.Fix:
						return new FixUnit(parent: parent, value: value);
					case MatchType.Less:
						return new LessUnit(parent: parent, value: value);
					case MatchType.LessOrEqual:
						return new LessOrEqualUnit(parent: parent, value: value);
					case MatchType.Mod:
						return new ModUnit(parent: parent, value: value);
					case MatchType.More:
						return new MoreUnit(parent: parent, value: value);
					case MatchType.MoreOrEqual:
						return new MoreOrEqualUnit(parent: parent, value: value);
					case MatchType.Multiplication:
						return new MultiplicationUnit(parent: parent, value: value);
					case MatchType.Negative:
						return new NegativeUnit(parent: parent, value: value);
					case MatchType.Not:
						return new NotUnit(parent: parent, value: value);
					case MatchType.NotEqual:
						return new NotEqualUnit(parent: parent, value: value);
					case MatchType.Open:
						return new OpenUnit(parent: parent, value: value);
					case MatchType.Or:
						return new OrUnit(parent: parent, value: value);
					case MatchType.Power:
						return new PowerUnit(parent: parent, value: value);
					case MatchType.Sqrt:
						return new SqrtUnit(parent: parent, value: value);
					case MatchType.Subtracting:
						return new SubtractingUnit(parent: parent, value: value);
					case MatchType.True:
						return new TrueUnit(parent: parent, value: value);
					case MatchType.Xor:
						return new XorUnit(parent: parent, value: value);
					default:
						return new DecimalUnit(parent: parent, value: value);
				}
			}

			public static BaseUnit Create(UnitCollection parent, Match match)
			{
				MatchType type = GetMatchType(match);
				string value = match.Value;
				switch (type)
				{
					case MatchType.Addition:
						return new AdditionUnit(parent: parent, value: value);
					case MatchType.And:
						return new AndUnit(parent: parent, value: value);
					case MatchType.Cell:
						return new CellUnit(parent: parent, value: value);
					case MatchType.Close:
						return new CloseUnit(parent: parent, value: value);
					case MatchType.Decimal:
						return new DecimalUnit(parent: parent, value: value);
					case MatchType.Division:
						return new DivisionUnit(parent: parent, value: value);
					case MatchType.Equal:
						return new EqualUnit(parent: parent, value: value);
					case MatchType.False:
						return new FalseUnit(parent: parent, value: value);
					case MatchType.Fix:
						return new FixUnit(parent: parent, value: value);
					case MatchType.Less:
						return new LessUnit(parent: parent, value: value);
					case MatchType.LessOrEqual:
						return new LessOrEqualUnit(parent: parent, value: value);
					case MatchType.Mod:
						return new ModUnit(parent: parent, value: value);
					case MatchType.More:
						return new MoreUnit(parent: parent, value: value);
					case MatchType.MoreOrEqual:
						return new MoreOrEqualUnit(parent: parent, value: value);
					case MatchType.Multiplication:
						return new MultiplicationUnit(parent: parent, value: value);
					case MatchType.Negative:
						return new NegativeUnit(parent: parent, value: value);
					case MatchType.Not:
						return new NotUnit(parent: parent, value: value);
					case MatchType.NotEqual:
						return new NotEqualUnit(parent: parent, value: value);
					case MatchType.Open:
						return new OpenUnit(parent: parent, value: value);
					case MatchType.Or:
						return new OrUnit(parent: parent, value: value);
					case MatchType.Power:
						return new PowerUnit(parent: parent, value: value);
					case MatchType.Sqrt:
						return new SqrtUnit(parent: parent, value: value);
					case MatchType.Subtracting:
						return new SubtractingUnit(parent: parent, value: value);
					case MatchType.True:
						return new TrueUnit(parent: parent, value: value);
					case MatchType.Xor:
						return new XorUnit(parent: parent, value: value);
					default:
						return new ErrorUnit(parent: parent, value: value);
				}
			}
		}

		/// <summary>
		/// Ошибочный знак.(-1)
		/// </summary>
		private class ErrorUnit : BaseUnit, IUnit
		{
			public ErrorUnit(UnitCollection parent, string value) :
				base(parent: parent, value: value,
					type: MatchType.Error, action: -1, arithmetic: false, boolean: false, logic: false)
			{ }
		}

		/// <summary>
		/// Ссылка на ячейку.(0)
		/// </summary>
		private class CellUnit : BaseUnit, IUnit
		{
			public CellUnit(UnitCollection parent, string value) :
				base(parent: parent, value: value,
					type: MatchType.Cell, action: 0, arithmetic: false, boolean: false, logic: false)
			{ }
		}

		/// <summary>
		/// Числовой показатель.(0)
		/// </summary>
		private class DecimalUnit : BaseUnit, IUnit
		{
			public DecimalUnit(UnitCollection parent, string value) :
				base(parent: parent, value: value,
					type: MatchType.Decimal, action: 0, arithmetic: false, boolean: false, logic: false)
			{ }
		}

		/// <summary>
		/// Закрывающаяся скобка.(0)
		/// </summary>
		private class CloseUnit : BaseUnit, IUnit
		{
			public CloseUnit(UnitCollection parent, string value) :
				base(parent: parent, value: value,
					type: MatchType.Close, action: 0, arithmetic: false, boolean: false, logic: false)
			{ }
		}

		/// <summary>
		/// Открывающаяся скобка.(0)
		/// </summary>
		private class OpenUnit : BaseUnit, IUnit
		{
			public OpenUnit(UnitCollection parent, string value) :
				base(parent: parent, value: value,
					type: MatchType.Open, action: 0, arithmetic: false, boolean: false, logic: false)
			{ }
		}

		/// <summary>
		/// Знак степени числа.(1)
		/// </summary>
		private class PowerUnit : BaseUnit, IUnit
		{
			public PowerUnit(UnitCollection parent, string value) :
				base(parent: parent, value: value,
					type: MatchType.Power, action: 1, arithmetic: true, boolean: false, logic: false)
			{ }
		}

		/// <summary>
		/// Знак корня числа.(1)
		/// </summary>
		private class SqrtUnit : BaseUnit, IUnit
		{
			public SqrtUnit(UnitCollection parent, string value) :
				base(parent: parent, value: value,
					type: MatchType.Sqrt, action: 1, arithmetic: true, boolean: false, logic: false)
			{ }
		}

		/// <summary>
		/// Знак отрицательного числа.(2)
		/// </summary>
		private class NegativeUnit : BaseUnit, IUnit
		{
			public NegativeUnit(UnitCollection parent, string value) :
				base(parent: parent, value: value,
					type: MatchType.Negative, action: 2, arithmetic: false, boolean: false, logic: false)
			{ }
		}

		/// <summary>
		/// Знак умножения.(3)
		/// </summary>
		private class MultiplicationUnit : BaseUnit, IUnit
		{
			public MultiplicationUnit(UnitCollection parent, string value) :
				base(parent: parent, value: value,
					type: MatchType.Multiplication, action: 3, arithmetic: true, boolean: false, logic: false)
			{ }
		}

		/// <summary>
		/// Знак деления.(3)
		/// </summary>
		private class DivisionUnit : BaseUnit, IUnit
		{
			public DivisionUnit(UnitCollection parent, string value) :
				base(parent: parent, value: value,
					type: MatchType.Division, action: 3, arithmetic: true, boolean: false, logic: false)
			{ }
		}

		/// <summary>
		/// Вычисление целой части от деления.(4)
		/// </summary>
		private class FixUnit : BaseUnit, IUnit
		{
			public FixUnit(UnitCollection parent, string value) :
				base(parent: parent, value: value,
					type: MatchType.Fix, action: 4, arithmetic: true, boolean: false, logic: false)
			{ }
		}

		/// <summary>
		/// Вычисление остатка от деления.(5)
		/// </summary>
		private class ModUnit : BaseUnit, IUnit
		{
			public ModUnit(UnitCollection parent, string value) :
				base(parent: parent, value: value,
					type: MatchType.Mod, action: 5, arithmetic: true, boolean: false, logic: false)
			{ }
		}

		/// <summary>
		/// Знак сложения.(6)
		/// </summary>
		private class AdditionUnit : BaseUnit, IUnit
		{
			public AdditionUnit(UnitCollection parent, string value) :
				base(parent: parent, value: value,
					type: MatchType.Addition, action: 6, arithmetic: true, boolean: false, logic: false)
			{ }
		}

		/// <summary>
		/// Знак вычитания.(6)
		/// </summary>
		private class SubtractingUnit : BaseUnit, IUnit
		{
			public SubtractingUnit(UnitCollection parent, string value) :
				base(parent: parent, value: value,
					type: MatchType.Subtracting, action: 6, arithmetic: true, boolean: false, logic: false)
			{ }
		}

		/// <summary>
		/// Знак равно.(7)
		/// </summary>
		private class EqualUnit : BaseUnit, IUnit
		{
			public EqualUnit(UnitCollection parent, string value) :
				base(parent: parent, value: value,
					type: MatchType.Equal, action: 7, arithmetic: false, boolean: false, logic: true)
			{ }
		}

		/// <summary>
		/// Знак не равно (!=).(8)
		/// </summary>
		private class NotEqualUnit : BaseUnit, IUnit
		{
			public NotEqualUnit(UnitCollection parent, string value) :
				base(parent: parent, value: value,
					type: MatchType.NotEqual, action: 8, arithmetic: false, boolean: false, logic: true)
			{ }
		}

		/// <summary>
		/// Знак меньше (&lt;).(9)
		/// </summary>
		private class LessUnit : BaseUnit, IUnit
		{
			public LessUnit(UnitCollection parent, string value) :
				base(parent: parent, value: value,
					type: MatchType.Less, action: 9, arithmetic: false, boolean: false, logic: true)
			{ }
		}

		/// <summary>
		/// Знак больше (>).(10)
		/// </summary>
		private class MoreUnit : BaseUnit, IUnit
		{
			public MoreUnit(UnitCollection parent, string value) :
				base(parent: parent, value: value,
					type: MatchType.More, action: 10, arithmetic: false, boolean: false, logic: true)
			{ }
		}

		/// <summary>
		/// Знак меньше или равно (&lt;=).(11)
		/// </summary>
		private class LessOrEqualUnit : BaseUnit, IUnit
		{
			public LessOrEqualUnit(UnitCollection parent, string value) :
				base(parent: parent, value: value,
					type: MatchType.LessOrEqual, action: 11, arithmetic: false, boolean: false, logic: true)
			{ }
		}

		/// <summary>
		/// Знак больше или равно (>=).(12)
		/// </summary>
		private class MoreOrEqualUnit : BaseUnit, IUnit
		{
			public MoreOrEqualUnit(UnitCollection parent, string value) :
				base(parent: parent, value: value,
					type: MatchType.MoreOrEqual, action: 12, arithmetic: false, boolean: false, logic: true)
			{ }
		}

		/// <summary>
		/// Знак положительного логического выражения (True).(0)
		/// </summary>
		private class TrueUnit : BaseUnit, IUnit
		{
			public TrueUnit(UnitCollection parent, string value) :
				base(parent: parent, value: value,
					type: MatchType.True, action: 0, arithmetic: false, boolean: false, logic: false)
			{ }
		}

		/// <summary>
		/// Знак отрицательного логического выражения (False).(0)
		/// </summary>
		private class FalseUnit : BaseUnit, IUnit
		{
			public FalseUnit(UnitCollection parent, string value) :
				base(parent: parent, value: value,
					type: MatchType.False, action: 0, arithmetic: false, boolean: false, logic: false)
			{ }
		}

		/// <summary>
		/// Знак логического отрицания (NOT).(13)
		/// </summary>
		private class NotUnit : BaseUnit, IUnit
		{
			public NotUnit(UnitCollection parent, string value) :
				base(parent: parent, value: value,
					type: MatchType.Not, action: 13, arithmetic: false, boolean: false, logic: false)
			{ }
		}

		/// <summary>
		/// Знак логического сложения (AND).(14)
		/// </summary>
		private class AndUnit : BaseUnit, IUnit
		{
			public AndUnit(UnitCollection parent, string value) :
				base(parent: parent, value: value,
					type: MatchType.And, action: 14, arithmetic: false, boolean: true, logic: false)
			{ }
		}

		/// <summary>
		/// Знак логического ИЛИ (OR).(15)
		/// </summary>
		private class OrUnit : BaseUnit, IUnit
		{
			public OrUnit(UnitCollection parent, string value) :
				base(parent: parent, value: value,
					type: MatchType.Or, action: 15, arithmetic: false, boolean: true, logic: false)
			{ }
		}

		/// <summary>
		/// Знак исключающегося ИЛИ (XOR).(16)
		/// </summary>
		private class XorUnit : BaseUnit, IUnit
		{
			public XorUnit(UnitCollection parent, string value) :
				base(parent: parent, value: value,
					type: MatchType.Xor, action: 16, arithmetic: false, boolean: true, logic: false)
			{ }
		}

		/// <summary>
		/// Определить тип элемента.
		/// </summary>
		/// <param name="match">Класс совпадения регулярного выражения.</param>
		private static MatchType GetMatchType(Match match)
		{
			string value = match.Value;

			if (ArithmeticExpression.regexCell != null &&
				ArithmeticExpression.regexCell.IsMatch(value))
			{
				return MatchType.Cell;
			}
			else if (BooleanExpression.regexTrue.IsMatch(value))
			{
				return MatchType.True;
			}
			else if (BooleanExpression.regexFalse.IsMatch(value))
			{
				return MatchType.False;
			}
			else if (BooleanExpression.regexAnd.IsMatch(value))
			{
				return MatchType.And;
			}
			else if (BooleanExpression.regexXor.IsMatch(value))
			{
				return MatchType.Xor;
			}
			else if (BooleanExpression.regexOr.IsMatch(value))
			{
				return MatchType.Or;
			}
			else if (LogicExpression.regexLessOrEqual.IsMatch(value))
			{
				return MatchType.LessOrEqual;
			}
			else if (LogicExpression.regexMoreOrEqual.IsMatch(value))
			{
				return MatchType.MoreOrEqual;
			}
			else if (LogicExpression.regexNotEqual.IsMatch(value))
			{
				return MatchType.NotEqual;
			}
			else if (LogicExpression.regexEqual.IsMatch(value))
			{
				return MatchType.Equal;
			}
			else if (LogicExpression.regexLess.IsMatch(value))
			{
				return MatchType.Less;
			}
			else if (LogicExpression.regexMore.IsMatch(value))
			{
				return MatchType.More;
			}
			else if (BooleanExpression.regexNot.IsMatch(value))
			{
				return MatchType.Not;
			}
			else if (ArithmeticExpression.regexDecimal.IsMatch(value))
			{
				return MatchType.Decimal;
			}
			else if (ArithmeticExpression.regexAddition.IsMatch(value))
			{
				return MatchType.Addition;
			}
			else if (ArithmeticExpression.regexSubtracting.IsMatch(value))
			{
				return MatchType.Subtracting;
			}
			else if (ArithmeticExpression.regexMultiplication.IsMatch(value))
			{
				return MatchType.Multiplication;
			}
			else if (ArithmeticExpression.regexDivision.IsMatch(value))
			{
				return MatchType.Division;
			}
			else if (ArithmeticExpression.regexFix.IsMatch(value))
			{
				return MatchType.Fix;
			}
			else if (ArithmeticExpression.regexMod.IsMatch(value))
			{
				return MatchType.Mod;
			}
			else if (ArithmeticExpression.regexOpen.IsMatch(value))
			{
				return MatchType.Open;
			}
			else if (ArithmeticExpression.regexClose.IsMatch(value))
			{
				return MatchType.Close;
			}
			else if (ArithmeticExpression.regexPower.IsMatch(value))
			{
				return MatchType.Power;
			}
			else if (ArithmeticExpression.regexSqrt.IsMatch(value))
			{
				return MatchType.Sqrt;
			}
			else
			{
				return MatchType.Error;
			}
		}

		/// <summary>
		/// Тип элемента.
		/// </summary>
		public enum MatchType : int
		{
			/// <summary>
			/// Значение, содержащее ошибку.
			/// </summary>
			Error = -1,

			/// <summary>
			/// Ссылка на ячейку.
			/// </summary>
			Cell = 0,

			/// <summary>
			/// Числовой показатель.
			/// </summary>
			Decimal = 1,

			/// <summary>
			/// Открывающаяся скобка [(].
			/// </summary>
			Open = 2,

			/// <summary>
			/// Закрывающаяся скобка [)].
			/// </summary>
			Close = 3,

			/// <summary>
			/// Степень числа [^].
			/// </summary>
			Power = 4,

			/// <summary>
			/// Корень [sqrt].
			/// </summary>
			Sqrt = 5,

			/// <summary>
			/// Отрицательное число [-].
			/// </summary>
			Negative = 6,

			/// <summary>
			/// Знак умножения [*].
			/// </summary>
			Multiplication = 7,

			/// <summary>
			/// Знак деления [/].
			/// </summary>
			Division = 8,

			/// <summary>
			/// Знак операции получения целой части от деления [\].
			/// </summary>
			Fix = 9,

			/// <summary>
			/// Знак операции получения остатка от деления [%].
			/// </summary>
			Mod = 10,

			/// <summary>
			/// Знак сложения [+].
			/// </summary>
			Addition = 11,

			/// <summary>
			/// Знак вычитания [-].
			/// </summary>
			Subtracting = 12,

			/// <summary>
			/// Равно [=].
			/// </summary>
			Equal = 13,

			/// <summary>
			/// Не равно [&lt;>].
			/// </summary>
			NotEqual = 14,

			/// <summary>
			/// Меньше [&lt;].
			/// </summary>
			Less = 15,

			/// <summary>
			/// Больше [>].
			/// </summary>
			More = 16,

			/// <summary>
			/// Меньше или равно [&lt;=].
			/// </summary>
			LessOrEqual = 17,

			/// <summary>
			/// Больше или равно [>=].
			/// </summary>
			MoreOrEqual = 18,

			/// <summary>
			/// Положительное логическое выражение.
			/// </summary>
			True = 19,

			/// <summary>
			/// Отрицательное логическое выражение.
			/// </summary>
			False = 20,

			/// <summary>
			/// Логическое отрицание [not].
			/// </summary>
			Not = 21,

			/// <summary>
			/// Логическое сложение [and].
			/// </summary>
			And = 22,

			/// <summary>
			/// Логическое ИЛИ [or].
			/// </summary>
			Or = 23,

			/// <summary>
			/// Исключающее ИЛИ [xor].
			/// </summary>
			Xor = 24
		}
	}
}