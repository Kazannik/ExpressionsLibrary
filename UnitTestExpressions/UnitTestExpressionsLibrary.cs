using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace UnitTestExpressions
{
    [TestClass]
    public class UnitTestExpressionsLibrary
    {
        [TestMethod]
        public void TestMethod1()
        {
            Random rnd = new Random(0);
            for (int i = 0; i < 100; i++)
            {
                int x = (int)(rnd.NextDouble() * 100) ;
                int y = (int)(rnd.NextDouble() * 100);

                ExpressionsLibrary.ArithmeticExpression expression = ExpressionsLibrary.ArithmeticExpression.Create( x.ToString() + '+' + y.ToString());
                Assert.AreEqual(expression.Value, x+y, "Значение должно быть равно " + (x+y).ToString());
                Debug.WriteLine(x.ToString() + '+' + y.ToString() + '=' + (x + y).ToString());
            }

            for (int i = 0; i < 100; i++)
            {
                int x = (int)(rnd.NextDouble() * 100);
                int y = (int)(rnd.NextDouble() * 100);

                ExpressionsLibrary.ArithmeticExpression expression = ExpressionsLibrary.ArithmeticExpression.Create(x.ToString() + '-' + y.ToString());
                Assert.AreEqual(expression.Value, x - y, "Значение должно быть равно " + (x - y).ToString());
                Debug.WriteLine(x.ToString() + '-' + y.ToString() + '=' + (x - y).ToString());
            }

            for (int i = 0; i < 100; i++)
            {
                int x = (int)(rnd.NextDouble() * 100);
                int y = (int)(rnd.NextDouble() * 100);

                ExpressionsLibrary.ArithmeticExpression expression = ExpressionsLibrary.ArithmeticExpression.Create(x.ToString() + '*' + y.ToString());
                Assert.AreEqual(expression.Value, x * y, "Значение должно быть равно " + (x * y).ToString());
                Debug.WriteLine(x.ToString() + '*' + y.ToString() + '=' + (x * y).ToString());
            }

        }
    }
}
