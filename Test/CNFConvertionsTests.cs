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

        private CNFOrdinal getOrdinal(int n) => new CNFOrdinal(
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
            //TODO: sum knuths
        }

        [Test]
        public void TestAdditionFGHPlusFGH()
        {
            //TODO: sum FGHs
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
            FGH b = new FGH(getOrdinal(1000), a);

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
            FGH b = new FGH(getOrdinal(1000), a);

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
