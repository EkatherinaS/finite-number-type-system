using System.Numerics;
using CNFConvertions.Number;
using CNFConvertions.Operations;
using CNFConvertions;

namespace CNFTestsNamespace
{
    [TestFixture]
    [Timeout(1000)]
    internal class CNFPowerTests
    {
        private void Test(INumber a, INumber b, INumber lb, INumber ub)
        {
            OperationPower op = new OperationPower(a, b);
            ResultPair p = op.Evaluate();

            Assert.IsTrue(lb.ToString().Equals(p.LowerBound.ToString()));
            Assert.IsTrue(ub.ToString().Equals(p.UpperBound.ToString()));
        }

        [Test]
        public void TestBigIntPowBigInt()
        {
            BigInt a, b, res;

            a = new BigInt(1);
            b = BigInt.GetMax();
            res = new BigInt(1);
            Test(a, b, res, res);

            a = new BigInt(2);
            b = new BigInt(5);
            res = new BigInt(32);
            Test(a, b, res, res);

            a = new BigInt(2);
            b = new BigInt(3318);
            res = new BigInt(BigInteger.Parse("656939859228052112960997764248777927812576207008525909873244982587709297017618619090983619966420963971684011408541379044009622725489344963480679063815893572377186531060053451252081730426500421533898349755146962192666929061228427532929451124448399118812501636658420024106138305282888797325110043985522754027805657150035393088313189461542278474264488576567817603023307586972435593154351499044866898218799087432428450248936931794583609273261606526476735645146370419856559623647106860258631893677311964365962220906534325242502816123083434754310744832481349640869699081289681188746817783780671536413723267990466240141954612684221696743238855827688217175321515613762355407520582962285207743109722350347515800996625812705657018927324241200119073561624632083530980754689722460479976900182389010444071386829696844421175780055775484557807394182156279555427119847179689473778906241556659059967044030280868982598096020548149908866656519079824040068456106459668345146540128789825403751827033374502962142216454144"));
            Test(a, b, res, res);

            a = new BigInt(10);
            b = new BigInt(1000);
            res = BigInt.GetMax();
            Test(a, b, res, res);

            a = BigInt.GetMax();
            b = new BigInt(1);
            res = BigInt.GetMax();
            Test(a, b, res, res);
        }

        [Test]
        public void TestBigIntPowBigIntOverflow()
        {
            BigInt a, b;
            KnuthUpArrow res;

            a = new BigInt(2);
            b = new BigInt(3319);
            res = new KnuthUpArrow(3, 2094, 1);
            Test(a, b, res, res);

            a = new BigInt(11);
            b = new BigInt(1000);
            res = new KnuthUpArrow(11, 1000, 1);
            Test(a, b, res, res);

            a = new BigInt(10);
            b = new BigInt(1001);
            res = new KnuthUpArrow(10, 1001, 1);
            Test(a, b, res, res);

            a = new BigInt(1000);
            b = new BigInt(1000);
            res = new KnuthUpArrow(1000, 1000, 1);
            Test(a, b, res, res);
        }


        [Test]
        public void TestBigintAndFGHPow()
        {
            BigInt a = new BigInt(3);
            FGH b = new FGH(CNFHelper.GetOrdinal(1000), new BigInt(3));

            FGH lb = new FGH(CNFHelper.GetOrdinal(1000), new BigInt(3));
            FGH ub = new FGH(CNFHelper.GetOrdinal(1000), new BigInt(4));

            Test(a, b, lb, ub);
            Test(b, a, lb, ub);
        }


        [Test]
        public void TestKnuthAndFGHPow()
        {
            KnuthUpArrow a = new KnuthUpArrow(new BigInt(3), new BigInt(3), 3);
            FGH b = new FGH(CNFHelper.GetOrdinal(1000), new BigInt(3));

            FGH lb = new FGH(CNFHelper.GetOrdinal(1000), new BigInt(3));
            FGH ub = new FGH(CNFHelper.GetOrdinal(1000), new BigInt(4));

            Test(a, b, lb, ub);
            Test(b, a, lb, ub);
        }


        [Test]
        public void TestFGHPowFGH()
        {
            FGH a = new FGH(CNFHelper.GetOrdinal(4200), new BigInt(7));
            FGH b = new FGH(CNFHelper.GetOrdinal(4201), new BigInt(7));

            FGH lb = new FGH(CNFHelper.GetOrdinal(4201), new BigInt(7));
            FGH ub = new FGH(CNFHelper.GetOrdinal(4201), new BigInt(8));

            Test(a, b, lb, ub);
            Test(b, a, lb, ub);


            a = new FGH(CNFHelper.GetOrdinal(4200), new BigInt(7));
            b = new FGH(CNFHelper.GetOrdinal(4200), new BigInt(8));

            lb = new FGH(CNFHelper.GetOrdinal(4200), new BigInt(8));
            ub = new FGH(CNFHelper.GetOrdinal(4200), new BigInt(9));

            Test(a, b, lb, ub);
            Test(b, a, lb, ub);
        }

