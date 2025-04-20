using Assets.Scripts.AssemblyMath;
using CNFConvertions;
using CNFConvertions.Number;
using CNFConvertions.Operations;

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
            //TODO: test overflow bigint to knuth
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
            Assert.That(p.LowerBound, Is.EqualTo(lowerBound));

            //TODO: Fix the Knuth's comparator0
            //Assert.That(p.UpperBound, Is.EqualTo(upperBound)); - fails with: Expected: <8↑↑↑↑4> But was:  < 8↑↑↑↑4 >
            Assert.That(((KnuthUpArrow)p.UpperBound).A, Is.EqualTo(upperBound.A));
            Assert.That(((KnuthUpArrow)p.UpperBound).B, Is.EqualTo(upperBound.B));
            Assert.That(((KnuthUpArrow)p.UpperBound).N, Is.EqualTo(upperBound.N));
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
            Assert.That(p.LowerBound, Is.EqualTo(lowerBound));

            //TODO: Fix the FGH's comparator0
            Assert.That(((FGH)p.UpperBound).Alpha, Is.EqualTo(upperBound.Alpha));
            Assert.That(((FGH)p.UpperBound).N, Is.EqualTo(upperBound.N));


            //fgh1 + fgh3
            lowerBound = fgh3;
            upperBound = new FGH(fgh3.Alpha, fgh3.N + 1);

            op = new OperationAddition(fgh1, fgh3);
            p = op.Evaluate();
            Assert.That(p.LowerBound, Is.EqualTo(lowerBound));

            //TODO: Fix the FGH's comparator0
            Assert.That(((FGH)p.UpperBound).Alpha, Is.EqualTo(upperBound.Alpha));
            Assert.That(((FGH)p.UpperBound).N, Is.EqualTo(upperBound.N));
        }

        [Test]
        public void TestAdditionBigintPlusSmallKnuth()
        {
            //TODO: test bigint plus small knuth
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
            //TODO: test addition tree
        }
    }
}
