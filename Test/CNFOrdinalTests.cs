using NUnit.Framework;
using System;
using System.Numerics;
using System.Collections.Generic;
using Assets.Scripts.AssemblyMath;

namespace OrdinalTestsNamespace
{
    [TestFixture]
    [Timeout(1000)]
    public class CNFOrdinalCombinedTests
    {
        /// <summary>
        /// Constructs the finite ordinal n (i.e. n = ω^0 * n).
        /// </summary>
        private CNFOrdinal Finite(int n)
        {
            return new CNFOrdinal(new List<CNFOrdinalTerm>
            {
                new CNFOrdinalTerm(CNFOrdinal.Zero, n)
            });
        }

        /// <summary>
        /// Constructs the ordinal ω^n (for a finite exponent n).
        /// For example, OmegaPower(3) represents ω^3.
        /// </summary>
        private CNFOrdinal OmegaPower(int n)
        {
            return new CNFOrdinal(new List<CNFOrdinalTerm>
            {
                new CNFOrdinalTerm(Finite(n), 1)
            });
        }

        [Test]
        public void TestAdditionFinite()
        {
            // 3 + 4 = 7
            CNFOrdinal three = Finite(3);
            CNFOrdinal four = Finite(4);
            CNFOrdinal expected = Finite(7);
            CNFOrdinal result = three + four;
            Assert.AreEqual(expected.ToString(), result.ToString(), "3 + 4 should equal 7.");
        }

        [Test]
        public void TestAdditionOmegaPlusFinite()
        {
            // ω + 5 becomes ω + 5.
            CNFOrdinal omega = CNFOrdinal.Omega;
            CNFOrdinal five = Finite(5);
            CNFOrdinal result = omega + five;
            Assert.AreEqual("ω + 5", result.ToString(), "ω + 5 should equal ω + 5.");
        }

        [Test]
        public void TestAdditionFinitePlusOmega()
        {
            // 5 + ω becomes ω.
            CNFOrdinal five = Finite(5);
            CNFOrdinal omega = CNFOrdinal.Omega;
            CNFOrdinal result = five + omega;
            Assert.AreEqual("ω", result.ToString(), "5 + ω should equal ω.");
        }

        [Test]
        public void TestAdditionOmegaPlusOmega()
        {
            // ω + ω = ω*2.
            CNFOrdinal omega = CNFOrdinal.Omega;
            CNFOrdinal result = omega + omega;
            Assert.AreEqual("ω*2", result.ToString(), "ω + ω should equal ω*2.");
        }

        [Test]
        public void TestAdditionComplex()
        {
            // (ω + 3) + (ω + 2) becomes ω*2 + 2.
            CNFOrdinal omega = CNFOrdinal.Omega;
            CNFOrdinal result = (omega + Finite(3)) + (omega + Finite(2));
            Assert.AreEqual("ω*2 + 2", result.ToString(), "(ω + 3) + (ω + 2) should equal ω*2 + 2.");
        }

        [Test]
        public void TestAdditionComplexReversed()
        {
            // (ω + 2) + (ω + 3) becomes ω*2 + 3.
            CNFOrdinal omega = CNFOrdinal.Omega;
            CNFOrdinal result = (omega + Finite(2)) + (omega + Finite(3));
            Assert.AreEqual("ω*2 + 3", result.ToString(), "(ω + 2) + (ω + 3) should equal ω*2 + 3.");
        }

        [Test]
        public void TestMultiplicationFiniteTimesOmega()
        {
            // 5 * ω becomes ω.
            CNFOrdinal five = Finite(5);
            CNFOrdinal omega = CNFOrdinal.Omega;
            CNFOrdinal result = five * omega;
            Assert.AreEqual("ω", result.ToString(), "5 * ω should equal ω.");
        }

        [Test]
        public void TestMultiplicationOmegaTimesFinite()
        {
            // ω * 5 yields ω*5.
            CNFOrdinal omega = CNFOrdinal.Omega;
            CNFOrdinal five = Finite(5);
            CNFOrdinal result = omega * five;
            Assert.AreEqual("ω*5", result.ToString(), "ω * 5 should equal ω*5.");
        }

        [Test]
        public void TestMultiplicationOmegaTimesOmega()
        {
            // ω * ω = ω^2.
            CNFOrdinal omega = CNFOrdinal.Omega;
            CNFOrdinal result = omega * omega;
            Assert.AreEqual("ω^2", result.ToString(), "ω * ω should equal ω^2.");
        }