        [Test]
        public void TestFGHPowFGHOverflowAlpha()
        {
            FGH a = new FGH(CNFHelper.GetOrdinal(4200), BigInt.GetMax());
            FGH b = new FGH(CNFHelper.GetOrdinal(4200), new BigInt(3));

            FGH lb = new FGH(CNFHelper.GetOrdinal(4200), BigInt.GetMax());
            FGH ub = new FGH(CNFHelper.GetOrdinal(4201), new BigInt(3));

            Test(a, b, lb, ub);
            Test(b, a, lb, ub);
        }


        [Test]
        public void TestBigKnuthPowBigInt()
        {
            KnuthUpArrow a, lb, ub;
            BigInt b;

            a = new KnuthUpArrow(3, 3, 3);
            b = new BigInt(10);

            lb = new KnuthUpArrow(3, 3, 3);
            ub = new KnuthUpArrow(4, 3, 3);
            Test(a, b, lb, ub);
            Test(b, a, lb, ub);

            a = new KnuthUpArrow(BigInt.GetMax(), new BigInt(3), 3);
            b = BigInt.GetMax();

            lb = new KnuthUpArrow(BigInt.GetMax(), new BigInt(3), 3);
            ub = new KnuthUpArrow(3, 4, 3);
            Test(a, b, lb, ub);
            Test(b, a, lb, ub);

            a = new KnuthUpArrow(BigInt.GetMax(), BigInt.GetMax(), 3);
            b = new BigInt(10);

            lb = new KnuthUpArrow(BigInt.GetMax(), BigInt.GetMax(), 3);
            ub = new KnuthUpArrow(3, 3, 4);
            Test(a, b, lb, ub);
            Test(b, a, lb, ub);
        }

        [Test]
        public void TestBigKnuthAndKnuthPow()
        {
            KnuthUpArrow a, b, lb, ub;

            a = new KnuthUpArrow(3, 3, 3);
            b = new KnuthUpArrow(3, 3, 1);

            lb = new KnuthUpArrow(3, 3, 3);
            ub = new KnuthUpArrow(4, 3, 3);
            Test(a, b, lb, ub);
            Test(b, a, lb, ub);

            a = new KnuthUpArrow(3, 3, 3);
            b = new KnuthUpArrow(3, 3, 2);

            lb = new KnuthUpArrow(3, 3, 3);
            ub = new KnuthUpArrow(4, 3, 3);
            Test(a, b, lb, ub);
            Test(b, a, lb, ub);

            a = new KnuthUpArrow(3, 3, 3);
            b = new KnuthUpArrow(3, 3, 3);

            lb = new KnuthUpArrow(3, 3, 3);
            ub = new KnuthUpArrow(3, 4, 3);
            Test(a, b, lb, ub);
            Test(b, a, lb, ub);

            a = new KnuthUpArrow(BigInt.GetMax(), new BigInt(3), 3);
            b = new KnuthUpArrow(3, 3, 3);

            lb = new KnuthUpArrow(BigInt.GetMax(), new BigInt(3), 3);
            ub = new KnuthUpArrow(3, 4, 3);
            Test(a, b, lb, ub);
            Test(b, a, lb, ub);
        }


        [Test]
        public void TestBigIntPowKnuth1Arrow()
        {
            BigInt a;
            KnuthUpArrow b;
            INumber lb, ub;

            a = new BigInt(3);
            b = new KnuthUpArrow(3, 3, 1);
            lb = new BigInt(7625597484987);
            ub = new BigInt(7625597484987);
            Test(a, b, lb, ub);

            a = new BigInt(BigInteger.Pow(10, 800));
            b = new KnuthUpArrow(new BigInt(BigInteger.Pow(10, 350)), new BigInt(3), 1);
            lb = new KnuthUpArrow(403, 3, 2);
            ub = new KnuthUpArrow(414, 3, 2);
            Test(a, b, lb, ub);
        }


