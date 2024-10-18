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
        private List<IUnit> List;

        private UnitCollection()
        {
            List = new List<IUnit>();
        }

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

        public static UnitCollection Create(string text)
        {
            string context = text.Replace(" ", "");
            Regex regexAll = new Regex(
                SelectExpression.SELECT + @"|" +
                BooleanExpression.BOOLEAN + @"|" +
                СomparisonExpression.COMPARISON + @"|" +
                ArithmeticExpression.ARITHMETIC + @"|" +
                ArithmeticExpression.OPEN + @"|" +
                ArithmeticExpression.CLOSE, ArithmeticExpression.OPTIONS);
            return From(regexAll.Matches(text));
        }
        
        public static UnitCollection Create(string text, string cellpattern)
        {
            string context = text.Replace(" ", "");
            Regex regexAll = new Regex(
                @"(" + cellpattern + @")|" +
                SelectExpression.SELECT + @"|" +
                BooleanExpression.BOOLEAN + @"|" + 
                СomparisonExpression.COMPARISON + @"|" + 
                ArithmeticExpression.ARITHMETIC + @"|" + 
                ArithmeticExpression.OPEN + @"|" + 
                ArithmeticExpression.CLOSE, ArithmeticExpression.OPTIONS);
            return From(regexAll.Matches(text));
        }

        private static UnitCollection From(MatchCollection collection)
        {
            UnitCollection result = new UnitCollection();
            foreach (Match m in collection)
            {
                result.List.Add(BaseUnit.Create(result, m));
            }
            return result;
        }

        public IUnit First
        {
            get { return List.First(); }
        }

        public IUnit Second
        {
            get { return List.Count>1 ? List[1]: null; }
        }

        public IUnit Last
        {
            get { return List.Last(); }
        }

        public IUnit this[int index]
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
                if (first)
                {
                    return true;
                }
                bool last = IsLastError();
                if (last)
                {
                    return true;
                }
                return false;
            }
        }

        public bool IsContainsError()
        {
            bool containsErr = (List.Count(x => x.PriorityAction < 0)) > 0;
            return containsErr;
        }
        public bool IsFirstError()
        {
            bool firstErr = List.Count > 0 && First.Action != ActionType.Non;
            return firstErr;
        }
        public bool IsLastError()
        {
            bool lastErr = List.Count > 0 && Last.Action != ActionType.Non;
            return lastErr;
        }

        /// <summary>
        /// Алгебраическое выражение.
        /// </summary>
        public bool IsArithmetic
        {
            get
            {
                return !IsSelect && !IsLogic && !IsBoolean && (List.Count(x => x.PriorityAction >= 1 && x.PriorityAction <= 6)) > 0;
            }
        }

        /// <summary>
        /// Логическое выражение.
        /// </summary>
        public bool IsLogic
        {
            get
            {
                return !IsSelect && !IsBoolean && (List.Count(x => x.PriorityAction >= 7 && x.PriorityAction <= 12)) > 0;
            }
        }

        /// <summary>
        /// Булевое логическое выражение.
        /// </summary>
        public bool IsBoolean
        {
            get
            {
                return !IsSelect && (List.Count(x => x.PriorityAction >= 13 && x.PriorityAction <= 16)) > 0;
            }
        }

        /// <summary>
        /// Выражение ветвления.
        /// </summary>
        public bool IsSelect
        {
            get
            {
                return IsIfAssociation && (List.Count(x => x.PriorityAction >= 17 && x.PriorityAction <= 19)) > 0;
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
        /// Условие ветвления, заключенное в скобки IF(... )
        /// </summary>
        public bool IsIfAssociation
        {
            get { return First.UnitType == MatchType.If && Second !=null && Second.UnitType == MatchType.Open; }
        }

        /// <summary>
        /// Результат ветвления, заключенный в скобки THEN(... )
        /// </summary>
        public bool IsThenAssociation
        {
            get { return OpenIndex() == 1 && First.UnitType == MatchType.Then; }
        }

        /// <summary>
        /// Результат ветвления, заключенный в скобки ELSE(... )
        /// </summary>
        public bool IsElseAssociation
        {
            get { return OpenIndex() == 1 && First.UnitType == MatchType.Else; }
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
                return association & (A != 0);
            }
        }

        public int Count
        {
            get { return List.Count; }
        }


        private int OpenIndex()
        {
            return OpenIndex(lastIndex: List.Count);            
        }

        private int OpenIndex(int lastIndex)
        {
            if (Last.UnitType == MatchType.Close)
            {
                int A = 0;
                for (int i = lastIndex - 1; i >= 0; i--)
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
                                     where U.PriorityAction > 0
                                     orderby U.PriorityAction descending
                                     group U by U.PriorityAction into G
                                     select G.First().PriorityAction;
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
                if (A == 0 && List[i].PriorityAction == action) { return i; }
            }
            if (association) { return -2; }
            else { return -1; }
        }

        IEnumerator<IUnit> IEnumerable<IUnit>.GetEnumerator()
        {
            return List.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return List.GetEnumerator();
        }

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
            int PriorityAction { get; }
            /// <summary>
            /// Тип действия.
            /// </summary>
            ActionType Action { get; }
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
            private UnitCollection parent;
            /// <summary>
            /// Тип элемента.
            /// </summary>
            public MatchType UnitType { get; }
            /// <summary>
            /// Приоритет математического действия.
            /// </summary>
            public int PriorityAction { get; }
            /// <summary>
            /// Тип действия.
            /// </summary>
            public ActionType Action { get; }
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

            /// <summary>
            /// Конструктор базового элемента алгебраического действия
            /// </summary>
            /// <param name="parent">Родительская коллекция элементов.</param>
            /// <param name="value">Строковое значение элемента.</param>
            /// <param name="matchType">Тип элемента.</param>
            /// <param name="priorityAction">Приоритет математического действия.</param>
            /// <param name="action">Тип операции.</param>
            protected BaseUnit(UnitCollection parent, string value, MatchType matchType, int priorityAction, ActionType action)
            {
                this.parent = parent;
                Value = value;
                UnitType = matchType;
                PriorityAction = priorityAction;
                Action = action;
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
                    case MatchType.If:
                        return new IfUnit(parent: parent, value: value);
                    case MatchType.Then:
                        return new ThenUnit(parent: parent, value: value);
                    case MatchType.Else:
                        return new ElseUnit(parent: parent, value: value);
                    default:
                        return new DecimalUnit(parent: parent, value: value);
                }
            }

            public static BaseUnit Create(UnitCollection parent, Match match)
            {
                string value = match.Value;
                MatchType type = GetMatchType(value);
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
                    case MatchType.If:
                        return new IfUnit(parent: parent, value: value);
                    case MatchType.Then:
                        return new ThenUnit(parent: parent, value: value);
                    case MatchType.Else:
                        return new ElseUnit(parent: parent, value: value);
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
            public const int ACTION = -1;

            public ErrorUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    matchType: MatchType.Error, priorityAction: ACTION, 
                    action: ActionType.Non)
            { }
        }
        
        /// <summary>
        /// Ссылка на ячейку.(0)
        /// </summary>
        private class CellUnit : BaseUnit, IUnit
        {
            public const int ACTION = 0;

            public CellUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    matchType: MatchType.Cell, 
                    priorityAction: ACTION,
                    action: ActionType.Non)
            { }
        }
        
        /// <summary>
        /// Числовой показатель.(0)
        /// </summary>
        private class DecimalUnit : BaseUnit, IUnit
        {
            public const int ACTION = 0;

            public DecimalUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    matchType: MatchType.Decimal, 
                    priorityAction: ACTION,
                    action: ActionType.Non)
            { }
        }
        
        /// <summary>
        /// Закрывающаяся скобка.(0)
        /// </summary>
        private class CloseUnit : BaseUnit, IUnit
        {
            public const int ACTION = 0;

            public CloseUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    matchType: MatchType.Close, 
                    priorityAction: ACTION,
                    action: ActionType.Non)
            { }
        }
        
        /// <summary>
        /// Открывающаяся скобка.(0)
        /// </summary>
        private class OpenUnit : BaseUnit, IUnit
        {
            public const int ACTION = 0;

            public OpenUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    matchType: MatchType.Open, 
                    priorityAction: ACTION,
                    action: ActionType.Non)
            { }
        }
        
        /// <summary>
        /// Знак степени числа.(1)
        /// </summary>
        private class PowerUnit : BaseUnit, IUnit
        {
            public const int ACTION = 1;

            public PowerUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    matchType: MatchType.Power, 
                    priorityAction: ACTION,
                    action: ActionType.Arithmetic)
            { }
        }
        
        /// <summary>
        /// Знак корня числа.(1)
        /// </summary>
        private class SqrtUnit : BaseUnit, IUnit
        {
            public const int ACTION = 1;

            public SqrtUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    matchType: MatchType.Sqrt, 
                    priorityAction: ACTION,
                    action: ActionType.Arithmetic)
            { }
        }
        
        /// <summary>
        /// Знак отрицательного числа.(2)
        /// </summary>
        private class NegativeUnit : BaseUnit, IUnit
        {
            public const int ACTION = 2;

            public NegativeUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    matchType: MatchType.Negative, 
                    priorityAction: ACTION,
                    action: ActionType.Non)
            { }
        }
        
        /// <summary>
        /// Знак умножения.(3)
        /// </summary>
        private class MultiplicationUnit : BaseUnit, IUnit
        {
            public const int ACTION = 3;

            public MultiplicationUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    matchType: MatchType.Multiplication,
                    priorityAction: ACTION,
                    action: ActionType.Arithmetic)
            { }
        }
        
        /// <summary>
        /// Знак деления.(3)
        /// </summary>
        private class DivisionUnit : BaseUnit, IUnit
        {
            public const int ACTION = 3;

            public DivisionUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    matchType: MatchType.Division, 
                    priorityAction: ACTION,
                    action: ActionType.Arithmetic)
            { }
        }
        
        /// <summary>
        /// Вычисление целой части от деления.(4)
        /// </summary>
        private class FixUnit : BaseUnit, IUnit
        {
            public const int ACTION = 4;

            public FixUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    matchType: MatchType.Fix, 
                    priorityAction: ACTION,
                    action: ActionType.Arithmetic)
            { }
        }
        
        /// <summary>
        /// Вычисление остатка от деления.(5)
        /// </summary>
        private class ModUnit : BaseUnit, IUnit
        {
            public const int ACTION = 5;

            public ModUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    matchType: MatchType.Mod, 
                    priorityAction: ACTION,
                    action: ActionType.Arithmetic)
            { }
        }
        
        /// <summary>
        /// Знак сложения.(6)
        /// </summary>
        private class AdditionUnit : BaseUnit, IUnit
        {
            public const int ACTION = 6;
            public AdditionUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    matchType: MatchType.Addition, 
                    priorityAction: ACTION,
                    action: ActionType.Arithmetic)
            { }
        }
        
        /// <summary>
        /// Знак вычитания.(6)
        /// </summary>
        private class SubtractingUnit : BaseUnit, IUnit
        {
            public const int ACTION = 6;

            public SubtractingUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    matchType: MatchType.Subtracting, 
                    priorityAction: ACTION,
                    action: ActionType.Arithmetic)
            { }
        }
        
        /// <summary>
        /// Знак равно.(7)
        /// </summary>
        private class EqualUnit : BaseUnit, IUnit
        {
            public const int ACTION = 7;

            public EqualUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    matchType: MatchType.Equal, 
                    priorityAction: ACTION,
                    action: ActionType.Сomparison)
            { }
        }
        
        /// <summary>
        /// Знак не равно (!=).(8)
        /// </summary>
        private class NotEqualUnit : BaseUnit, IUnit
        {
            public const int ACTION = 8;

            public NotEqualUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    matchType: MatchType.NotEqual, 
                    priorityAction: ACTION,
                    action: ActionType.Сomparison)
            { }
        }
        
        /// <summary>
        /// Знак меньше (&lt;).(9)
        /// </summary>
        private class LessUnit : BaseUnit, IUnit
        {
            public const int ACTION = 9;

            public LessUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    matchType: MatchType.Less, 
                    priorityAction: ACTION,
                    action: ActionType.Сomparison)
            { }
        }
        
        /// <summary>
        /// Знак больше (>).(10)
        /// </summary>
        private class MoreUnit : BaseUnit, IUnit
        {
            public const int ACTION = 10;

            public MoreUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    matchType: MatchType.More, 
                    priorityAction: ACTION,
                    action: ActionType.Сomparison)
            { }
        }
        
        /// <summary>
        /// Знак меньше или равно (&lt;=).(11)
        /// </summary>
        private class LessOrEqualUnit : BaseUnit, IUnit
        {
            public const int ACTION = 11;

            public LessOrEqualUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    matchType: MatchType.LessOrEqual, 
                    priorityAction: ACTION,
                    action: ActionType.Сomparison)
            { }
        }
        
        /// <summary>
        /// Знак больше или равно (>=).(12)
        /// </summary>
        private class MoreOrEqualUnit : BaseUnit, IUnit
        {
            public const int ACTION = 12;

            public MoreOrEqualUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    matchType: MatchType.MoreOrEqual, 
                    priorityAction: ACTION,
                    action: ActionType.Сomparison)
            { }
        }
        
        /// <summary>
        /// Знак положительного логического выражения (True).(0)
        /// </summary>
        private class TrueUnit : BaseUnit, IUnit
        {
            public const int ACTION = 0;

            public TrueUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    matchType: MatchType.True, 
                    priorityAction: ACTION,
                    action: ActionType.Non)
            { }
        }
        
        /// <summary>
        /// Знак отрицательного логического выражения (False).(0)
        /// </summary>
        private class FalseUnit : BaseUnit, IUnit
        {
            public const int ACTION = 0;

            public FalseUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    matchType: MatchType.False, 
                    priorityAction: ACTION,
                    action: ActionType.Non)
            { }
        }
        
        /// <summary>
        /// Знак логического отрицания (NOT).(13)
        /// </summary>
        private class NotUnit : BaseUnit, IUnit
        {
            public const int ACTION = 13;

            public NotUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    matchType: MatchType.Not, 
                    priorityAction: ACTION,
                    action: ActionType.Boolean)
            { }
        }
        
        /// <summary>
        /// Знак логического сложения (AND).(14)
        /// </summary>
        private class AndUnit : BaseUnit, IUnit
        {
            public const int ACTION = 14;

            public AndUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    matchType: MatchType.And, 
                    priorityAction: ACTION,
                    action: ActionType.Boolean)
            { }
        }
        
        /// <summary>
        /// Знак логического ИЛИ (OR).(15)
        /// </summary>
        private class OrUnit : BaseUnit, IUnit
        {
            public const int ACTION = 15;

            public OrUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    matchType: MatchType.Or, 
                    priorityAction: ACTION,
                    action: ActionType.Boolean)
            { }
        }
        
        /// <summary>
        /// Знак исключающегося ИЛИ (XOR).(16)
        /// </summary>
        private class XorUnit : BaseUnit, IUnit
        {
            public const int ACTION = 16;

            public XorUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    matchType: MatchType.Xor, 
                    priorityAction: ACTION,
                    action: ActionType.Boolean)
            { }
        }
        
        /// <summary>
        /// Знак ветвления ЕСЛИ (IF).(17)
        /// </summary>
        private class IfUnit : BaseUnit, IUnit
        {
            public const int ACTION = 17;

            public IfUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    matchType: MatchType.If, 
                    priorityAction: ACTION,
                    action: ActionType.Select)
            { }
        }

        /// <summary>
        /// Знак ветвления ТОГДА (THEN).(18)
        /// </summary>
        private class ThenUnit : BaseUnit, IUnit
        {
            public const int ACTION = 18;

            public ThenUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    matchType: MatchType.Then, 
                    priorityAction: ACTION,
                    action: ActionType.Select)
            { }
        }

        /// <summary>
        /// Знак ветвления ИНАЧЕ (ELSE).(19)
        /// </summary>
        private class ElseUnit : BaseUnit, IUnit
        {
            public const int ACTION = 19;

            public ElseUnit(UnitCollection parent, string value) :
                base(parent: parent, value: value,
                    matchType: MatchType.Else, 
                    priorityAction: ACTION,
                    action: ActionType.Select)
            { }
        }
 
        /// <summary>
        /// Определить тип элемента.
        /// </summary>
        /// <param name="match">Класс совпадения регулярного выражения.</param>
        private static MatchType GetMatchType(string value)
        {
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
            else if (СomparisonExpression.regexLessOrEqual.IsMatch(value))
            {
                return MatchType.LessOrEqual;
            }
            else if (СomparisonExpression.regexMoreOrEqual.IsMatch(value))
            {
                return MatchType.MoreOrEqual;
            }
            else if (СomparisonExpression.regexNotEqual.IsMatch(value))
            {
                return MatchType.NotEqual;
            }
            else if (СomparisonExpression.regexEqual.IsMatch(value))
            {
                return MatchType.Equal;
            }
            else if (СomparisonExpression.regexLess.IsMatch(value))
            {
                return MatchType.Less;
            }
            else if (СomparisonExpression.regexMore.IsMatch(value))
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
            else if (SelectExpression.regexIf.IsMatch(value))
            {
                return MatchType.If;
            }
            else if (SelectExpression.regexThen.IsMatch(value))
            {
                return MatchType.Then;
            }
            else if (SelectExpression.regexElse.IsMatch(value))
            {
                return MatchType.Else;
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
            Xor = 24,
            /// <summary>
            /// Условие ЕСЛИ [IF]
            /// </summary>
            If = 25,
            /// <summary>
            /// Условие ТОГДА [THEN]
            /// </summary>
            Then = 26,
            /// <summary>
            /// Условие ИНАЧЕ [ELSE]
            /// </summary>
            Else = 27
        }

        /// <summary>
        /// Тип операции.
        /// </summary>
        public enum ActionType: int
        {
            /// <summary>
            /// Иной элемент.
            /// </summary>
            Non = 0,
            /// <summary>
            /// Арифметика.
            /// </summary>
            Arithmetic = 1,
            /// <summary>
            /// Сравнение.
            /// </summary>
            Сomparison = 2,
            /// <summary>
            /// Булевая логика.
            /// </summary>
            Boolean = 3,
            /// <summary>
            /// Ветвление.
            /// </summary>
            Select = 4
        }
    }
}