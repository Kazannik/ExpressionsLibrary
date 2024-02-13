using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionLibraryDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random(0);


            for (int i = 0; i < 100; i++)
            {
                decimal x = (decimal)(rnd.NextDouble() * 100);
                decimal y = (decimal)(rnd.NextDouble() * 100);

                ExpressionsLibrary.IExpression expression = ExpressionsLibrary.Expression.Create(x.ToString() + @"<>" + y.ToString());
              Console.WriteLine(expression.Formula());
            }

        }
    }
}
