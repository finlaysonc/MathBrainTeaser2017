using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathBrainTeaser2017;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathBrainTeaser2017.Tests
{
    [TestClass()]
    public class ConstExprTests
    {
        [TestMethod()]
        public void ConstExprTest()
        {
            ConstExpr c = new ConstExpr("2017", -1);
            Assert.AreEqual("201.7", c.ToString());
        }

        [TestMethod()]
        public void EqualsTest()
        {
            ConstExpr c = new ConstExpr("21", 1);
            ConstExpr c2 = new ConstExpr("21.0", 1);
            Assert.IsTrue(c.Equals(c2));
            Assert.IsTrue(c == c2);
            Assert.AreEqual(c, c2);
        }

        [TestMethod()]
        public void GetHashCodeTest()
        {
            Assert.Fail();
        }
    }
}