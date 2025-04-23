using Assets.Scripts.AssemblyMath;
using CNFConvertions;
using CNFConvertions.Number;
using CNFConvertions.Operations;
using System.Numerics;

namespace Test
{
    [TestFixture]
    [Timeout(1000)]
    internal class CNFConvertionsTests
    {
        private CNFOrdinal GetOrdinal(int n) => new CNFOrdinal(
            new List<CNFOrdinalTerm>
            {
                new CNFOrdinalTerm(CNFOrdinal.Zero, n)
            }
        );

        [Test]
        public void TestBigIntBasic()
        {
            BigInt bigInt;

            bigInt = new BigInt(1);
            Assert.That(bigInt.ToString(), Is.EqualTo("1"));

            bigInt = new BigInt(100000);
            Assert.That(bigInt.ToString(), Is.EqualTo("100000"));

            string max = "1";
            string min = "1";
            for (int i = 0; i < 1000; i++)
            {
                max += "0";
            }

            Assert.That(BigInt.GetMin().ToString(), Is.EqualTo(min));
            Assert.That(BigInt.GetMax().ToString(), Is.EqualTo(max));
            Assert.That(BigInt.GetMin().ToLaTeX(), Is.EqualTo(min));
            Assert.That(BigInt.GetMax().ToLaTeX(), Is.EqualTo(max));
        }


        [Test]
        public void TestAdditionBigintPlusBigintSimple()
        {
            BigInt a = new BigInt(1);
            BigInt b = new BigInt(2);
            BigInt c = new BigInt(3);
            BigInt d = new BigInt(4);
            BigInt e = new BigInt(7);

            OperationAddition op1 = new OperationAddition(a, b);
            ResultPair p1 = op1.Evaluate();

            Assert.That(p1.LowerBound, Is.EqualTo(c));
            Assert.That(p1.UpperBound, Is.EqualTo(c));

            OperationAddition op2 = new OperationAddition(a, c);
            ResultPair p2 = op2.Evaluate();

            Assert.That(p2.LowerBound, Is.EqualTo(d));
            Assert.That(p2.UpperBound, Is.EqualTo(d));

            OperationAddition op = new OperationAddition(op1, op2);
            ResultPair p = op.Evaluate();

            Assert.That(p.LowerBound, Is.EqualTo(e));
            Assert.That(p.UpperBound, Is.EqualTo(e));
        }


        [Test]
        public void TestAdditionBigintPlusBigintOverflow()
        {
            BigInt a = BigInt.GetMax();
            BigInt b = BigInt.GetMax();

            OperationAddition op = new OperationAddition(a, b);
            ResultPair p = op.Evaluate();

            KnuthUpArrow res = new KnuthUpArrow(new BigInt(10), new BigInt(Constants.EXPONENT + 1), 1);
            Assert.IsTrue(res.Equals((KnuthUpArrow)p.LowerBound));
            Assert.IsTrue(res.Equals((KnuthUpArrow)p.UpperBound));
        }

        [Test]
        public void TestAdditionKnuthPlusKnuth()
        {
            BigInt v1 = new BigInt(4);
            BigInt v2 = new BigInt(7);

            KnuthUpArrow a = new KnuthUpArrow(v1, v2, 4);
            KnuthUpArrow b = new KnuthUpArrow(v2, v1, 4);

            KnuthUpArrow lowerBound = b;
            KnuthUpArrow upperBound = new KnuthUpArrow(v2 + 1, v1, 4);

            OperationAddition op = new OperationAddition(a, b);
            ResultPair p = op.Evaluate();

            Assert.IsTrue(lowerBound.Equals((KnuthUpArrow)p.LowerBound));
            Assert.IsTrue(upperBound.Equals((KnuthUpArrow)p.UpperBound));
        }