        [Test]
        public void TestMultiplicationExample1()
        {
            // ω * 2 + 1 should equal ω*2 + 1.
            CNFOrdinal result = (CNFOrdinal.Omega * Finite(2)) + Finite(1);
            Assert.AreEqual("ω*2 + 1", result.ToString(), "ω * 2 + 1 should equal ω*2 + 1.");
        }

        [Test]
        public void TestMultiplicationExample2()
        {
            // (2 * ω + 1) * 2 should equal ω*2 + 1.
            // Note: 2 * ω equals ω.
            CNFOrdinal result = (Finite(2) * CNFOrdinal.Omega + Finite(1)) * Finite(2);
            Assert.AreEqual("ω*2 + 1", result.ToString(), "(2*ω + 1) * 2 should equal ω*2 + 1.");
        }

        [Test]
        public void TestMultiplicationExample3()
        {
            // ω * ((2 * ω) * 2) should equal ω^2*2.
            CNFOrdinal result = CNFOrdinal.Omega * ((Finite(2) * CNFOrdinal.Omega) * Finite(2));
            Assert.AreEqual("ω^2*2", result.ToString(), "ω * ((2 * ω) * 2) should equal ω^2*2.");
        }

        [Test]
        public void TestMultiplicationExample4()
        {
            // (ω + 1) * (ω + 1) should equal ω^2 + ω + 1.
            CNFOrdinal result = (CNFOrdinal.Omega + Finite(1)) * (CNFOrdinal.Omega + Finite(1));
            Assert.AreEqual("ω^2 + ω + 1", result.ToString(), "(ω + 1) * (ω + 1) should equal ω^2 + ω + 1.");
        }

        [Test]
        public void TestMultiplicationExample5()
        {
            // (ω + 1)^2 should equal ω^2 + ω + 1.
            CNFOrdinal result = (CNFOrdinal.Omega + Finite(1)).Power(Finite(2));
            Assert.AreEqual("ω^2 + ω + 1", result.ToString(), "(ω + 1)^2 should equal ω^2 + ω + 1.");
        }

        [Test]
        public void TestMultiplicationRightFinite3()
        {
            // (ω + 1) * 2 should equal ω*2 + 1.
            CNFOrdinal result = (CNFOrdinal.Omega + Finite(1)) * Finite(2);
            Assert.AreEqual("ω*2 + 1", result.ToString(), "(ω+1)*2 should equal ω*2 + 1.");
        }

        [Test]
        public void TestExponentiationFiniteBaseFiniteExponent()
        {
            // 2^3 = 8.
            CNFOrdinal two = Finite(2);
            CNFOrdinal three = Finite(3);
            CNFOrdinal result = two.Power(three);
            Assert.AreEqual("8", result.ToString(), "2^3 should equal 8.");
        }

        [Test]
        public void TestExponentiationOmegaPower3()
        {
            // ω^3 computed as ω raised to 3.
            CNFOrdinal omega = CNFOrdinal.Omega;
            CNFOrdinal three = Finite(3);
            CNFOrdinal result = omega.Power(three);
            Assert.AreEqual("ω^3", result.ToString(), "ω^3 should equal ω^3.");
        }

        [Test]
        public void TestAdditionOmegaPow3PlusOmegaPow2()
        {
            // ω^3 + ω^2 remains ω^3 + ω^2.
            CNFOrdinal omegaPow3 = OmegaPower(3);
            CNFOrdinal omegaPow2 = OmegaPower(2);
            CNFOrdinal result = omegaPow3 + omegaPow2;
            Assert.AreEqual("ω^3 + ω^2", result.ToString(), "ω^3 + ω^2 should equal ω^3 + ω^2.");
        }

        [Test]
        public void TestAdditionOmegaPow2PlusOmegaPow3()
        {
            // ω^2 + ω^3 equals ω^3.
            CNFOrdinal omegaPow2 = OmegaPower(2);
            CNFOrdinal omegaPow3 = OmegaPower(3);
            CNFOrdinal result = omegaPow2 + omegaPow3;
            Assert.AreEqual("ω^3", result.ToString(), "ω^2 + ω^3 should equal ω^3.");
        }

