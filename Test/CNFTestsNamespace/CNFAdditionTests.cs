using CNFConvertions.Number;
using CNFConvertions.Operations;
using CNFConvertions;

namespace CNFTestsNamespace
{
    [TestFixture]
    [Timeout(1000)]
    internal class CNFAdditionTests
    {
        private void Test(INumber a, INumber b, INumber lb, INumber ub)
        {
            OperationAddition op = new OperationAddition(a, b);
            ResultPair p = op.Evaluate();

            Assert.IsTrue(lb.ToString().Equals(p.LowerBound.ToString()));
            Assert.IsTrue(ub.ToString().Equals(p.UpperBound.ToString()));

            op = new OperationAddition(b, a);
            p = op.Evaluate();

            Assert.IsTrue(lb.ToString().Equals(p.LowerBound.ToString()));
            Assert.IsTrue(ub.ToString().Equals(p.UpperBound.ToString()));
        }

        [Test]
        public void TestAdditionBigintAddBigintSimple()
        {
            BigInt a = new BigInt(37);
            BigInt b = new BigInt(5);

            BigInt res = new BigInt(42);

            Test(a, b, res, res);
        }

        [Test]
        public void TestAdditionBigintAddBigintOverflow()
        {
            BigInt a = BigInt.GetMax();
            BigInt b = new BigInt(2);

            KnuthUpArrow res = new KnuthUpArrow(10, Constants.EXPONENT + 1, 1);

            Test(a, b, res, res);
        }

        [Test]
        public void TestAdditionKnuthAddKnuthSimple()
        {
            KnuthUpArrow a = new KnuthUpArrow(new BigInt(4), new BigInt(7), 4);
            KnuthUpArrow b = new KnuthUpArrow(new BigInt(7), new BigInt(4), 4);

            KnuthUpArrow lb = new KnuthUpArrow(new BigInt(4), new BigInt(7), 4);
            KnuthUpArrow ub = new KnuthUpArrow(new BigInt(5), new BigInt(7), 4);

            Test(a, b, lb, ub);
        }

        [Test]
        public void TestAdditionKnuthAddKnuthOverflowB()
        {
            KnuthUpArrow a = new KnuthUpArrow(BigInt.GetMax(), new BigInt(3), 3);
            KnuthUpArrow b = new KnuthUpArrow(new BigInt(3), new BigInt(3), 3);

            KnuthUpArrow lb = new KnuthUpArrow(BigInt.GetMax(), new BigInt(3), 3);
            KnuthUpArrow ub = new KnuthUpArrow(new BigInt(3), new BigInt(4), 3);

            Test(a, b, lb, ub);
        }

        [Test]
        public void TestAdditionKnuthAddKnuthOverflowN()
        {
            KnuthUpArrow a = new KnuthUpArrow(BigInt.GetMax(), BigInt.GetMax(), 4);
            KnuthUpArrow b = new KnuthUpArrow(new BigInt(3), new BigInt(3), 4);

            KnuthUpArrow lb = new KnuthUpArrow(BigInt.GetMax(), BigInt.GetMax(), 4);
            KnuthUpArrow ub = new KnuthUpArrow(new BigInt(3), new BigInt(3), 5);

            Test(a, b, lb, ub);
        }

        [Test]
        public void TestAdditionKnuthAddKnuthOverflowFGH()
        {
            KnuthUpArrow a = KnuthUpArrow.GetMax();
            KnuthUpArrow b = KnuthUpArrow.GetMin();

            KnuthUpArrow lb = KnuthUpArrow.GetMax();
            FGH ub = FGH.GetMin();

            Test(a, b, lb, ub);
        }

