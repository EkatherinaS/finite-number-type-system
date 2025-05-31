using CNFConvertions.Number;
using System.Numerics;

namespace CNFTestsNamespace
{
    [TestFixture]
    [Timeout(1000)]
    internal class CNFComparisonTests
    {
        private void TestMinMax(INumber min, INumber max)
        {
            Assert.IsTrue(max.CompareTo(min) > 0);
            Assert.IsTrue(min.CompareTo(max) < 0);
            Assert.IsTrue(max.CompareTo(max) == 0);
            Assert.IsTrue(min.CompareTo(min) == 0);
            Assert.IsTrue(max.Equals(max));
            Assert.IsTrue(min.Equals(min));
            Assert.IsFalse(min.Equals(max));
            Assert.IsFalse(max.Equals(min));
        }

        private void TestEquals(INumber a, INumber b)
        {
            Assert.IsTrue(a.CompareTo(b) == 0);
            Assert.IsTrue(b.CompareTo(a) == 0);
            Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(b.Equals(a));

            TestMinMax(b, a.Succ());
            TestMinMax(a, b.Succ());
        }

        [Test]
        public void TestCompareBigIntToBigInt()
        {
            BigInt max = new BigInt(42);
            BigInt min = new BigInt(17);

            Assert.IsTrue(max > min);
            TestMinMax(min, max);

            max = BigInt.GetMax();
            min = BigInt.GetMin();

            Assert.IsTrue(max > min);
            TestMinMax(min, max);
        }

        [Test]
        public void TestCompareBigIntToKnuth()
        {
            KnuthUpArrow knuth;
            BigInt bigint;

            knuth = new KnuthUpArrow(new BigInt(5), new BigInt(6), 1);
            bigint = new BigInt(15625);
            TestEquals(knuth, bigint);

            knuth = new KnuthUpArrow(new BigInt(3), new BigInt(3), 2);
            bigint = new BigInt(7625597484987);
            TestEquals(knuth, bigint);

            bigint = BigInt.GetMax();
            
            knuth = new KnuthUpArrow(new BigInt(BigInteger.Pow(10, 333)), new BigInt(3), 1);
            TestMinMax(knuth, bigint);
            knuth = new KnuthUpArrow(new BigInt(BigInteger.Pow(11, 333)), new BigInt(3), 1);
            TestMinMax(bigint, knuth);
            knuth = new KnuthUpArrow(new BigInt(BigInteger.Pow(10, 334)), new BigInt(3), 1);
            TestMinMax(bigint, knuth);
            knuth = new KnuthUpArrow(new BigInt(BigInteger.Pow(10, 333)), new BigInt(4), 1);
            TestMinMax(bigint, knuth);

            knuth = new KnuthUpArrow(new BigInt(3), new BigInt(2000), 1);
            TestMinMax(knuth, bigint);
            knuth = new KnuthUpArrow(new BigInt(4), new BigInt(2095), 1);
            TestMinMax(bigint, knuth);
            knuth = new KnuthUpArrow(new BigInt(3), new BigInt(2096), 1);
            TestMinMax(bigint, knuth);

            knuth = new KnuthUpArrow(new BigInt(4), new BigInt(3), 2);
            TestMinMax(knuth, bigint);
            knuth = new KnuthUpArrow(new BigInt(5), new BigInt(3), 2);
            TestMinMax(bigint, knuth);
            knuth = new KnuthUpArrow(new BigInt(4), new BigInt(4), 2);
            TestMinMax(bigint, knuth);
        }

        [Test]
        public void TestCompareBigIntToFGH()
        {
            BigInt bigint = BigInt.GetMax();
            FGH fgh = FGH.GetMin();

            TestMinMax(bigint, fgh);
        }

        [Test]
        public void TestCompareKnuthToKnuth()
        {
            KnuthUpArrow a, b;

            a = new KnuthUpArrow(new BigInt(3), new BigInt(3), 2);
            b = new KnuthUpArrow(new BigInt(3), new BigInt(27), 1);
            TestEquals(a, b);

            a = new KnuthUpArrow(new BigInt(4), new BigInt(3), 2);
            b = new KnuthUpArrow(new BigInt(4), new BigInt(256), 1);
            TestEquals(a, b);

            a = new KnuthUpArrow(BigInt.GetMax(), BigInt.GetMax(), 1);
            b = new KnuthUpArrow(new BigInt(4), new BigInt(4), 2);
            TestMinMax(b, a);
            b = new KnuthUpArrow(new BigInt(3), new BigInt(5), 2);
            TestMinMax(a, b);
            b = new KnuthUpArrow(new BigInt(387), new BigInt(3), 2);
            TestMinMax(a, b);

            a = new KnuthUpArrow(BigInt.GetMax(), BigInt.GetMax(), 2);
            b = new KnuthUpArrow(new BigInt(3), new BigInt(3), 3);
            TestMinMax(b, a);
            b = new KnuthUpArrow(new BigInt(4), new BigInt(3), 3);
            TestMinMax(a, b);
            b = new KnuthUpArrow(new BigInt(3), new BigInt(4), 3);
            TestMinMax(a, b);

            a = new KnuthUpArrow(BigInt.GetMax(), BigInt.GetMax(), 3);
            b = new KnuthUpArrow(new BigInt(3), new BigInt(3), 4);
            TestMinMax(a, b);

            a = new KnuthUpArrow(BigInt.GetMax(), BigInt.GetMax(), 4);
            b = new KnuthUpArrow(new BigInt(3), new BigInt(3), 5);
            TestMinMax(a, b);
        }

        [Test]
        public void TestCompareKnuthToFGH()
        {
            KnuthUpArrow knuth = KnuthUpArrow.GetMax();
            FGH fgh = FGH.GetMin();

            TestMinMax(knuth, fgh);
        }

        [Test]
        public void TestCompareFGHToFGH()
        {
            FGH a = new FGH(CNFHelper.GetOrdinal(1000), new BigInt(3));
            FGH b = new FGH(CNFHelper.GetOrdinal(1001), new BigInt(3));
            TestMinMax(a, b);
            b = new FGH(CNFHelper.GetOrdinal(1000), new BigInt(4));
            TestMinMax(a, b);

            a = new FGH(CNFHelper.GetOrdinal(1000), BigInt.GetMax());
            b = new FGH(CNFHelper.GetOrdinal(1001), new BigInt(3));
            TestMinMax(a, b);
        }
    }
}