        [Test]
        public void TestAdditionFGHPlusFGH()
        {
            CNFOrdinal ord1 = GetOrdinal(4200);
            CNFOrdinal ord2 = GetOrdinal(4201);

            BigInt v1 = new BigInt(7);
            BigInt v2 = new BigInt(8);

            FGH fgh1 = new FGH(ord1, v1);
            FGH fgh2 = new FGH(ord2, v1);
            FGH fgh3 = new FGH(ord1, v2);

            //fgh1 + fgh2
            FGH lowerBound = fgh2;
            FGH upperBound = new FGH(fgh2.Alpha, fgh2.N + 1);

            OperationAddition op = new OperationAddition(fgh1, fgh2);
            ResultPair p = op.Evaluate();

            Assert.IsTrue(lowerBound.Equals((FGH)p.LowerBound));
            Assert.IsTrue(upperBound.Equals((FGH)p.UpperBound));

            //fgh1 + fgh3
            lowerBound = fgh3;
            upperBound = new FGH(fgh3.Alpha, fgh3.N + 1);

            op = new OperationAddition(fgh1, fgh3);
            p = op.Evaluate();

            Assert.IsTrue(lowerBound.Equals((FGH)p.LowerBound));
            Assert.IsTrue(upperBound.Equals((FGH)p.UpperBound));
        }

        [Test]
        public void TestAdditionBigintPlusSmallKnuth()
        {
            KnuthUpArrow a = new KnuthUpArrow(new BigInt(3), new BigInt(3), 1);
            BigInt b = new BigInt(3);

            OperationAddition op = new OperationAddition(a, b);
            ResultPair p = op.Evaluate();

            BigInt res = new BigInt(30);
            Assert.IsTrue(res.Equals((BigInt)p.LowerBound));
            Assert.IsTrue(res.Equals((BigInt)p.UpperBound));
        }

        [Test]
        public void TestAdditionBigintPlusBigKnuth()
        {
            BigInt a = new BigInt(3);
            KnuthUpArrow b = new KnuthUpArrow(a, a, 3);

            OperationAddition op1 = new OperationAddition(a, b);
            ResultPair p1 = op1.Evaluate();
            Assert.That(p1.LowerBound, Is.EqualTo(b));
            Assert.That(p1.UpperBound, Is.EqualTo(b));

            OperationAddition op2 = new OperationAddition(b, a);
            ResultPair p2 = op2.Evaluate();
            Assert.That(p2.LowerBound, Is.EqualTo(b));
            Assert.That(p2.UpperBound, Is.EqualTo(b));
        }

        [Test]
        public void TestAdditionBigintPlusFGH()
        {
            BigInt a = new BigInt(3);
            FGH b = new FGH(GetOrdinal(1000), a);

            OperationAddition op1 = new OperationAddition(a, b);
            ResultPair p1 = op1.Evaluate();
            Assert.That(p1.LowerBound, Is.EqualTo(b));
            Assert.That(p1.UpperBound, Is.EqualTo(b));

            OperationAddition op2 = new OperationAddition(b, a);
            ResultPair p2 = op2.Evaluate();
            Assert.That(p2.LowerBound, Is.EqualTo(b));
            Assert.That(p2.UpperBound, Is.EqualTo(b));
        }

        [Test]
        public void TestAdditionKnuthPlusFGH()
        {
            BigInt a = new BigInt(3);
            FGH b = new FGH(GetOrdinal(1000), a);

            OperationAddition op1 = new OperationAddition(a, b);
            ResultPair p1 = op1.Evaluate();
            Assert.That(p1.LowerBound, Is.EqualTo(b));
            Assert.That(p1.UpperBound, Is.EqualTo(b));

            OperationAddition op2 = new OperationAddition(b, a);
            ResultPair p2 = op2.Evaluate();
            Assert.That(p2.LowerBound, Is.EqualTo(b));
            Assert.That(p2.UpperBound, Is.EqualTo(b));
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
            res += 15625;
            res += 3;

            Assert.IsTrue(res.Equals((BigInt)p.LowerBound));
            Assert.IsTrue(res.Equals((BigInt)p.UpperBound));
        }
    }
}
