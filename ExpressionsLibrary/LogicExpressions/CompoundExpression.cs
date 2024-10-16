using System.Collections.Generic;

namespace ExpressionsLibrary.LogicExpressions
{
    /// <summary>
    /// Составное логическое выражение (Например, равенство, сравнение и т.д.)
    /// </summary>
    abstract class CompoundExpression : ExpressionBase, ILogicExpression
    {
        protected CompoundExpression(ref Dictionary<string, ArithmeticExpressions.ICell> cells, UnitCollection left, UnitCollection right) : base()
        {
            LeftExpression = ArithmeticExpressions.Expression.Create(ref cells, left);
            RightExpression = ArithmeticExpressions.Expression.Create(ref cells, right);
        }

        protected string GetLeftFormula()
        {
            if (LeftExpression.IsError)
                return LeftExpression.ToString();
            else
                return LeftExpression.Value.ToString();
        }

        protected string GetLeftFormula(string format)
        {
            if (LeftExpression.IsError)
                return LeftExpression.ToString(format: format);
            else
                return LeftExpression.Value.ToString(format: format);
        }

        protected string GetRightFormula(string format)
        {
            if (RightExpression.IsError)
                return RightExpression.Formula();
            else
                return RightExpression.Value.ToString(format: format);
        }

        protected string GetRightFormula()
        {
            if (RightExpression.IsError)
                return RightExpression.Formula();
            else
                return RightExpression.Value.ToString();
        }

        /// <summary>
        /// Признак содержания ошибки в выражении.
        /// </summary>
        public override bool IsError
        {
            get { return LeftExpression.IsError || RightExpression.IsError; }
        }

        /// <summary>
        /// Левая часть алгебраического выражения.
        /// </summary>
        public ArithmeticExpressions.IExpression LeftExpression { get; }

        /// <summary>
        /// Правая часть алгебраического выражения.
        /// </summary>
        public ArithmeticExpressions.IExpression RightExpression { get; }
        public static ILogicExpression Create(ref Dictionary<string, ArithmeticExpressions.ICell> cells, UnitCollection array)
        {
            int i = array.GetLastIndex();
            if (i > 0)
            {
                switch (array[i].UnitType)
                {
                    case UnitCollection.MatchType.Equal:
                        return EqualExpression.Create(ref cells, UnitCollection.Create(array, 0, i), UnitCollection.Create(array, i + 1));
                    case UnitCollection.MatchType.Less:
                        return LessExpression.Create(ref cells, UnitCollection.Create(array, 0, i), UnitCollection.Create(array, i + 1));
                    case UnitCollection.MatchType.LessOrEqual:
                        return LessOrEqualExpression.Create(ref cells, UnitCollection.Create(array, 0, i), UnitCollection.Create(array, i + 1));
                    case UnitCollection.MatchType.More:
                        return MoreExpression.Create(ref cells, UnitCollection.Create(array, 0, i), UnitCollection.Create(array, i + 1));
                    case UnitCollection.MatchType.MoreOrEqual:
                        return MoreOrEqualExpression.Create(ref cells, UnitCollection.Create(array, 0, i), UnitCollection.Create(array, i + 1));
                    case UnitCollection.MatchType.NotEqual:
                        return NotEqualExpression.Create(ref cells, UnitCollection.Create(array, 0, i), UnitCollection.Create(array, i + 1));
                    default:
                        return ErrorExpression.Create(array);
                }
            }
            else if (i == -2)
            {
                return ErrorExpression.Create(array);
            }
            else
            {
                return Expression.Create(ref cells, array);
            }
        }
    }
}