        [Test]
        public void TestBigIntPowKnuth2Arrow()
        {
            BigInt a;
            KnuthUpArrow b;
            INumber lb, ub;

            // LB = knuth(b.a, b.b + k, 2)
            // UB = knuth(b.a, b.b + k + 1, 2)

            // a < b.a -> k = 0
            a = new BigInt(42);
            b = new KnuthUpArrow(73, 37, 2);

            lb = new KnuthUpArrow(73, 37 + 0, 2);
            ub = new KnuthUpArrow(73, 37 + 0 + 1, 2);
            Test(a, b, lb, ub);

            // a < b.a ^ b.a -> k = 1
            a = new BigInt(42);
            b = new KnuthUpArrow(7, 37, 2);

            lb = new KnuthUpArrow(7, 37 + 1, 2);
            ub = new KnuthUpArrow(7, 37 + 1 + 1, 2);
            Test(a, b, lb, ub);

            // a < b.a ^^ 3 -> k = 2
            a = new BigInt(7625597484986);
            b = new KnuthUpArrow(3, 37, 2);

            lb = new KnuthUpArrow(3, 37 + 2, 2);
            ub = new KnuthUpArrow(3, 37 + 2 + 1, 2);
            Test(a, b, lb, ub);

            // a < b.a ^^ 4 -> k = 3
            a = new BigInt(7625597484988);
            b = new KnuthUpArrow(3, 37, 2);

            lb = new KnuthUpArrow(3, 37 + 3, 2);
            ub = new KnuthUpArrow(3, 37 + 3 + 1, 2);
            Test(a, b, lb, ub);


            //overflow
            a = new BigInt(42);
            b = new KnuthUpArrow(new BigInt(73), BigInt.GetMax(), 2);

            lb = new KnuthUpArrow(new BigInt(73), BigInt.GetMax(), 2);
            //ub = new KnuthUpArrow(3, BigInt.GetMax() + 0 + 1, 2); - overflow
            ub = new KnuthUpArrow(4, 3, 3);
            Test(a, b, lb, ub);


            a = new BigInt(42);
            b = new KnuthUpArrow(new BigInt(7), BigInt.GetMax(), 2);

            //lb = new KnuthUpArrow(7, BigInt.GetMax() + 1, 2); - overflow
            lb = new KnuthUpArrow(4, 3, 3);
            //ub = new KnuthUpArrow(7, BigInt.GetMax() + 1 + 1, 2); - overflow
            ub = new KnuthUpArrow(5, 3, 3);
            Test(a, b, lb, ub);
        }


        [Test]
        public void TestKnuth1ArrowPowBigInt()
        {
            KnuthUpArrow a;
            BigInt b;

            BigInt xa, xb, y;
            BigInt res;
            OperationPower op;
            ResultPair p;

            //knuth(x.a, x.b * y, 1)
            //x.b * y - bigint -> knuth(x.a, x.b * y, 1)

            xa = new BigInt(42);
            xb = new BigInt(6);
            y = new BigInt(7);
            res = new BigInt(42);
            op = new OperationPower(xa, res);
            p = op.Evaluate();
            a = new KnuthUpArrow(xa, xb, 1);
            b = y;
            Test(a, b, p.LowerBound, p.UpperBound);

            xa = new BigInt(42);
            xb = BigInt.GetMax();
            y = new BigInt(1);
            res = BigInt.GetMax();
            op = new OperationPower(xa, res);
            p = op.Evaluate();
            a = new KnuthUpArrow(xa, xb, 1);
            b = y;
            Test(a, b, p.LowerBound, p.UpperBound);
        }

        [Test]
        public void TestKnuth1ArrowPowBigIntOverflow()
        {
            KnuthUpArrow a;
            BigInt b;

            BigInt xa, xb, y;
            KnuthUpArrow res;
            OperationPower op;
            ResultPair p;

            //knuth(x.a, x.b * y, 1)
            //x.b * y - knuth -> x.a ^ (x.b * y)

            xa = new BigInt(42);
            xb = BigInt.GetMax();
            y = new BigInt(2);
            res = new KnuthUpArrow(10, Constants.EXPONENT + 1, 1);
            op = new OperationPower(xa, res);
            p = op.Evaluate();
            a = new KnuthUpArrow(xa, xb, 1);
            b = y;
            Test(a, b, p.LowerBound, p.UpperBound);
            
            xa = new BigInt(42);
            xb = BigInt.GetMax();
            y = BigInt.GetMax();
            res = new KnuthUpArrow(10, 2 * Constants.EXPONENT + 1, 1);
            op = new OperationPower(xa, res);
            p = op.Evaluate();
            a = new KnuthUpArrow(xa, xb, 1);
            b = y;
            Test(a, b, p.LowerBound, p.UpperBound);
        }


