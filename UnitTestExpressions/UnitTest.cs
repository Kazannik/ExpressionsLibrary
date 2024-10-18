﻿using ExpressionsLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestExpressions
{
    [TestClass]
    public class UnitTest
    {

        [TestMethod]
        public void TestTextReaderMethod()
        {
            IExpression expression = Expression.Create(@"1 + 1");
            Assert.AreEqual(expression.objValue, (decimal)2, "Значение должно быть " + 2);

            expression = Expression.Create(@"2 - 1");
            Assert.AreEqual(expression.objValue, 1, "Значение должно быть " + 1);

            expression = Expression.Create(@"2 / 2");
            Assert.AreEqual(expression.objValue, 1, "Значение должно быть " + 1);

            expression = Expression.Create(@"2 * 2");
            Assert.AreEqual(expression.objValue, 4, "Значение должно быть " + 4);

            expression = Expression.Create(@"2 \ 2");
            Assert.AreEqual(expression.objValue, 0, "Значение должно быть " + 0);

            expression = Expression.Create(@"(2 - 1) + 2");
            Assert.AreEqual(expression.objValue, 3, "Значение должно быть " + 3);

            expression = Expression.Create(@"(2 + 1) + 2");
            Assert.AreEqual(expression.objValue, 5, "Значение должно быть " + 5);

            expression = Expression.Create(@"2 - 1");
            Assert.AreEqual(expression.objValue, 1, "Значение должно быть " + 1);

            expression = Expression.Create(@"2 - 1");
            Assert.AreEqual(expression.objValue, 1, "Значение должно быть " + 1);

            expression = Expression.Create(@"2 - 1");
            Assert.AreEqual(expression.objValue, 1, "Значение должно быть " + 1);

            expression = Expression.Create(@"2 - 1");
            Assert.AreEqual(expression.objValue, 1, "Значение должно быть " + 1);

            expression = Expression.Create(@"2 - 1");
            Assert.AreEqual(expression.objValue, 1, "Значение должно быть " + 1);

        }


        [TestMethod]
        public void TestMethod1()
        {
            Random rnd = new Random(0);
            for (int i = 0; i < 100; i++)
            {
                decimal x = (decimal)(rnd.NextDouble() * 100);
                decimal y = (decimal)(rnd.NextDouble() * 100);

                IExpression expression = Expression.Create(x.ToString() + '+' + y.ToString());
                Assert.AreEqual(expression.objValue, x + y, "Значение должно быть равно " + (x + y).ToString());
            }

            for (int i = 0; i < 100; i++)
            {
                decimal x = (decimal)(rnd.NextDouble() * 100);
                decimal y = (decimal)(rnd.NextDouble() * 100);

                IExpression expression = Expression.Create(x.ToString() + '-' + y.ToString());
                Assert.AreEqual(expression.objValue, x - y, "Значение должно быть равно " + (x - y).ToString());
            }

            for (int i = 0; i < 100; i++)
            {
                decimal x = (decimal)(rnd.NextDouble() * 100);
                decimal y = (decimal)(rnd.NextDouble() * 100);

                IExpression expression = Expression.Create(x.ToString() + '*' + y.ToString());
                Assert.AreEqual(expression.objValue, x * y, "Значение должно быть равно " + (x * y).ToString());
            }

            for (int i = 0; i < 100; i++)
            {
                decimal x = (decimal)(rnd.NextDouble() * 100);
                decimal y = (decimal)(rnd.NextDouble() * 100) + 1;

                IExpression expression = Expression.Create(x.ToString() + '/' + y.ToString());
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

                IExpression expression = Expression.Create("(" + d1 + " + " + d2 + ") /" + d3 + "+" + d4 + "*" + d5 + "-" + d6);
                Assert.AreEqual(expression.objValue, (d1 + d2) / d3 + d4 * d5 - d6, "Значение должно быть равно " + ((d1 + d2) / d3 + d4 * d5 - d6).ToString());
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

                IExpression expression = Expression.Create(x.ToString() + '=' + y.ToString());
                Assert.AreEqual(expression.objValue, x == y, "Значение должно быть " + (x == y).ToString());
            }


            for (int i = 0; i < 100; i++)
            {
                decimal x = (decimal)(rnd.NextDouble() * 100);
                decimal y = (decimal)(rnd.NextDouble() * 100);

                IExpression expression = Expression.Create(x.ToString() + @"<>" + y.ToString());
                Assert.AreEqual(expression.objValue, x != y, "Значение должно быть " + (x != y).ToString());
            }

            for (int i = 0; i < 100; i++)
            {
                decimal x = (decimal)(rnd.NextDouble() * 100);
                decimal y = (decimal)(rnd.NextDouble() * 100);

                IExpression expression = Expression.Create(x.ToString() + @">" + y.ToString());
                Assert.AreEqual(expression.objValue, x > y, "Значение должно быть " + (x > y).ToString());
            }

            for (int i = 0; i < 100; i++)
            {
                decimal x = (decimal)(rnd.NextDouble() * 100);
                decimal y = (decimal)(rnd.NextDouble() * 100);

                IExpression expression = Expression.Create(x.ToString() + @"<" + y.ToString());
                Assert.AreEqual(expression.objValue, x < y, "Значение должно быть " + (x < y).ToString());
            }

            for (int i = 0; i < 100; i++)
            {
                decimal x = (decimal)(rnd.NextDouble() * 100);
                decimal y = (decimal)(rnd.NextDouble() * 100);

                IExpression expression = Expression.Create(x.ToString() + @">=" + y.ToString());
                Assert.AreEqual(expression.objValue, x >= y, "Значение должно быть " + (x >= y).ToString());
            }

            for (int i = 0; i < 100; i++)
            {
                decimal x = (decimal)(rnd.NextDouble() * 100);
                decimal y = (decimal)(rnd.NextDouble() * 100);

                IExpression expression = Expression.Create(x.ToString() + @"<=" + y.ToString());
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

                IExpression expression = Expression.Create("Not (" + x.ToString() + '=' + y.ToString() + ")");
                Assert.AreEqual(expression.objValue, x != y, "Значение должно быть " + (x != y).ToString());
            }

            for (int i = 0; i < 100; i++)
            {
                decimal x = (decimal)(rnd.NextDouble() * 100);
                decimal y = (decimal)(rnd.NextDouble() * 100);

                IExpression expression = Expression.Create("(" + x.ToString() + '=' + y.ToString() + ") AND (" + x.ToString() + '>' + y.ToString() + ")");
                Assert.AreEqual(expression.objValue, (x == y && x > y), "Значение должно быть " + (x == y && x > y).ToString());
            }

            for (int i = 0; i < 100; i++)
            {
                decimal x = (decimal)(rnd.NextDouble() * 100);
                decimal y = (decimal)(rnd.NextDouble() * 100);

                IExpression expression = Expression.Create("NOT ((" + x.ToString() + '=' + y.ToString() + ") OR (" + x.ToString() + '>' + y.ToString() + "))");
                Assert.AreEqual(expression.objValue, (!(x == y || x > y)), "Значение должно быть " + (!(x == y || x > y)).ToString() + " => " + expression.Formula());
            }
        }

        [TestMethod]
        public void TestMethod4()
        {
            Random rnd = new Random(0);
            for (int i = 0; i < 100; i++)
            {
                decimal x = (decimal)(rnd.NextDouble() * 100);
                decimal y = (decimal)(rnd.NextDouble() * 100);

                IExpression expression = Expression.Create("[1] + [2] * [1] - [1]", @"\[\d+\]");
                Assert.AreEqual(expression.Count, 2, "Значение должно быть " + 2);

                expression["[1]"].SetValue(x);
                expression["[2]"].SetValue(y);

                Assert.AreEqual(expression.objValue, x + y * x - x, "Значение должно быть " + (x + y * x - x).ToString());
            }

            for (int i = 0; i < 100; i++)
            {
                decimal d1 = (decimal)(rnd.NextDouble() * 100);
                decimal d2 = (decimal)(rnd.NextDouble() * 100);
                decimal d3 = (decimal)(rnd.NextDouble() * 100);
                decimal d4 = (decimal)(rnd.NextDouble() * 100) + 1;
                decimal d5 = (decimal)(rnd.NextDouble() * 100);

                IExpression expression = Expression.Create("[1] + [2] * [1] - [3] / [4] - [5]", @"\[\d+\]");
                Assert.AreEqual(expression.Count, 5, "Значение должно быть " + 5);

                expression["[1]"].SetValue(d1);
                expression["[2]"].SetValue(d2);
                expression["[3]"].SetValue(d3);
                expression["[4]"].SetValue(d4);
                expression["[5]"].SetValue(d5);

                Assert.AreEqual(expression.objValue, d1 + d2 * d1 - d3 / d4 - d5, "Значение должно быть " + (d1 + d2 * d1 - d3 / d4 - d5).ToString());
            }
        }


        [TestMethod]
        public void TestMethod5()
        {
            Random rnd = new Random(0);

            for (int i = 0; i < 100; i++)
            {
                decimal[] d = {
                    (decimal)(rnd.NextDouble() * 100) + 1,
                    (decimal)(rnd.NextDouble() * 100) + 1,
                    (decimal)(rnd.NextDouble() * 100) + 1,
                    (decimal)(rnd.NextDouble() * 100) + 1,
                    (decimal)(rnd.NextDouble() * 100) + 1
                };

                IExpression expression = Expression.Create("[1] + [2] * [1] - [3] / [4] - [5]", @"\[\d+\]");
                Assert.AreEqual(expression.Count, 5, "Значение должно быть " + 5);

                int t = 0;
                foreach (ExpressionsLibrary.ArithmeticExpressions.ICell cell in expression)
                {
                    cell.SetValue(d[t]);
                    t++;
                }

                Assert.AreEqual(expression.objValue, d[0] + d[1] * d[0] - d[2] / d[3] - d[4], "Значение должно быть " + (d[0] + d[1] * d[0] - d[2] / d[3] - d[4]).ToString());
            }
        }


        
    }
}
