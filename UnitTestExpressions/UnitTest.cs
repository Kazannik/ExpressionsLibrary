using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestExpressions
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Random rnd = new Random(0);
            for (int i = 0; i < 100; i++)
            {
                decimal x = (decimal)(rnd.NextDouble() * 100) ;
                decimal y = (decimal)(rnd.NextDouble() * 100);

                ExpressionsLibrary.IExpression expression = ExpressionsLibrary.Expression.Create( x.ToString() + '+' + y.ToString());
                Assert.AreEqual(expression.objValue, x + y, "Значение должно быть равно " + (x+y).ToString());
            }

            for (int i = 0; i < 100; i++)
            {
                decimal x = (decimal)(rnd.NextDouble() * 100);
                decimal y = (decimal)(rnd.NextDouble() * 100);

                ExpressionsLibrary.IExpression expression = ExpressionsLibrary.Expression.Create(x.ToString() + '-' + y.ToString());
                Assert.AreEqual(expression.objValue, x - y, "Значение должно быть равно " + (x - y).ToString());
            }

            for (int i = 0; i < 100; i++)
            {
                decimal x = (decimal)(rnd.NextDouble() * 100);
                decimal y = (decimal)(rnd.NextDouble() * 100);

                ExpressionsLibrary.IExpression expression = ExpressionsLibrary.Expression.Create(x.ToString() + '*' + y.ToString());
                Assert.AreEqual(expression.objValue, x * y, "Значение должно быть равно " + (x * y).ToString());
            }

            for (int i = 0; i < 100; i++)
            {
                decimal x = (decimal)(rnd.NextDouble() * 100);
                decimal y = (decimal)(rnd.NextDouble() * 100) + 1;

                ExpressionsLibrary.IExpression expression = ExpressionsLibrary.Expression.Create(x.ToString() + '/' + y.ToString());
                Assert.AreEqual(expression.objValue, x / y, "Значение должно быть равно " + (x / y).ToString());
            }


            for (int i = 0; i < 100; i++)
            {
                decimal d1 = (decimal)(rnd.NextDouble() * 100);
                decimal d2 = (decimal)(rnd.NextDouble() * 100);
                decimal d3 = (decimal)(rnd.NextDouble() * 100) + 1;
                decimal d4 = (decimal)(rnd.NextDouble() * 100);
                decimal d5 = (decimal)(rnd.NextDouble() * 100);
                decimal d6 = (decimal)(rnd.NextDouble() * 100);

                ExpressionsLibrary.IExpression expression = ExpressionsLibrary.Expression.Create("(" + d1 + " + " + d2 + ") /" + d3 + "+" + d4 + "*" + d5 + "-" + d6);
                Assert.AreEqual(expression.objValue, (d1+ d2)/d3 + d4 * d5 - d6, "Значение должно быть равно " + ((d1 + d2) / d3 + d4 * d5 - d6).ToString());
            }

        }

        [TestMethod]
        public void TestMethod2()
        {
            Random rnd = new Random(0);
            for (int i = 0; i < 100; i++)
            {
                decimal x = (decimal)(rnd.NextDouble() * 100);
                decimal y = (decimal)(rnd.NextDouble() * 100);

                ExpressionsLibrary.IExpression expression = ExpressionsLibrary.Expression.Create(x.ToString() + '=' + y.ToString());
                Assert.AreEqual(expression.objValue, x == y, "Значение должно быть " + (x == y).ToString());
            }


            for (int i = 0; i < 100; i++)
            {
                decimal x = (decimal)(rnd.NextDouble() * 100);
                decimal y = (decimal)(rnd.NextDouble() * 100);

                ExpressionsLibrary.IExpression expression = ExpressionsLibrary.Expression.Create(x.ToString() + @"<>" + y.ToString());
                Assert.AreEqual(expression.objValue, x != y, "Значение должно быть " + (x != y).ToString());
            }


            for (int i = 0; i < 100; i++)
            {
                decimal x = (decimal)(rnd.NextDouble() * 100);
                decimal y = (decimal)(rnd.NextDouble() * 100);

                ExpressionsLibrary.IExpression expression = ExpressionsLibrary.Expression.Create(x.ToString() + @"!=" + y.ToString());
                Assert.AreEqual(expression.objValue, x != y, "Значение должно быть " + (x != y).ToString());
            }

            for (int i = 0; i < 100; i++)
            {
                decimal x = (decimal)(rnd.NextDouble() * 100);
                decimal y = (decimal)(rnd.NextDouble() * 100);

                ExpressionsLibrary.IExpression expression = ExpressionsLibrary.Expression.Create(x.ToString() + @">" + y.ToString());
                Assert.AreEqual(expression.objValue, x > y, "Значение должно быть " + (x > y).ToString());
            }

            for (int i = 0; i < 100; i++)
            {
                decimal x = (decimal)(rnd.NextDouble() * 100);
                decimal y = (decimal)(rnd.NextDouble() * 100);

                ExpressionsLibrary.IExpression expression = ExpressionsLibrary.Expression.Create(x.ToString() + @"<" + y.ToString());
                Assert.AreEqual(expression.objValue, x < y, "Значение должно быть " + (x < y).ToString());
            }

            for (int i = 0; i < 100; i++)
            {
                decimal x = (decimal)(rnd.NextDouble() * 100);
                decimal y = (decimal)(rnd.NextDouble() * 100);

                ExpressionsLibrary.IExpression expression = ExpressionsLibrary.Expression.Create(x.ToString() + @">=" + y.ToString());
                Assert.AreEqual(expression.objValue, x >= y, "Значение должно быть " + (x >= y).ToString());
            }

            for (int i = 0; i < 100; i++)
            {
                decimal x = (decimal)(rnd.NextDouble() * 100);
                decimal y = (decimal)(rnd.NextDouble() * 100);

                ExpressionsLibrary.IExpression expression = ExpressionsLibrary.Expression.Create(x.ToString() + @"<=" + y.ToString());
                Assert.AreEqual(expression.objValue, x <= y, "Значение должно быть " + (x <= y).ToString());
            }
        }


        [TestMethod]
        public void TestMethod3()
        {
            Random rnd = new Random(0);
            for (int i = 0; i < 100; i++)
            {
                decimal x = (decimal)(rnd.NextDouble() * 100);
                decimal y = (decimal)(rnd.NextDouble() * 100);

                ExpressionsLibrary.IExpression expression = ExpressionsLibrary.Expression.Create("Not (" + x.ToString() + '=' + y.ToString() + ")");
                Assert.AreEqual(expression.objValue, x != y, "Значение должно быть " + (x != y).ToString());
            }

            for (int i = 0; i < 100; i++)
            {
                decimal x = (decimal)(rnd.NextDouble() * 100);
                decimal y = (decimal)(rnd.NextDouble() * 100);

                ExpressionsLibrary.IExpression expression = ExpressionsLibrary.Expression.Create("(" + x.ToString() + '=' + y.ToString() + ") AND (" + x.ToString() + '>' + y.ToString() + ")");
                Assert.AreEqual(expression.objValue, (x == y && x > y), "Значение должно быть " + (x == y && x > y).ToString());
            }

            for (int i = 0; i < 100; i++)
            {
                decimal x = (decimal)(rnd.NextDouble() * 100);
                decimal y = (decimal)(rnd.NextDouble() * 100);

                ExpressionsLibrary.IExpression expression = ExpressionsLibrary.Expression.Create("(" + x.ToString() + '=' + y.ToString() + ") OR (" + x.ToString() + '>' + y.ToString() + ")");
                Assert.AreEqual(expression.objValue, (x == y || x > y), "Значение должно быть " + (x == y || x > y).ToString());
            }
        }
    }
}
