using CNFConvertions;
using CNFConvertions.Number;
using CNFConvertions.Operations;
using System.Numerics;

namespace CNFTestsNamespace
{
    [TestFixture]
    [Timeout(1000)]
    internal class CNFConvertionsTests
    {
        private void Test(INumber a, INumber b)
        {
            INumber res = a.Succ();
            Assert.IsTrue(res.ToString().Equals(b.ToString()));
        }


        [Test]
        public void TestBigIntSucc()
        {
            INumber a, b;

            a = new BigInt(1);
            b = new BigInt(2);
            Test(a, b);

            a = new BigInt(BigInteger.Pow(10, 1000) - 1);
            b = BigInt.GetMax();
            Test(a, b);

            a = BigInt.GetMax();
            b = new KnuthUpArrow(10, 1000, 1);
            Test(a, b);
        }

        [Test]
        public void TestKnuthSucc()
        {
            INumber a, b;

            a = new KnuthUpArrow(4, 5, 1);
            b = new KnuthUpArrow(5, 5, 1);
            Test(a, b);

            a = new KnuthUpArrow(BigInt.GetMax(), new BigInt(3), 2);
            b = new KnuthUpArrow(BigInt.GetMax(), new BigInt(4), 2);
            Test(a, b);

            a = new KnuthUpArrow(BigInt.GetMax(), new BigInt(3), 3);
            b = new KnuthUpArrow(new BigInt(3), new BigInt(4), 3);
            Test(a, b);

            a = new KnuthUpArrow(BigInt.GetMax(), BigInt.GetMax(), 1);
            b = new KnuthUpArrow(new BigInt(3), new BigInt(5), 2);
            Test(a, b);

            a = new KnuthUpArrow(BigInt.GetMax(), BigInt.GetMax(), Constants.ARROW_COUNT);
            b = FGH.GetMin();
            Test(a, b);
        }

        [Test]
        public void TestFGHSucc()
        {
            INumber a, b;

            a = new FGH(CNFHelper.GetOrdinal(1000), new BigInt(42));
            b = new FGH(CNFHelper.GetOrdinal(1000), new BigInt(43));
            Test(a, b);

            a = new FGH(CNFHelper.GetOrdinal(1000), BigInt.GetMax());
            b = new FGH(CNFHelper.GetOrdinal(1001), new BigInt(3));
            Test(a, b);
        }
    }
}