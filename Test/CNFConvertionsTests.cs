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

        // Basic tests for numbers

        [Test]
        public void TestBigIntNumber()
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
        public void TestKnuthNumber()
        {
            var arrow = new KnuthUpArrow(3, 4, 1);
            Assert.That(arrow.A, Is.EqualTo(new BigInt(3)));
            Assert.That(arrow.B, Is.EqualTo(new BigInt(4)));
            Assert.That(arrow.N, Is.EqualTo(1));

            var arrow1 = new KnuthUpArrow(3, 4, 1);
            var arrow2 = new KnuthUpArrow(3, 4, 2);
            var arrow3 = new KnuthUpArrow(3, 4, 3);

            Assert.That(arrow1.ToString(), Is.EqualTo("3↑4"));
            Assert.That(arrow2.ToString(), Is.EqualTo("3↑↑4"));
            Assert.That(arrow3.ToString(), Is.EqualTo("3↑↑↑4"));

            Assert.That(arrow1.ToLaTeX(), Is.EqualTo("3 \\uparrow 4"));
            Assert.That(arrow2.ToLaTeX(), Is.EqualTo("3 \\uparrow \\uparrow 4"));
            Assert.That(arrow3.ToLaTeX(), Is.EqualTo("3 \\uparrow \\uparrow \\uparrow 4"));

            var min = KnuthUpArrow.GetMin();
            Assert.That(min.A, Is.EqualTo(new BigInt(3)));
            Assert.That(min.B, Is.EqualTo(new BigInt(3)));
            Assert.That(min.N, Is.EqualTo(1));

            var max = KnuthUpArrow.GetMax();
            Assert.That(max.A, Is.EqualTo(BigInt.GetMax()));
            Assert.That(max.B, Is.EqualTo(BigInt.GetMax()));
            Assert.That(max.N, Is.EqualTo(Constants.ARROW_COUNT));


            arrow1 = new KnuthUpArrow(3, 3, 1);
            var result1 = arrow1.ToBigInt();
            Assert.IsNotNull(result1);
            Assert.That(result1, Is.EqualTo(new BigInt(27)));

            arrow2 = new KnuthUpArrow(3, 3, 2);
            var result2 = arrow2.ToBigInt();
            Assert.IsNotNull(result2);
            Assert.That(result2, Is.EqualTo(new BigInt(7625597484987)));


            arrow1 = new KnuthUpArrow(10, 10, 2);
            result1 = arrow1.ToBigInt();
            Assert.IsNull(result1);

            arrow2 = new KnuthUpArrow(3, 2096, 1);
            result2 = arrow2.ToBigInt();
            Assert.IsNull(result2);
        }

        [Test]
        public void TestFGHNumber()
        {
            //TODO: Test FGH
        }

        // Tests for Succ method

        [Test]
        public void TestBigIntSucc()
        {
            //TODO: Test FGH
        }

        [Test]
        public void TestKnuthSucc()
        {
            var arrow1 = new KnuthUpArrow(4, 5, 1);
            var succ1 = arrow1.Succ();
            Assert.That(succ1.ToString(), Is.EqualTo("5↑5"));

            var arrow2 = new KnuthUpArrow(BigInt.GetMax(), new BigInt(3), 2);
            var succ2 = arrow2.Succ();
            Assert.That(succ2.ToString(), Is.EqualTo(BigInt.GetMax().ToString() + "↑↑4"));

            var arrow3 = new KnuthUpArrow(BigInt.GetMax(), new BigInt(3), 3);
            var succ3 = arrow3.Succ();
            Assert.That(succ3.ToString(), Is.EqualTo("3↑↑↑4"));

            var arrow4 = new KnuthUpArrow(BigInt.GetMax(), BigInt.GetMax(), 1);
            var succ4 = arrow4.Succ();
            Assert.That(succ4.ToString(), Is.EqualTo("3↑↑5"));

            var arrow5 = new KnuthUpArrow(BigInt.GetMax(), BigInt.GetMax(), Constants.ARROW_COUNT);
            var succ5 = arrow5.Succ();
            Assert.IsInstanceOf<FGH>(succ5);
        }

        [Test]
        public void TestFGHSucc()
        {
            //TODO: Test FGH
        }


        [Test]
        public void TestCompareBigIntToBigInt()
        {
            //TODO: Test Compare BigInt to BigInt
        }

        [Test]
        public void TestCompareBigIntToKnuth()
        {
            //TODO: Test Compare BigInt to Knuth
        }

        [Test]
        public void TestCompareBigIntToFGH()
        {
            //TODO: Test Compare BigInt to FGH
        }

        [Test]
        public void TestCompareKnuthToKnuth()
        {
            //TODO: Test Compare Knuth to Knuth
        }

        [Test]
        public void TestCompareKnuthToFGH()
        {
            //TODO: Test Compare Knuth to FGH
        }

        [Test]
        public void TestCompareFGHToFGH()
        {
            //TODO: Test Compare FGH to FGH
        }

        [Test]
        public void TestConvertBigIntToKnuth()
        {
            //TODO: Test Convert BigInt To Knuth
        }

        [Test]
        public void TestConvertKnuthToKnuthPlusOne()
        {
            //TODO: Test Convert Knuth To Knuth Plus One
        }

        [Test]
        public void TestConvertKnuthToFGH()
        {
            //TODO: Test Convert Knuth To FGH
        }

        [Test]
        public void TestConvertFGHToFGHPlusOne()
        {
            //TODO: Test Convert FGH To FGH Plus One
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

            KnuthUpArrow res = new KnuthUpArrow(10, Constants.EXPONENT + 1, 1);
            Assert.IsTrue(res.Equals((KnuthUpArrow)p.LowerBound));
            Assert.IsTrue(res.Equals((KnuthUpArrow)p.UpperBound));
        }

        [Test]
        public void TestAdditionKnuthPlusKnuth()
        {
            BigInt v1 = new BigInt(4);
            BigInt v2 = new BigInt(7);
            BigInt v3 = new BigInt(5);

            KnuthUpArrow a = new KnuthUpArrow(v1, v2, 4);
            KnuthUpArrow b = new KnuthUpArrow(v2, v1, 4);
            KnuthUpArrow c = new KnuthUpArrow(v3, v2, 4);

            OperationAddition op = new OperationAddition(a, b);
            ResultPair p = op.Evaluate();

            Assert.That(p.LowerBound.ToString(), Is.EqualTo(a.ToString()));
            Assert.That(p.UpperBound.ToString(), Is.EqualTo(c.ToString()));
        }

        [Test]
        public void TestAdditionFGHPlusFGH()
        {
            CNFOrdinal ord1 = GetOrdinal(4200);
            CNFOrdinal ord2 = GetOrdinal(4201);

            BigInt v1 = new BigInt(7);
            BigInt v2 = new BigInt(8);
            BigInt v3 = new BigInt(9);

            FGH fgh1 = new FGH(ord1, v1);
            FGH fgh2 = new FGH(ord2, v1);
            FGH fgh3 = new FGH(ord1, v2);

            //fgh1 + fgh2
            FGH lowerBound = fgh2;
            FGH upperBound = new FGH(fgh2.Alpha, v2);

            OperationAddition op = new OperationAddition(fgh1, fgh2);
            ResultPair p = op.Evaluate();

            Assert.IsTrue(lowerBound.Equals((FGH)p.LowerBound));
            Assert.IsTrue(upperBound.Equals((FGH)p.UpperBound));

            //fgh1 + fgh3
            lowerBound = fgh3;
            upperBound = new FGH(fgh3.Alpha, v3);

            op = new OperationAddition(fgh1, fgh3);
            p = op.Evaluate();

            Assert.IsTrue(lowerBound.Equals((FGH)p.LowerBound));
            Assert.IsTrue(upperBound.Equals((FGH)p.UpperBound));
        }

        [Test]
        public void TestAdditionBigintPlusSmallKnuth()
        {
            KnuthUpArrow a = new KnuthUpArrow(3, 3, 1);
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
            BigInt bi3 = new BigInt(3);
            BigInt bi4 = new BigInt(4);

            KnuthUpArrow b = new KnuthUpArrow(bi3, bi3, 3);
            KnuthUpArrow c = new KnuthUpArrow(bi4, bi3, 3);

            OperationAddition op1 = new OperationAddition(bi3, b);
            ResultPair p1 = op1.Evaluate();
            Assert.That(p1.LowerBound.ToString(), Is.EqualTo(b.ToString()));
            Assert.That(p1.UpperBound.ToString(), Is.EqualTo(c.ToString()));

            OperationAddition op2 = new OperationAddition(b, bi3);
            ResultPair p2 = op2.Evaluate();
            Assert.That(p2.LowerBound.ToString(), Is.EqualTo(b.ToString()));
            Assert.That(p2.UpperBound.ToString(), Is.EqualTo(c.ToString()));
        }

        [Test]
        public void TestAdditionBigintPlusFGH()
        {
            BigInt bi3 = new BigInt(3);
            BigInt bi4 = new BigInt(4);
            FGH b = new FGH(GetOrdinal(1000), bi3);
            FGH c = new FGH(GetOrdinal(1000), bi4);

            OperationAddition op1 = new OperationAddition(bi3, b);
            ResultPair p1 = op1.Evaluate();
            Assert.That(p1.LowerBound.ToString(), Is.EqualTo(b.ToString()));
            Assert.That(p1.UpperBound.ToString(), Is.EqualTo(c.ToString()));

            OperationAddition op2 = new OperationAddition(b, bi3);
            ResultPair p2 = op2.Evaluate();
            Assert.That(p2.LowerBound.ToString(), Is.EqualTo(b.ToString()));
            Assert.That(p2.UpperBound.ToString(), Is.EqualTo(c.ToString()));
        }

        [Test]
        public void TestAdditionKnuthPlusFGH()
        {
            BigInt bi3 = new BigInt(3);
            BigInt bi4 = new BigInt(4);
            FGH b = new FGH(GetOrdinal(1000), bi3);
            FGH c = new FGH(GetOrdinal(1000), bi4);

            OperationAddition op1 = new OperationAddition(bi3, b);
            ResultPair p1 = op1.Evaluate();
            Assert.That(p1.LowerBound.ToString(), Is.EqualTo(b.ToString()));
            Assert.That(p1.UpperBound.ToString(), Is.EqualTo(c.ToString()));

            OperationAddition op2 = new OperationAddition(b, bi3);
            ResultPair p2 = op2.Evaluate();
            Assert.That(p2.LowerBound.ToString(), Is.EqualTo(b.ToString()));
            Assert.That(p2.UpperBound.ToString(), Is.EqualTo(c.ToString()));
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

        [Test]
        public void TestCountDigits()
        {
            Assert.That(INumber.CountDigits(BigInteger.Zero), Is.EqualTo(1));
            for (int i = 1; i < 10; i++)
                Assert.That(INumber.CountDigits(new BigInteger(i)), Is.EqualTo(1));
            for (int i = 1; i <= 20; i++)
            {
                BigInteger power = BigInteger.Pow(10, i);
                Assert.That(INumber.CountDigits(power), Is.EqualTo(i + 1), $"Failed at 10^{i}");
            }

            var testCases = new Dictionary<string, int>
            {
                ["99"] = 2,
                ["100"] = 3,
                ["123456789"] = 9,
                ["12345678901234567890"] = 20,
                ["99999999999999999999"] = 20,
                ["100000000000000000000"] = 21
            };

            foreach (var kv in testCases)
            {
                BigInteger value = BigInteger.Parse(kv.Key);
                int expectedDigits = kv.Value;
                Assert.That(INumber.CountDigits(value), Is.EqualTo(expectedDigits), $"Failed at {kv.Key}");
            }
        }

        [Test]
        public void TestCountDigitsPow()
        {
            Assert.That(INumber.CountDigitsPow(0, 10), Is.EqualTo(1));
            Assert.That(INumber.CountDigitsPow(10, 0), Is.EqualTo(1));

            // 10^2 = 100 → 3 digits
            Assert.That(INumber.CountDigitsPow(10, 2), Is.EqualTo(3));
            // 2^10 = 1024 → 4 digits
            Assert.That(INumber.CountDigitsPow(2, 10), Is.EqualTo(4));
            // 3^5 = 243 → 3 digits
            Assert.That(INumber.CountDigitsPow(3, 5), Is.EqualTo(3));

            // 10^100 has 101 digits
            BigInteger bigBase = BigInteger.Pow(10, 100);
            Assert.That(INumber.CountDigitsPow(bigBase, 1), Is.EqualTo(101));
            // (10^100)^1 = 10^100 → 101 digits
            Assert.That(INumber.CountDigitsPow(bigBase, 1), Is.EqualTo(101));
            // (10^100)^2 = 10^200 → 201 digits
            Assert.That(INumber.CountDigitsPow(bigBase, 2), Is.EqualTo(201));

            // 2^100 ≈ 1.26e30 → 31 digits
            Assert.That(INumber.CountDigitsPow(2, 100), Is.EqualTo(31));
            // 3^100 ≈ 5.15e47 → 48 digits
            Assert.That(INumber.CountDigitsPow(3, 100), Is.EqualTo(48));

            // Test with numbers having more than 15 digits (scale limit)
            BigInteger veryLargeBase = BigInteger.Parse("123456789012345678901234567890");

            // For b=1, digits should equal the base's digit count
            int baseDigits = veryLargeBase.ToString().Length;
            Assert.That(INumber.CountDigitsPow(veryLargeBase, 1), Is.EqualTo(baseDigits));

            // 10^10^10 has 10^10+1 digits (but we can't compute this directly)
            // Instead test reasonable large exponents
            BigInteger baseNum = 10;
            BigInteger exponent = 1000;
            int expectedDigits = 1001; // 10^1000 has 1001 digits
            Assert.That(INumber.CountDigitsPow(baseNum, exponent), Is.EqualTo(expectedDigits));

            // 9^1000 has floor(1000*log10(9))+1 digits
            baseNum = 9;
            exponent = 1000;
            expectedDigits = (int)Math.Floor(1000 * Math.Log10(9)) + 1;
            Assert.That(INumber.CountDigitsPow(baseNum, exponent), Is.EqualTo(expectedDigits));
        }
    }
}