        [Test]
        public void TestKnuth1ArrowPowKnuth()
        {
            KnuthUpArrow a;
            KnuthUpArrow b;
            INumber lb, ub;

            BigInt xa, xb;
            KnuthUpArrow y;
            OperationPower opPow;
            OperationMultiplication opMul;
            ResultPair p1, p2, pMul;

            //x.a^(x.b * y)
            
            xa = new BigInt(42);
            xb = BigInt.GetMax();
            y = new KnuthUpArrow(7, 3, 1);

            opMul = new OperationMultiplication(xb, y);
            pMul = opMul.Evaluate();

            opPow = new OperationPower(xa, pMul.LowerBound);
            p1 = opPow.Evaluate();

            opPow = new OperationPower(xa, pMul.UpperBound);
            p2 = opPow.Evaluate();

            lb = p1.LowerBound < p2.LowerBound ? p1.LowerBound : p2.LowerBound;
            ub = p1.UpperBound > p2.UpperBound ? p1.UpperBound : p2.UpperBound;

            a = new KnuthUpArrow(xa, xb, 1);
            b = y;
            Test(a, b, lb, ub);

            xa = new BigInt(42);
            xb = BigInt.GetMax();
            y = new KnuthUpArrow(3, 7, 2);

            opMul = new OperationMultiplication(xb, y);
            pMul = opMul.Evaluate();

            opPow = new OperationPower(xa, pMul.LowerBound);
            p1 = opPow.Evaluate();

            opPow = new OperationPower(xa, pMul.UpperBound);
            p2 = opPow.Evaluate();

            lb = p1.LowerBound < p2.LowerBound ? p1.LowerBound : p2.LowerBound;
            ub = p1.UpperBound > p2.UpperBound ? p1.UpperBound : p2.UpperBound;

            a = new KnuthUpArrow(xa, xb, 1);
            b = y;
            Test(a, b, lb, ub);
        }


        [Test]
        public void TestKnuth2ArrowPowBigInt()
        {
            KnuthUpArrow a;
            BigInt b;
            INumber lb, ub;

            BigInt xa, xb, y;
            OperationPower opPow;
            OperationMultiplication opMul;
            ResultPair p, pMul;

            //x.a^(knuth(x.a,  x.b, x.n - 1) * y)

            xa = new BigInt(42);
            xb = BigInt.GetMax();
            y = new BigInt(2);
            opMul = new OperationMultiplication(new KnuthUpArrow(xa, xb, 1), y);
            pMul = opMul.Evaluate();

            opPow = new OperationPower(xa, pMul.LowerBound);
            p = opPow.Evaluate();
            lb = p.LowerBound;

            opPow = new OperationPower(xa, pMul.UpperBound);
            p = opPow.Evaluate();
            ub = p.UpperBound;

            a = new KnuthUpArrow(xa, xb, 2);
            b = y;
            Test(a, b, lb, ub);

            xa = new BigInt(42);
            xb = BigInt.GetMax();
            y = BigInt.GetMax();
            opMul = new OperationMultiplication(new KnuthUpArrow(xa, xb, 1), y);
            pMul = opMul.Evaluate();

            opPow = new OperationPower(xa, pMul.LowerBound);
            p = opPow.Evaluate();
            lb = p.LowerBound;

            opPow = new OperationPower(xa, pMul.UpperBound);
            p = opPow.Evaluate();
            ub = p.UpperBound;

            a = new KnuthUpArrow(xa, xb, 2);
            b = y;
            Test(a, b, lb, ub);
        }


        [Test]
        public void TestKnuth2ArrowPowKnuth()
        {
            KnuthUpArrow a;
            KnuthUpArrow b;
            INumber lb, ub;

            BigInt xa, xb;
            KnuthUpArrow y;
            OperationPower opPow;
            OperationMultiplication opMul;
            ResultPair p, pMul;

            //x.a^(knuth(x.a,  x.b, x.n - 1) * y)

            xa = new BigInt(42);
            xb = BigInt.GetMax();
            y = new KnuthUpArrow(7, 3, 1);

            opMul = new OperationMultiplication(new KnuthUpArrow(xa, xb, 1), y);
            pMul = opMul.Evaluate();

            opPow = new OperationPower(xa, pMul.LowerBound);
            p = opPow.Evaluate();
            lb = p.LowerBound;

            opPow = new OperationPower(xa, pMul.UpperBound);
            p = opPow.Evaluate();
            ub = p.UpperBound;

            a = new KnuthUpArrow(xa, xb, 2);
            b = y;
            Test(a, b, lb, ub);


            xa = new BigInt(42);
            xb = BigInt.GetMax();
            y = new KnuthUpArrow(3, 7, 2);

            opMul = new OperationMultiplication(new KnuthUpArrow(xa, xb, 1), y);
            pMul = opMul.Evaluate();

            opPow = new OperationPower(xa, pMul.LowerBound);
            p = opPow.Evaluate();
            lb = p.LowerBound;

            opPow = new OperationPower(xa, pMul.UpperBound);
            p = opPow.Evaluate();
            ub = p.UpperBound;

            a = new KnuthUpArrow(xa, xb, 2);
            b = y;
            Test(a, b, lb, ub);
        }
    }
}
