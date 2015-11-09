using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using winFileSearchLib;

namespace winFileSearchTests
{
    [TestClass]
    public class UnitTestCircularQueue
    {
        [TestMethod]
        public void CircularQueue_push_cmp()
        {
            CircularQueue<int> x = new CircularQueue<int>(3);
            x.push(10);
            Assert.IsTrue(x.cmp(new[] { 10 }));
            x.push(15);
            Assert.IsTrue(x.cmp(new[] { 10, 15}));
            x.push(20);
            Assert.IsTrue(x.cmp(new[] { 10, 15, 20 }));
            x.push(25);
            Assert.IsTrue(x.cmp(new[] { 15, 20, 25 }));
        }
    }
}
