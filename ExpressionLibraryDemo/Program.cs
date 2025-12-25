using System;

namespace ExpressionLibraryDemo
{
	class Program
	{
		static void Main(string[] args)
		{
			Random rnd = new Random(0);
			ExpressionsLibrary.IExpression expression;

			for (int i = 0; i < 100; i++)
			{
				decimal x = (decimal)(rnd.NextDouble() * 100);
				decimal y = (decimal)(rnd.NextDouble() * 100);

				expression = ExpressionsLibrary.Expression.Create(x.ToString() + @"<>" + y.ToString());
				Console.WriteLine(expression.Formula() + " " + expression.ObjValue);

				expression = ExpressionsLibrary.Expression.Create(x.ToString() + @"=" + y.ToString());
				Console.WriteLine(expression.Formula() + " " + expression.ObjValue);

				expression = ExpressionsLibrary.Expression.Create(x.ToString() + @"+" + y.ToString());
				Console.WriteLine(expression.Formula() + " " + expression.ObjValue);
			}

			expression = ExpressionsLibrary.Expression.Create(@"2/0");
			Console.WriteLine(expression.Formula() + " = " + expression.ObjValue);

			Console.ReadKey();
		}
	}
}