        [Test]
        public void TestMultiplicationOmegaPow2TimesOmegaPow3()
        {
            // ω^2 * ω^3 = ω^5.
            CNFOrdinal omegaPow2 = OmegaPower(2);
            CNFOrdinal omegaPow3 = OmegaPower(3);
            CNFOrdinal result = omegaPow2 * omegaPow3;
            Assert.AreEqual("ω^5", result.ToString(), "ω^2 * ω^3 should equal ω^5.");
        }

        [Test]
        public void TestExponentiationComplex1()
        {
            // (ω^3)^(ω^2) = ω^(ω^2) since 3 * ω^2 = ω^2.
            CNFOrdinal omegaPow3 = OmegaPower(3);
            CNFOrdinal omegaPow2 = OmegaPower(2);
            CNFOrdinal result = omegaPow3.Power(omegaPow2);
            Assert.AreEqual("ω^(ω^2)", result.ToString(), "(ω^3)^(ω^2) should equal ω^(ω^2).");
        }

        [Test]
        public void TestExponentiationComplex2()
        {
            // (ω^ω)^2 = ω^(ω*2).
            CNFOrdinal omegaPowOmega = new CNFOrdinal(new List<CNFOrdinalTerm>
            {
                new CNFOrdinalTerm(CNFOrdinal.Omega, 1)
            });
            CNFOrdinal two = Finite(2);
            CNFOrdinal result = omegaPowOmega.Power(two);
            Assert.AreEqual("ω^(ω*2)", result.ToString(), "(ω^ω)^2 should equal ω^(ω*2).");
        }

        [Test]
        public void TestExponentiationTower()
        {
            // A tower of 3 ω's should equal ω^(ω^ω).
            CNFOrdinal baseTower = CNFOrdinal.Omega;
            CNFOrdinal expTower = new CNFOrdinal(new List<CNFOrdinalTerm>
            {
                new CNFOrdinalTerm(CNFOrdinal.Omega, 1)
            });
            CNFOrdinal tower3 = baseTower.Power(expTower);
            Assert.AreEqual("ω^(ω^ω)", tower3.ToString(), "A tower of 3 ω's should equal ω^(ω^ω).");
        }

        [Test]
        public void TestExponentiationFiniteInfiniteBase()
        {
            // 2^(ω) = ω.
            CNFOrdinal result = Finite(2).Power(CNFOrdinal.Omega);
            Assert.AreEqual("ω", result.ToString(), "2^(ω) should equal ω.");
        }

        [Test]
        public void TestExponentiationInfiniteInfiniteBase()
        {
            // (ω+1)^(ω) = ω^ω.
            CNFOrdinal result = (CNFOrdinal.Omega + Finite(1)).Power(CNFOrdinal.Omega);
            Assert.AreEqual("ω^ω", result.ToString(), "(ω+1)^(ω) should equal ω^ω.");
        }

        [Test]
        public void TestMultiplicationOfInfinite4()
        {
            // ω^3 * ω^3 = ω^6.
            CNFOrdinal result = OmegaPower(3) * OmegaPower(3);
            Assert.AreEqual("ω^6", result.ToString(), "ω^3 * ω^3 should equal ω^6.");
        }

        [Test]
        public void TestComparisonOfAddition3()
        {
            // ω^3 + ω^2 should be greater than ω^3.
            CNFOrdinal sum = OmegaPower(3) + OmegaPower(2);
            Assert.IsTrue(sum.CompareTo(OmegaPower(3)) > 0, "ω^3 + ω^2 should be greater than ω^3.");
        }

        [Test]
        public void TestAdditionOrdering3()
        {
            // ω^2 + ω^3 should equal ω^3.
            CNFOrdinal result = OmegaPower(2) + OmegaPower(3);
            Assert.AreEqual("ω^3", result.ToString(), "ω^2 + ω^3 should equal ω^3.");
        }

        // ----- Additional Expected Examples -----

