using CNFConvertions;
using CNFConvertions.Number;
using System.Numerics;

namespace CNFTestsNamespace
{
    [TestFixture]
    [Timeout(1000)]
    internal class CNFNumberTests
    {

        [Test]
        public void TestBigIntNumber()
        {
            BigInt bigInt;

            bigInt = new BigInt(1);
            Assert.IsTrue(bigInt.ToString().Equals("1"));
            Assert.IsTrue(bigInt.ToLaTeX().Equals("1"));

            bigInt = new BigInt(100000);
            Assert.IsTrue(bigInt.ToString().Equals("100000"));
            Assert.IsTrue(bigInt.ToLaTeX().Equals("100000"));

            string max = "1";
            string min = "1";
            for (int i = 0; i < 1000; i++)
            {
                max += "0";
            }

            Assert.IsTrue(BigInt.GetMin().ToString().Equals(min));
            Assert.IsTrue(BigInt.GetMax().ToString().Equals(max));
            Assert.IsTrue(BigInt.GetMin().ToLaTeX().Equals(min));
            Assert.IsTrue(BigInt.GetMax().ToLaTeX().Equals(max));

            Assert.Throws<ArgumentException>(() => new BigInt(0));
            Assert.Throws<ArgumentException>(() => new BigInt(BigInteger.Pow(10, Constants.EXPONENT) + 1));
        }

        [Test]
        public void TestKnuthNumber()
        {
            var arrow = new KnuthUpArrow(3, 4, 1);
            Assert.IsTrue(arrow.A.Equals(new BigInt(3)));
            Assert.IsTrue(arrow.B.Equals(new BigInt(4)));
            Assert.IsTrue(arrow.N.Equals(1));

            var arrow1 = new KnuthUpArrow(3, 4, 1);
            var arrow2 = new KnuthUpArrow(3, 3, 2);
            var arrow3 = new KnuthUpArrow(3, 4, 3);

            Assert.IsTrue(arrow1.ToString().Equals("3↑4"));
            Assert.IsTrue(arrow2.ToString().Equals("3↑↑3"));
            Assert.IsTrue(arrow3.ToString().Equals("3↑↑↑4"));

            Assert.IsTrue(arrow1.ToLaTeX().Equals("3 \\uparrow 4"));
            Assert.IsTrue(arrow2.ToLaTeX().Equals("3 \\uparrow \\uparrow 3"));
            Assert.IsTrue(arrow3.ToLaTeX().Equals("3 \\uparrow \\uparrow \\uparrow 4"));

            var min = KnuthUpArrow.GetMin();
            Assert.IsTrue(min.A.Equals(new BigInt(3)));
            Assert.IsTrue(min.B.Equals(new BigInt(3)));
            Assert.IsTrue(min.N.Equals(1));

            var max = KnuthUpArrow.GetMax();
            Assert.IsTrue(max.A.Equals(BigInt.GetMax()));
            Assert.IsTrue(max.B.Equals(BigInt.GetMax()));
            Assert.IsTrue(max.N.Equals(Constants.ARROW_COUNT));

            var result1 = arrow1.ToBigInt();
            Assert.IsTrue(result1.Equals(new BigInt(81)));

            var result2 = arrow2.ToBigInt();
            Assert.IsTrue(result2.Equals(new BigInt(7625597484987)));

            var tooBigArrow1 = new KnuthUpArrow(10, 10, 2);
            result1 = tooBigArrow1.ToBigInt();
            Assert.IsNull(result1);

            var tooBigArrow2 = new KnuthUpArrow(3, 2096, 1);
            result2 = tooBigArrow2.ToBigInt();
            Assert.IsNull(result2);

            Assert.Throws<ArgumentException>(() => new KnuthUpArrow(new BigInt(2), new BigInt(3), 1));
            Assert.Throws<ArgumentException>(() => new KnuthUpArrow(new BigInt(3), new BigInt(2), 1));
            Assert.Throws<ArgumentException>(() => new KnuthUpArrow(new BigInt(3), new BigInt(3), 0));
            Assert.Throws<ArgumentException>(() => new KnuthUpArrow(new BigInt(3), new BigInt(3), Constants.ARROW_COUNT + 1));
        }

        [Test]
        public void TestFGHNumber()
        {
            FGH fgh = new FGH(CNFHelper.GetOrdinal(1000), new BigInt(42));
            Assert.IsTrue(fgh.ToString().Equals("f(1000, 42)"));
            Assert.IsTrue(fgh.ToLaTeX().Equals("f_{1000}(42)"));

            Assert.Throws<ArgumentException>(() => new FGH(CNFHelper.GetOrdinal(999), new BigInt(3)));
            Assert.Throws<ArgumentException>(() => new FGH(CNFHelper.GetOrdinal(1000), new BigInt(2)));
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
            Assert.IsTrue(INumber.CountDigitsPow(0, 10).Equals(1));
            Assert.IsTrue(INumber.CountDigitsPow(10, 0).Equals(1));

            // 10^2 = 100 → 3 digits
            Assert.IsTrue(INumber.CountDigitsPow(10, 2).Equals(3));
            // 2^10 = 1024 → 4 digits
            Assert.IsTrue(INumber.CountDigitsPow(2, 10).Equals(4));
            // 3^5 = 243 → 3 digits
            Assert.IsTrue(INumber.CountDigitsPow(3, 5).Equals(3));

            // 10^100 has 101 digits
            BigInteger bigBase = BigInteger.Pow(10, 100);
            Assert.IsTrue(INumber.CountDigitsPow(bigBase, 1).Equals(101));
            // (10^100)^1 = 10^100 → 101 digits
            Assert.IsTrue(INumber.CountDigitsPow(bigBase, 1).Equals(101));
            // (10^100)^2 = 10^200 → 201 digits
            Assert.IsTrue(INumber.CountDigitsPow(bigBase, 2).Equals(201));

            // 2^100 ≈ 1.26e30 → 31 digits
            Assert.IsTrue(INumber.CountDigitsPow(2, 100).Equals(31));
            // 3^100 ≈ 5.15e47 → 48 digits
            Assert.IsTrue(INumber.CountDigitsPow(3, 100).Equals(48));

            // Test with numbers having more than 15 digits (scale limit)
            BigInteger veryLargeBase = BigInteger.Parse("123456789012345678901234567890");

            // For b=1, digits should equal the base's digit count
            int baseDigits = veryLargeBase.ToString().Length;
            Assert.IsTrue(INumber.CountDigitsPow(veryLargeBase, 1).Equals(baseDigits));

            // 10^10^10 has 10^10+1 digits (but we can't compute this directly)
            // Instead test reasonable large exponents
            BigInteger baseNum = 10;
            BigInteger exponent = 1000;
            int expectedDigits = 1001; // 10^1000 has 1001 digits
            Assert.IsTrue(INumber.CountDigitsPow(baseNum, exponent).Equals(expectedDigits));

            // 9^1000 has floor(1000*log10(9))+1 digits
            baseNum = 9;
            exponent = 1000;
            expectedDigits = (int)Math.Floor(1000 * Math.Log10(9)) + 1;
            Assert.IsTrue(INumber.CountDigitsPow(baseNum, exponent).Equals(expectedDigits));
        }
    }
}
