using CNFConvertions.Number;
using CNFConvertions.Operations;
using CNFConvertions;
using System.Numerics;

namespace CNFTestsNamespace
{
    [TestFixture]
    [Timeout(1000)]
    internal class CNFMultiplicationTests
    {
        private void Test(INumber a, INumber b, INumber lb, INumber ub)
        {
            OperationMultiplication op = new OperationMultiplication(a, b);
            ResultPair p = op.Evaluate();

            Assert.IsTrue(lb.ToString().Equals(p.LowerBound.ToString()));
            Assert.IsTrue(ub.ToString().Equals(p.UpperBound.ToString()));

            op = new OperationMultiplication(b, a);
            p = op.Evaluate();

            Assert.IsTrue(lb.ToString().Equals(p.LowerBound.ToString()));
            Assert.IsTrue(ub.ToString().Equals(p.UpperBound.ToString()));
        }

        [Test]
        public void TestMultiplicationBigintMulBigintSimple()
        {
            BigInt a, b, res;

            a = new BigInt(6);
            b = new BigInt(7);
            res = new BigInt(42);
            Test(a, b, res, res);

            a = BigInt.GetMax();
            b = new BigInt(1);
            res = BigInt.GetMax();
            Test(a, b, res, res);
        }

        [Test]
        public void TestMultiplicationBigintMulBigintOverflow()
        {
            BigInt a, b;
            KnuthUpArrow res;

            //TODO: Check which Knuth it overflows to

            a = BigInt.GetMax();
            b = new BigInt(2);
            res = new KnuthUpArrow(10, Constants.EXPONENT, 1);
            Test(a, b, res, res);

            a = BigInt.GetMax();
            b = BigInt.GetMax();
            res = new KnuthUpArrow(10, 2 * Constants.EXPONENT, 1);
            Test(a, b, res, res);
        }


        [Test]
        public void TestMultiplicationBigintMulFGH()
        {
            BigInt a = new BigInt(3);
            FGH b = new FGH(CNFHelper.GetOrdinal(1000), new BigInt(3));

            FGH lb = new FGH(CNFHelper.GetOrdinal(1000), new BigInt(3));
            FGH ub = new FGH(CNFHelper.GetOrdinal(1000), new BigInt(4));

            Test(a, b, lb, ub);
        }

        [Test]
        public void TestMultiplicationKnuthMulFGH()
        {
            KnuthUpArrow a = new KnuthUpArrow(new BigInt(3), new BigInt(3), 3);
            FGH b = new FGH(CNFHelper.GetOrdinal(1000), new BigInt(3));

            FGH lb = new FGH(CNFHelper.GetOrdinal(1000), new BigInt(3));
            FGH ub = new FGH(CNFHelper.GetOrdinal(1000), new BigInt(4));

            Test(a, b, lb, ub);
        }

        [Test]
        public void TestMultiplicationFGHMulFGH()
        {
            FGH a = new FGH(CNFHelper.GetOrdinal(4200), new BigInt(7));
            FGH b = new FGH(CNFHelper.GetOrdinal(4201), new BigInt(7));

            FGH lb = new FGH(CNFHelper.GetOrdinal(4201), new BigInt(7));
            FGH ub = new FGH(CNFHelper.GetOrdinal(4201), new BigInt(8));

            Test(a, b, lb, ub);


            a = new FGH(CNFHelper.GetOrdinal(4200), new BigInt(7));
            b = new FGH(CNFHelper.GetOrdinal(4200), new BigInt(8));

            lb = new FGH(CNFHelper.GetOrdinal(4200), new BigInt(8));
            ub = new FGH(CNFHelper.GetOrdinal(4200), new BigInt(9));

            Test(a, b, lb, ub);
        }

        [Test]
        public void TestMultiplicationFGHMulFGHOverflowAlpha()
        {
            FGH a = new FGH(CNFHelper.GetOrdinal(4200), BigInt.GetMax());
            FGH b = new FGH(CNFHelper.GetOrdinal(4200), new BigInt(3));

            FGH lb = new FGH(CNFHelper.GetOrdinal(4200), BigInt.GetMax());
            FGH ub = new FGH(CNFHelper.GetOrdinal(4201), new BigInt(3));

            Test(a, b, lb, ub);
        }

