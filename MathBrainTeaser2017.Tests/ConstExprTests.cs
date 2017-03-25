using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathBrainTeaser2017;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathBrainTeaser2017.Tests
{


    public static class Utils
    {
        public static ConstExpr Two = new ConstExpr("2", 0);
        public static ConstExpr Seven  = new ConstExpr("7", 0);

        public static ConstExpr MakeNum(int num)
        {
            return new ConstExpr(num.ToString(), 0);
        }
    }
    [TestClass()]
    public class UnaryExprTests
    {
        [TestMethod()]
        public void TestEquals()
        {
            ConstExpr c = new ConstExpr("0", 0);
            Factorial f = new Factorial(c);
            DoubleFactorial f2 = new DoubleFactorial(c);
            Assert.IsTrue(f.Equals(f2));

            BinaryExpr b = new AddExpr(Utils.Two, Utils.MakeNum(5));
            BinaryExpr b2 = new SubExpr(Utils.MakeNum(9), Utils.Two);
            BinaryExpr b3 = new AddExpr( Utils.MakeNum(5), Utils.Two);
            BinaryExpr b4 = new AddExpr(Utils.MakeNum(5), new ConstExpr("20", -1));
            Assert.IsFalse(b.Equals(b2));
            Assert.IsTrue(b.Equals(b3));
            Assert.IsTrue(b3.Equals(b4));
        }
    }


    [TestClass()]
    public class ConstExprTests
    {
        [TestMethod()]
        public void Profile()
        {
            ASTSolution problem = new ASTSolution();

            var result = problem.Run();

        }


        [TestMethod()]
        public void ConstExprTest()
        {
            ConstExpr c = new ConstExpr("2017", -1);
            Assert.AreEqual("201.7", c.ToString());
        }

        [TestMethod()]
        public void EqualsTest()
        {
            ConstExpr c = new ConstExpr("21", 0);
            ConstExpr c2 = new ConstExpr("210",-1);
            Assert.IsTrue(c.Equals(c2));
            Assert.AreEqual(c, c2);
        }

        [TestMethod()]
        public void GetHashCodeTest()
        {
            Assert.Fail();
        }


    }
}