        [Test]
        public void TestToStringExamples()
        {
            // ω^ω should equal "ω^(ω)"
            CNFOrdinal omegaPowOmega = new CNFOrdinal(new List<CNFOrdinalTerm>
            {
                new CNFOrdinalTerm(CNFOrdinal.Omega, 1)
            });
            Assert.AreEqual("ω^ω", omegaPowOmega.ToString(), "ω^ω should equal ω^ω.");

            // A tower of 4 ω's should equal "ω^(ω^(ω^ω))"
            CNFOrdinal inner = new CNFOrdinal(new List<CNFOrdinalTerm>
            {
                new CNFOrdinalTerm(CNFOrdinal.Omega, 1)
            });
            CNFOrdinal middle = new CNFOrdinal(new List<CNFOrdinalTerm>
            {
                new CNFOrdinalTerm(inner, 1)
            });
            CNFOrdinal tower4 = new CNFOrdinal(new List<CNFOrdinalTerm>
            {
                new CNFOrdinalTerm(middle, 1)
            });
            Assert.AreEqual("ω^(ω^(ω^ω))", tower4.ToString(), "A tower of 4 ω's should equal ω^(ω^(ω^ω)).");

            // ω^(ω + 1) should equal "ω^(ω + 1)"
            CNFOrdinal omegaPlusOne = CNFOrdinal.Omega + Finite(1);
            CNFOrdinal exp1 = new CNFOrdinal(new List<CNFOrdinalTerm>
            {
                new CNFOrdinalTerm(omegaPlusOne, 1)
            });
            Assert.AreEqual("ω^(ω + 1)", exp1.ToString(), "ω^(ω + 1) should equal ω^(ω + 1).");

            // ω^(ω^ω*2 + 1) should equal "ω^(ω^ω*2 + 1)"
            CNFOrdinal omegaPowOmegaTimes2 = omegaPowOmega.Multiply(Finite(2));
            CNFOrdinal expTail = omegaPowOmegaTimes2 + Finite(1);
            CNFOrdinal exp2 = new CNFOrdinal(new List<CNFOrdinalTerm>
            {
                new CNFOrdinalTerm(expTail, 1)
            });
            Assert.AreEqual("ω^(ω^ω*2 + 1)", exp2.ToString(), "ω^(ω^ω*2 + 1) should equal ω^(ω^ω*2 + 1).");
        }

        [Test]
        public void TestIterativeAddition_EqualHeads_MergesCoefficients()
        {
            // Create two ordinals with the same head exponent (using CNFOrdinal.One as exponent)
            CNFOrdinal ordinal1 = new CNFOrdinal(new List<CNFOrdinalTerm> { new CNFOrdinalTerm(CNFOrdinal.One, 2) });
            CNFOrdinal ordinal2 = new CNFOrdinal(new List<CNFOrdinalTerm> { new CNFOrdinalTerm(CNFOrdinal.One, 3) });
            CNFOrdinal sum = ordinal1.Add(ordinal2);
            // Expect the merged coefficient to be 5 with the same exponent
            Assert.AreEqual(CNFOrdinal.One, sum[0].Exponent);
            Assert.AreEqual(5, sum[0].Coefficient);
        }

        [Test]
        public void TestIterativeAddition_MixedOrder_AppendsRemainingTerms()
        {
            // Create an ordinal with a higher head exponent (using CNFOrdinal.Omega)
            CNFOrdinal ordinal1 = new CNFOrdinal(new List<CNFOrdinalTerm> { new CNFOrdinalTerm(CNFOrdinal.Omega, 4) });
            // Create an ordinal with a lower head exponent (using CNFOrdinal.One)
            CNFOrdinal ordinal2 = new CNFOrdinal(new List<CNFOrdinalTerm> { new CNFOrdinalTerm(CNFOrdinal.One, 3) });
            CNFOrdinal sum = ordinal1.Add(ordinal2);
            // The result should preserve ordinal1's term and then append ordinal2's term
            Assert.AreEqual(2, sum.Count);
            Assert.AreEqual(CNFOrdinal.Omega, sum[0].Exponent);
            Assert.AreEqual(4, sum[0].Coefficient);
            Assert.AreEqual(CNFOrdinal.One, sum[1].Exponent);
            Assert.AreEqual(3, sum[1].Coefficient);
        }

        [Test]
        public void TestIterativeAddition_FiniteOrdinals_AddsNormally()
        {
            // Finite ordinal addition should use standard natural number addition
            CNFOrdinal finite1 = CNFOrdinal.FromFinite(7);
            CNFOrdinal finite2 = CNFOrdinal.FromFinite(8);
            CNFOrdinal sum = finite1.Add(finite2);
            Assert.IsTrue(sum.IsFinite);
            Assert.AreEqual(15, sum.ToFiniteValue());
        }
    }
}