        [Test]
        public void TestMultiplicationBigintMulKnuthOverflowFGH()
        {
            BigInt a = BigInt.GetMin();
            KnuthUpArrow b = KnuthUpArrow.GetMax();

            KnuthUpArrow lb = KnuthUpArrow.GetMax();
            FGH ub = FGH.GetMin();

            Test(a, b, lb, ub);
        }

        [Test]
        public void TestMultiplicationKnuthMulKnuthOverflowFGH()
        {
            KnuthUpArrow a = KnuthUpArrow.GetMin();
            KnuthUpArrow b = KnuthUpArrow.GetMax();

            KnuthUpArrow lb = KnuthUpArrow.GetMax();
            FGH ub = FGH.GetMin();

            Test(a, b, lb, ub);
        }

        [Test]
        public void TestMultiplicationBigKnuthMulBigInt()
        {
            KnuthUpArrow a = new KnuthUpArrow(new BigInt(3), new BigInt(3), 3);
            BigInt b = BigInt.GetMax();

            KnuthUpArrow lb = new KnuthUpArrow(new BigInt(3), new BigInt(3), 3);
            KnuthUpArrow ub = new KnuthUpArrow(new BigInt(4), new BigInt(3), 3);

            Test(a, b, lb, ub);
        }

        [Test]
        public void TestMultiplicationBigKnuth()
        {
            KnuthUpArrow a, b, lb, ub;

            a = new KnuthUpArrow(new BigInt(3), new BigInt(3), 3);
            b = new KnuthUpArrow(BigInt.GetMax(), BigInt.GetMax(), 1);
            lb = new KnuthUpArrow(new BigInt(3), new BigInt(3), 3);
            ub = new KnuthUpArrow(new BigInt(4), new BigInt(3), 3);
            Test(a, b, lb, ub);

            a = new KnuthUpArrow(new BigInt(3), new BigInt(3), 3);
            b = new KnuthUpArrow(BigInt.GetMax(), BigInt.GetMax(), 2);
            lb = new KnuthUpArrow(new BigInt(3), new BigInt(3), 3);
            ub = new KnuthUpArrow(new BigInt(4), new BigInt(3), 3);
            Test(a, b, lb, ub);

            a = new KnuthUpArrow(new BigInt(3), new BigInt(3), 3);
            b = new KnuthUpArrow(BigInt.GetMax(), BigInt.GetMax(), 3);
            lb = new KnuthUpArrow(BigInt.GetMax(), BigInt.GetMax(), 3);
            ub = new KnuthUpArrow(new BigInt(3), new BigInt(3), 4);
            Test(a, b, lb, ub);

            a = new KnuthUpArrow(new BigInt(3), new BigInt(3), 3);
            b = new KnuthUpArrow(BigInt.GetMax(), BigInt.GetMax(), 3);
            lb = new KnuthUpArrow(BigInt.GetMax(), BigInt.GetMax(), 3);
            ub = new KnuthUpArrow(new BigInt(3), new BigInt(3), 4);
            Test(a, b, lb, ub);

            a = new KnuthUpArrow(new BigInt(3), new BigInt(3), 4);
            b = new KnuthUpArrow(BigInt.GetMax(), BigInt.GetMax(), 3);
            lb = new KnuthUpArrow(new BigInt(3), new BigInt(3), 4);
            ub = new KnuthUpArrow(new BigInt(4), new BigInt(3), 4);
            Test(a, b, lb, ub);

            a = new KnuthUpArrow(new BigInt(4), new BigInt(7), 4);
            b = new KnuthUpArrow(new BigInt(7), new BigInt(4), 4);
            lb = new KnuthUpArrow(new BigInt(4), new BigInt(7), 4);
            ub = new KnuthUpArrow(new BigInt(5), new BigInt(7), 4);
            Test(a, b, lb, ub);

            a = new KnuthUpArrow(BigInt.GetMax(), new BigInt(3), 3);
            b = new KnuthUpArrow(new BigInt(3), new BigInt(3), 3);
            lb = new KnuthUpArrow(BigInt.GetMax(), new BigInt(3), 3);
            ub = new KnuthUpArrow(new BigInt(3), new BigInt(4), 3);
            Test(a, b, lb, ub);

            a = new KnuthUpArrow(BigInt.GetMax(), BigInt.GetMax(), 4);
            b = new KnuthUpArrow(new BigInt(3), new BigInt(3), 4);
            lb = new KnuthUpArrow(BigInt.GetMax(), BigInt.GetMax(), 4);
            ub = new KnuthUpArrow(new BigInt(3), new BigInt(3), 5);
            Test(a, b, lb, ub);
        }


