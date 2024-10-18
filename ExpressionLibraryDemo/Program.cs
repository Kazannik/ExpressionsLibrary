﻿using System;

namespace ExpressionLibraryDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random(0);
            ExpressionsLibrary.IExpression expression;

            for (int i = 0; i < 1; i++)
            {
                decimal x = (decimal)(rnd.NextDouble() * 100);
                decimal y = (decimal)(rnd.NextDouble() * 100);

                expression = ExpressionsLibrary.Expression.Create(x.ToString() + @"<>" + y.ToString());
                Console.WriteLine(expression.Formula("00.000") + " = " + expression.objValue);
            }

            expression = ExpressionsLibrary.Expression.Create(@"!(2=0)");
            Console.WriteLine(expression.Formula() + " = " + expression.objValue);


            expression = ExpressionsLibrary.Expression.Create(@"IF(2<>0)THEN(1)");
            Console.WriteLine(expression.Formula() + " = " + expression.objValue);


            Console.ReadKey();
        }
    }
}