        [Test]
        public void TestAdditionFGHAddFGH()
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
        public void TestAdditionFGHAddFGHOverflowAlpha()
        {
            FGH a = new FGH(CNFHelper.GetOrdinal(4200), BigInt.GetMax());
            FGH b = new FGH(CNFHelper.GetOrdinal(4200), new BigInt(3));

            FGH lb = new FGH(CNFHelper.GetOrdinal(4200), BigInt.GetMax());
            FGH ub = new FGH(CNFHelper.GetOrdinal(4201), new BigInt(3));

            Test(a, b, lb, ub);
        }

        [Test]
        public void TestAdditionBigintAddSmallKnuth()
        {
            KnuthUpArrow a = new KnuthUpArrow(3, 3, 1);
            BigInt b = new BigInt(3);

            BigInt res = new BigInt(30);

            Test(a, b, res, res);
        }

        [Test]
        public void TestAdditionBigintAddBigKnuth()
        {
            BigInt a = new BigInt(3);
            KnuthUpArrow b = new KnuthUpArrow(new BigInt(3), new BigInt(3), 3);

            KnuthUpArrow lb = new KnuthUpArrow(new BigInt(3), new BigInt(3), 3);
            KnuthUpArrow ub = new KnuthUpArrow(new BigInt(4), new BigInt(3), 3);

            Test(a, b, lb, ub);
        }

        [Test]
        public void TestAdditionBigintAddBigKnuthOverflow()
        {
            BigInt a = BigInt.GetMin();
            KnuthUpArrow b = KnuthUpArrow.GetMax();

            KnuthUpArrow lb = KnuthUpArrow.GetMax();
            FGH ub = FGH.GetMin();

            Test(a, b, lb, ub);
        }

        [Test]
        public void TestAdditionBigintAddFGH()
        {
            BigInt a = new BigInt(3);
            FGH b = new FGH(CNFHelper.GetOrdinal(1000), new BigInt(3));

            FGH lb = new FGH(CNFHelper.GetOrdinal(1000), new BigInt(3));
            FGH ub = new FGH(CNFHelper.GetOrdinal(1000), new BigInt(4));

            Test(a, b, lb, ub);
        }

        [Test]
        public void TestAdditionKnuthAddFGH()
        {
            KnuthUpArrow a = new KnuthUpArrow(new BigInt(3), new BigInt(3), 3);
            FGH b = new FGH(CNFHelper.GetOrdinal(1000), new BigInt(3));

            FGH lb = new FGH(CNFHelper.GetOrdinal(1000), new BigInt(3));
            FGH ub = new FGH(CNFHelper.GetOrdinal(1000), new BigInt(4));

            Test(a, b, lb, ub);
        }

        [Test]
        public void TestAdditionTree()
        {
            BigInt bi1 = new BigInt(1);
            BigInt bi2 = new BigInt(2);
            BigInt bi3 = new BigInt(3);
            BigInt bi4 = new BigInt(4);
            BigInt bi5 = new BigInt(5);
            BigInt bi6 = new BigInt(6);

            KnuthUpArrow knuth1 = new KnuthUpArrow(bi5, bi6, 1);
            KnuthUpArrow knuth2 = new KnuthUpArrow(bi4, bi3, 2);

            OperationAddition op1 = new OperationAddition(bi1, bi2);
            OperationAddition op2 = new OperationAddition(knuth1, op1);
            OperationAddition op3 = new OperationAddition(op2, knuth2);

            ResultPair p = op3.Evaluate();
            BigInt res = new BigInt(4);
            res = BigInt.Pow(new BigInt(4), (int)res.N);
            res = BigInt.Pow(new BigInt(4), (int)res.N);

            OperationAddition op4 = new OperationAddition(res, new BigInt(15625));
            res = (BigInt)op4.Evaluate().UpperBound;
            OperationAddition op5 = new OperationAddition(res, new BigInt(3));
            res = (BigInt)op5.Evaluate().UpperBound;

            Assert.IsTrue(res.Equals((BigInt)p.LowerBound));
            Assert.IsTrue(res.Equals((BigInt)p.UpperBound));
        }
    }
}
