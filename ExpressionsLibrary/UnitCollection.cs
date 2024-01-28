﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExpressionsLibrary
{
    /// <summary>
    /// Коллекция элементов алгебраического действия.
    /// </summary>
    class UnitCollection : IEnumerable<UnitCollection.BaseUnit>
    {
        List<BaseUnit> List;

        public UnitCollection()
        {
            List = new List<BaseUnit>();
        }

        public static UnitCollection Create(BaseUnit unit)
        {
            UnitCollection result = new UnitCollection();
            result.List.Add(BaseUnit.Create(result, unit));
            return result;
        }

        public static UnitCollection Create(UnitCollection array)
        {
            UnitCollection result = new UnitCollection();
            foreach (BaseUnit u in array)
            {
                result.List.Add(BaseUnit.Create(result, u));
            }
            return result;
        }

        public static UnitCollection Create(UnitCollection array, int start)
        {
            return Create(array, start, array.List.Count - start);
        }

        public static UnitCollection Create(UnitCollection array, int start, int lenght)
        {
            UnitCollection result = new UnitCollection();
            for (int i = 0; i < lenght; i++)
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

        public BaseUnit First
        {
            get { return List.First(); }
        }

        public BaseUnit Last
        {
            get { return List.Last(); }
        }

        public BaseUnit this[int index]
        {
            get { return List[index]; }
        }

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

        public bool IsContainsError()
        {
            bool containsErr = (List.Count(x => x.Action < 0)) > 0;
            return containsErr;
        }
        public bool IsFirstError()
        {
            bool firstErr = List.Count > 0 && First.IsArithmetic || First.IsBoolean || First.IsLogic;
            return firstErr;
        }
        public bool IsLastError()
        {
            bool lastErr = List.Count > 0 && Last.IsArithmetic || Last.IsBoolean || Last.IsLogic;
            return lastErr;
        }

        /// <summary>
        /// Алгебраическое выражение.
        /// </summary>
        public bool IsArithmetic
        {
            get
            {
                return !IsLogic && (List.Count(x => x.Action >= 1 && x.Action <= 6)) > 0;
            }
        }

        /// <summary>
        /// Логическое выражение.
        /// </summary>
        public bool IsLogic
        {
            get
            {
                return !IsBoolean && (List.Count(x => x.Action >= 7 && x.Action <= 12)) > 0;
            }
        }

        /// <summary>
        /// Булевое логическое выражение.
        /// </summary>
        public bool IsBoolean
        {
            get
            {
                return (List.Count(x => x.Action >= 13 && x.Action <= 16)) > 0;
            }
        }

        /// <summary>
        /// Выражение целиком заключено в скобки (X+Y).
        /// </summary>
        public bool IsAssociation
        { // Значение 0 указывает на то, что открывающаяся скобка имеет нулевую позицию, а значит все выражение заключено в скобки.
            get { return OpenIndex() == 0; }
        }

        /// <summary>
        /// Орицательное выражение заключенное в скобки -(X+Y)
        /// </summary>
        public bool IsNegativeAssociation
        {
            get { return OpenIndex() == 1 && First.UnitType == MatchType.Subtracting; }
        }

        /// <summary>
        /// Орицательное выражение заключенное в скобки Not(... )
        /// </summary>
        public bool IsNotAssociation
        {
            get { return OpenIndex() == 1 && First.UnitType == MatchType.Not; }
        }

        /// <summary>
        /// Положительное выражение заключенное в скобки +(X+Y)
        /// </summary>
        public bool IsPositiveAssociation
        {
            get { return OpenIndex() == 1 && First.UnitType == MatchType.Addition; }
        }

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
                return association & (A!=0);
            }          
        }

        public int Count
        {
            get { return List.Count; }
        }

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
            IEnumerable<int> array = from BaseUnit U in List
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
                if (List[i].UnitType == MatchType.Close) { A--; association = true;}
                if (List[i].UnitType == MatchType.Open) { A++; association = true; }
                if (A == 0 && List[i].Action == action) { return i; }
            }
            if (association) { return -2; }
            else { return -1; }
        }

        IEnumerator<BaseUnit> IEnumerable<UnitCollection.BaseUnit>.GetEnumerator()
        {
            return List.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return List.GetEnumerator();
        }

        /// <summary>
        /// Базовый элемент алгебраического действия.
        /// </summary>
        public abstract class BaseUnit
        {
            private UnitCollection parent;
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
            public int Index
            {
                get { return parent.List.IndexOf(this); }
            }

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

            public static BaseUnit Create(UnitCollection parent, BaseUnit unit)
            {
                switch (unit.UnitType)
                {
                    case MatchType.Addition:
                        return new AdditionUnit(parent: parent, value: unit.Value);
                    case MatchType.And:
                        return new AndUnit(parent: parent, value: unit.Value);
                    case MatchType.Cell:
                        return new CellUnit(parent: parent, value: unit.Value);
                    case MatchType.Close:
                        return new CloseUnit(parent: parent, value: unit.Value);
                    case MatchType.Decimal:
                        return new DecimalUnit(parent: parent, value: unit.Value);
                    case MatchType.Division:
                        return new DivisionUnit(parent: parent, value: unit.Value);
                    case MatchType.Equal:
                        return new EqualUnit(parent: parent, value: unit.Value);
                    case MatchType.False:
                        return new FalseUnit(parent: parent, value: unit.Value);
                    case MatchType.Fix:
                        return new FixUnit(parent: parent, value: unit.Value);
                    case MatchType.Less:
                        return new LessUnit(parent: parent, value: unit.Value);
                    case MatchType.LessOrEqual:
                        return new LessOrEqualUnit(parent: parent, value: unit.Value);
                    case MatchType.Mod:
                        return new ModUnit(parent: parent, value: unit.Value);
                    case MatchType.More:
                        return new MoreUnit(parent: parent, value: unit.Value);
                    case MatchType.MoreOrEqual:
                        return new MoreOrEqualUnit(parent: parent, value: unit.Value);
                    case MatchType.Multiplication:
                        return new MultiplicationUnit(parent: parent, value: unit.Value);
                    case MatchType.Negative:
                        return new NegativeUnit(parent: parent, value: unit.Value);
                    case MatchType.Not:
                        return new NotUnit(parent: parent, value: unit.Value);
                    case MatchType.NotEqual:
                        return new NotEqualUnit(parent: parent, value: unit.Value);
                    case MatchType.Open:
                        return new OpenUnit(parent: parent, value: unit.Value);
                    case MatchType.Or:
                        return new OrUnit(parent: parent, value: unit.Value);
                    case MatchType.Power:
                        return new PowerUnit(parent: parent, value: unit.Value);
                    case MatchType.Sqrt:
                        return new SqrtUnit(parent: parent, value: unit.Value);
                    case MatchType.Subtracting:
                        return new SubtractingUnit(parent: parent, value: unit.Value);
                    case MatchType.True:
                        return new TrueUnit(parent: parent, value: unit.Value);
                    case MatchType.Xor:
                        return new XorUnit(parent: parent, value: unit.Value);
                    default:
                        return new DecimalUnit(parent: parent, value: unit.Value);
                }
            }

            public static BaseUnit Create(UnitCollection parent, Match match)
            {
                MatchType t = GetMatchType(match);
                switch (t)
                {
                    case MatchType.Addition:
                        return new AdditionUnit(parent: parent, value: match.Value);
                    case MatchType.And:
                        return new AndUnit(parent: parent, value: match.Value);
                    case MatchType.Cell:
                        return new CellUnit(parent: parent, value: match.Value);
                    case MatchType.Close:
                        return new CloseUnit(parent: parent, value: match.Value);
                    case MatchType.Decimal:
                        return new DecimalUnit(parent: parent, value: match.Value);
                    case MatchType.Division:
                        return new DivisionUnit(parent: parent, value: match.Value);
                    case MatchType.Equal:
                        return new EqualUnit(parent: parent, value: match.Value);
                    case MatchType.False:
                        return new FalseUnit(parent: parent, value: match.Value);
                    case MatchType.Fix:
                        return new FixUnit(parent: parent, value: match.Value);
                    case MatchType.Less:
                        return new LessUnit(parent: parent, value: match.Value);
                    case MatchType.LessOrEqual:
                        return new LessOrEqualUnit(parent: parent, value: match.Value);
                    case MatchType.Mod:
                        return new ModUnit(parent: parent, value: match.Value);
                    case MatchType.More:
                        return new MoreUnit(parent: parent, value: match.Value);
                    case MatchType.MoreOrEqual:
                        return new MoreOrEqualUnit(parent: parent, value: match.Value);
                    case MatchType.Multiplication:
                        return new MultiplicationUnit(parent: parent, value: match.Value);
                    case MatchType.Negative:
                        return new NegativeUnit(parent: parent, value: match.Value);
                    case MatchType.Not:
                        return new NotUnit(parent: parent, value: match.Value);
                    case MatchType.NotEqual:
                        return new NotEqualUnit(parent: parent, value: match.Value);
                    case MatchType.Open:
                        return new OpenUnit(parent: parent, value: match.Value);
                    case MatchType.Or:
                        return new OrUnit(parent: parent, value: match.Value);
                    case MatchType.Power:
                        return new PowerUnit(parent: parent, value: match.Value);
                    case MatchType.Sqrt:
                        return new SqrtUnit(parent: parent, value: match.Value);
                    case MatchType.Subtracting:
                        return new SubtractingUnit(parent: parent, value: match.Value);
                    case MatchType.True:
                        return new TrueUnit(parent: parent, value: match.Value);
                    case MatchType.Xor:
                        return new XorUnit(parent: parent, value: match.Value);
                    default:
                        return new ErrorUnit(parent: parent, value: match.Value);
                }
            }
        }

        /// <summary>
        /// Ошибочный знак.(-1)
        /// </summary>
        class ErrorUnit : BaseUnit
        {
            public ErrorUnit(UnitCollection parent, string value) : 
                base(parent: parent, value: value, 
                    type: MatchType.Error, action:-1, arithmetic: false, boolean:false, logic: false)
            { }
        }
        /// <summary>
        /// Ссылка на ячейку.(0)
        /// </summary>
        class CellUnit : BaseUnit
        {
            public CellUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    type: MatchType.Cell, action: 0, arithmetic: false, boolean: false, logic: false)
            { }
        }
        /// <summary>
        /// Числовой показатель.(0)
        /// </summary>
        class DecimalUnit : BaseUnit
        {
            public DecimalUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    type: MatchType.Decimal, action: 0, arithmetic: false, boolean: false, logic: false)
            { }
        }
        /// <summary>
        /// Закрывающаяся скобка.(0)
        /// </summary>
        class CloseUnit : BaseUnit
        {
            public CloseUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    type: MatchType.Close, action: 0, arithmetic: false, boolean: false, logic: false)
            { }
        }
        /// <summary>
        /// Открывающаяся скобка.(0)
        /// </summary>
        class OpenUnit : BaseUnit
        {
            public OpenUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    type: MatchType.Open, action: 0, arithmetic: false, boolean: false, logic: false)
            { }
        }
        /// <summary>
        /// Знак степени числа.(1)
        /// </summary>
        class PowerUnit : BaseUnit
        {
            public PowerUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    type: MatchType.Power, action: 1, arithmetic: true, boolean: false, logic: false)
            { }
        }
        /// <summary>
        /// Знак корня числа.(1)
        /// </summary>
        class SqrtUnit : BaseUnit
        {
            public SqrtUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    type: MatchType.Sqrt, action: 1, arithmetic: true, boolean: false, logic: false)
            { }
        }
        /// <summary>
        /// Знак отрицательного числа.(2)
        /// </summary>
        class NegativeUnit : BaseUnit
        {
            public NegativeUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    type: MatchType.Negative, action: 2, arithmetic: false, boolean: false, logic: false)
            { }
        }
        /// <summary>
        /// Знак умножения.(3)
        /// </summary>
        class MultiplicationUnit : BaseUnit
        {
            public MultiplicationUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    type: MatchType.Multiplication, action: 3, arithmetic: true, boolean: false, logic: false)
            { }
        }
        /// <summary>
        /// Знак деления.(3)
        /// </summary>
        class DivisionUnit : BaseUnit
        {
            public DivisionUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    type: MatchType.Division, action: 3, arithmetic: true, boolean: false, logic: false)
            { }
        }
        /// <summary>
        /// Вычисление целой части от деления.(4)
        /// </summary>
        class FixUnit : BaseUnit
        {
            public FixUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    type: MatchType.Fix, action: 4, arithmetic: true, boolean: false, logic: false)
            { }
        }
        /// <summary>
        /// Вычисление остатка от деления.(5)
        /// </summary>
        class ModUnit : BaseUnit
        {
            public ModUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    type: MatchType.Mod, action: 5, arithmetic: true, boolean: false, logic: false)
            { }
        }
        /// <summary>
        /// Знак сложения.(6)
        /// </summary>
        class AdditionUnit : BaseUnit
        {
            public AdditionUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    type: MatchType.Addition, action: 6, arithmetic: true, boolean: false, logic: false)
            { }
        }
        /// <summary>
        /// Знак вычитания.(6)
        /// </summary>
        class SubtractingUnit : BaseUnit
        {
            public SubtractingUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    type: MatchType.Subtracting, action: 6, arithmetic: true, boolean: false, logic: false)
            { }
        }
        /// <summary>
        /// Знак равно.(7)
        /// </summary>
        class EqualUnit : BaseUnit
        {
            public EqualUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    type: MatchType.Equal, action: 7, arithmetic: false, boolean: false, logic: true)
            { }
        }
        /// <summary>
        /// Знак не равно (!=).(8)
        /// </summary>
        class NotEqualUnit : BaseUnit
        {
            public NotEqualUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    type: MatchType.NotEqual, action: 8, arithmetic: false, boolean: false, logic: true)
            { }
        }
        /// <summary>
        /// Знак меньше (&lt;).(9)
        /// </summary>
        class LessUnit : BaseUnit
        {
            public LessUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    type: MatchType.Less, action: 9, arithmetic: false, boolean: false, logic: true)
            { }
        }
        /// <summary>
        /// Знак больше (>).(10)
        /// </summary>
        class MoreUnit : BaseUnit
        {
            public MoreUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    type: MatchType.More, action: 10, arithmetic: false, boolean: false, logic: true)
            { }
        }
        /// <summary>
        /// Знак меньше или равно (&lt;=).(11)
        /// </summary>
        class LessOrEqualUnit : BaseUnit
        {
            public LessOrEqualUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    type: MatchType.LessOrEqual, action: 11, arithmetic: false, boolean: false, logic: true)
            { }
        }
        /// <summary>
        /// Знак больше или равно (>=).(12)
        /// </summary>
        class MoreOrEqualUnit : BaseUnit
        {
            public MoreOrEqualUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    type: MatchType.MoreOrEqual, action: 12, arithmetic: false, boolean: false, logic: true)
            { }
        }
        /// <summary>
        /// Знак положительного логического выражения (True).(0)
        /// </summary>
        class TrueUnit : BaseUnit
        {
            public TrueUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    type: MatchType.True, action: 0, arithmetic: false, boolean: false, logic: false)
            { }
        }
        /// <summary>
        /// Знак отрицательного логического выражения (False).(0)
        /// </summary>
        class FalseUnit : BaseUnit
        {
            public FalseUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    type: MatchType.False, action: 0, arithmetic: false, boolean: false, logic: false)
            { }
        }
        /// <summary>
        /// Знак логического отрицания (NOT).(13)
        /// </summary>
        class NotUnit : BaseUnit
        {
            public NotUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    type: MatchType.Not, action: 13, arithmetic: false, boolean: false, logic: false)
            { }
        }
        /// <summary>
        /// Знак логического сложения (AND).(14)
        /// </summary>
        class AndUnit : BaseUnit
        {
            public AndUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    type: MatchType.And, action: 14, arithmetic: false, boolean: true, logic: false)
            { }
        }
        /// <summary>
        /// Знак логического ИЛИ (OR).(15)
        /// </summary>
        class OrUnit : BaseUnit
        {
            public OrUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    type: MatchType.Or, action: 15, arithmetic: false, boolean: true, logic: false)
            { }
        }
        /// <summary>
        /// Знак исключающегося ИЛИ (XOR).(16)
        /// </summary>
        class XorUnit : BaseUnit
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
            if (ArithmeticExpression.regexCell != null && ArithmeticExpression.regexCell.IsMatch(match.Value))
            {
                return MatchType.Cell;
            }
            else if (BooleanExpression.regexTrue.IsMatch(match.Value))
            {
                return MatchType.True;
            }
            else if (BooleanExpression.regexFalse.IsMatch(match.Value))
            {
                return MatchType.False;
            }
            else if (BooleanExpression.regexAnd.IsMatch(match.Value))
            {
                return MatchType.And;
            }
            else if (BooleanExpression.regexXor.IsMatch(match.Value))
            {
                return MatchType.Xor;
            }
            else if (BooleanExpression.regexOr.IsMatch(match.Value))
            {
                return MatchType.Or;
            }
            else if (LogicExpression.regexLessOrEqual.IsMatch(match.Value))
            {
                return MatchType.LessOrEqual;
            }
            else if (LogicExpression.regexMoreOrEqual.IsMatch(match.Value))
            {
                return MatchType.MoreOrEqual;
            }
            else if (LogicExpression.regexNotEqual.IsMatch(match.Value))
            {
                return MatchType.NotEqual;
            }
            else if (LogicExpression.regexEqual.IsMatch(match.Value))
            {
                return MatchType.Equal;
            }
            else if (LogicExpression.regexLess.IsMatch(match.Value))
            {
                return MatchType.Less;
            }
            else if (LogicExpression.regexMore.IsMatch(match.Value))
            {
                return MatchType.More;
            }
            else if (BooleanExpression.regexNot.IsMatch(match.Value))
            {
                return MatchType.Not;
            }
            else if (ArithmeticExpression.regexDecimal.IsMatch(match.Value))
            {
                return MatchType.Decimal;
            }
            else if (ArithmeticExpression.regexAddition.IsMatch(match.Value))
            {
                return MatchType.Addition;
            }
            else if (ArithmeticExpression.regexSubtracting.IsMatch(match.Value))
            {
                return MatchType.Subtracting;
            }
            else if (ArithmeticExpression.regexMultiplication.IsMatch(match.Value))
            {
                return MatchType.Multiplication;
            }
            else if (ArithmeticExpression.regexDivision.IsMatch(match.Value))
            {
                return MatchType.Division;
            }
            else if (ArithmeticExpression.regexFix.IsMatch(match.Value))
            {
                return MatchType.Fix;
            }
            else if (ArithmeticExpression.regexMod.IsMatch(match.Value))
            {
                return MatchType.Mod;
            }
            else if (ArithmeticExpression.regexOpen.IsMatch(match.Value))
            {
                return MatchType.Open;
            }
            else if (ArithmeticExpression.regexClose.IsMatch(match.Value))
            {
                return MatchType.Close;
            }
            else if (ArithmeticExpression.regexPower.IsMatch(match.Value))
            {
                return MatchType.Power;
            }
            else if (ArithmeticExpression.regexSqrt.IsMatch(match.Value))
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