        [Test]
        public void TestMultiplicationSmall2ArrowKnuth()
        {
            KnuthUpArrow a, b2;
            BigInt b1, lb, ub;

            a = new KnuthUpArrow(new BigInt(3), new BigInt(3), 2);
            b1 = new BigInt(3);
            lb = new BigInt(22876792454961);
            ub = new BigInt(22876792454961);
            Test(a, b1, lb, ub);

            a = new KnuthUpArrow(new BigInt(3), new BigInt(3), 2);
            b2 = new KnuthUpArrow(new BigInt(4), new BigInt(3), 1);
            lb = new BigInt(488038239039168);
            ub = new BigInt(488038239039168);
            Test(a, b2, lb, ub);

            a = new KnuthUpArrow(new BigInt(3), new BigInt(3), 2);
            b2 = new KnuthUpArrow(new BigInt(3), new BigInt(3), 2);
            lb = new BigInt(BigInteger.Parse("58149737003040060000000000"));
            ub = new BigInt(BigInteger.Parse("58149737003040060000000000"));
            Test(a, b2, lb, ub);
        }


        [Test]
        public void TestMultiplicationBig2ArrowKnuth()
        {
            KnuthUpArrow a, b1, lb, ub;
            BigInt b2;

            a = new KnuthUpArrow(new BigInt(5), new BigInt(3), 2);
            b1 = new KnuthUpArrow(new BigInt(4), new BigInt(3), 1);
            lb = new KnuthUpArrow(new BigInt(5), new BigInt(3), 2);
            ub = new KnuthUpArrow(new BigInt(5), new BigInt(4), 2);
            Test(a, b1, lb, ub);

            a = new KnuthUpArrow(new BigInt(3), new BigInt(4), 2);
            b1 = new KnuthUpArrow(new BigInt(42), new BigInt(73), 2);
            lb = new KnuthUpArrow(new BigInt(42), new BigInt(73), 2);
            ub = new KnuthUpArrow(new BigInt(42), new BigInt(74), 2);
            Test(a, b1, lb, ub);

            a = new KnuthUpArrow(new BigInt(5), new BigInt(3), 2);
            b2 = new BigInt(42);

            KnuthUpArrow aOneArrow = new KnuthUpArrow(new BigInt(5), new BigInt(3125), 1);
            OperationMultiplication op = new OperationMultiplication(aOneArrow, b2);
            ResultPair p = op.Evaluate();

            Test(a, b1, p.LowerBound, p.UpperBound);
        }


        [Test]
        public void TestMultiplication1ArrowKnuth()
        {
            KnuthUpArrow a, b2, lb2, ub2;
            BigInt b1, lb1, ub1;

            a = new KnuthUpArrow(3, 3, 1);
            b1 = new BigInt(3);
            lb1 = new BigInt(30);
            ub1 = new BigInt(30);
            Test(a, b1, lb1, ub1);

            a = new KnuthUpArrow(4, 5, 1);
            b1 = new BigInt(32);
            lb1 = new BigInt(32768);
            ub1 = new BigInt(32768);
            Test(a, b1, lb1, ub1);

            a = new KnuthUpArrow(new BigInt(4), new BigInt(2000), 1);
            b1 = new BigInt(64);
            lb2 = new KnuthUpArrow(new BigInt(4), new BigInt(2003), 1);
            ub2 = new KnuthUpArrow(new BigInt(5), new BigInt(2003), 1);
            Test(a, b1, lb2, ub2);

            a = new KnuthUpArrow(new BigInt(4), new BigInt(2000), 1);
            b2 = new KnuthUpArrow(new BigInt(64), new BigInt(512), 1);

            lb2 = new KnuthUpArrow(new BigInt(4), new BigInt(3536), 1);
            ub2 = new KnuthUpArrow(new BigInt(5), new BigInt(3536), 1);
            Test(a, b2, lb2, ub2);
        }
    }
}
