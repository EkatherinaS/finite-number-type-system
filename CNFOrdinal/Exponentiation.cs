using System;

namespace Assets.Scripts.AssemblyMath
{
    public partial class CNFOrdinal
    {
        public CNFOrdinal Power(CNFOrdinal other)
        {
            return expo(this, other);
        }

        public static CNFOrdinal operator ^(CNFOrdinal a, CNFOrdinal b)
        {
            return a.Power(b);
        }

        internal static CNFOrdinal expo(CNFOrdinal a, CNFOrdinal b)
        {
            if (b.IsZero || a.Equals(One))
                return One;
            if (a.IsZero)
                return Zero;
            if (a.IsFinite && b.IsFinite)
                return FinitePower(a, b);
            if (a.IsFinite)
                return exp1(a, b);
            if (b.IsFinite)
                return exp3(a, b[0].Coefficient);
            return exp4(a, b);
        }

        internal static CNFOrdinal FinitePower(CNFOrdinal a, CNFOrdinal b)
        {
            if (a.IsZero || b.IsZero) return One;
            int baseVal = a[0].Coefficient;
            int expVal = b[0].Coefficient;
            int result = (int)Math.Pow(baseVal, expVal);
            return new CNFOrdinal(new CNFOrdinalTerm[] { new CNFOrdinalTerm(Zero, result) }, 0, 1, true);
        }

        internal static CNFOrdinal exp1(CNFOrdinal finiteBase, CNFOrdinal a)
        {
            (CNFOrdinalTerm head, CNFOrdinal tail) = GetHeadAndTail(a);
            if (head.Exponent.CompareTo(One) == 0)
            {
                CNFOrdinal finiteExp = FinitePower(finiteBase, tail);
                CNFOrdinal newExp = new CNFOrdinal(new CNFOrdinalTerm[] { new CNFOrdinalTerm(Zero, head.Coefficient) }, 0, 1, true);
                int finiteExpVal = finiteExp[0].Coefficient;
                return new CNFOrdinal(new CNFOrdinalTerm[] { new CNFOrdinalTerm(newExp, finiteExpVal) }, 0, 1, true);
            }
            if (tail.IsFinite)
            {
                CNFOrdinal headExpMinusOne = subtractOneFromSuccessor(head.Exponent);
                CNFOrdinal term1 = new CNFOrdinal(new CNFOrdinalTerm[] { new CNFOrdinalTerm(headExpMinusOne, head.Coefficient) }, 0, 1, true);
                CNFOrdinal finiteExp = FinitePower(finiteBase, tail);
                return term1.Add(finiteExp);
            }
            CNFOrdinal c = exp1(finiteBase, tail);
            CNFOrdinal newHead = new CNFOrdinal(new CNFOrdinalTerm[] { new CNFOrdinalTerm(subtractOneFromSuccessor(head.Exponent), 1) }, 0, 1, true);
            CNFOrdinal termFromC = new CNFOrdinal(new CNFOrdinalTerm[] { new CNFOrdinalTerm(c[0].Exponent, c[0].Coefficient) }, 0, 1, true);
            return newHead.Add(termFromC);
        }

        internal static CNFOrdinal exp2(CNFOrdinal a, int k)
        {
            if (k == 0) return One;
            CNFOrdinal factor = a[0].Exponent.Multiply(
                new CNFOrdinal(new CNFOrdinalTerm[] { new CNFOrdinalTerm(Zero, k - 1) }, 0, 1, true));
            CNFOrdinal term = new CNFOrdinal(new CNFOrdinalTerm[] { new CNFOrdinalTerm(factor, 1) }, 0, 1, true);
            return term.Multiply(a);
        }

        internal static CNFOrdinal exp3h(CNFOrdinal a, CNFOrdinal p, int n, int k)
        {
            if (k == 1)
            {
                return a.Multiply(p).Add(p);
            }
            else
            {
                CNFOrdinal part1 = exp2(a, k).Multiply(p);
                CNFOrdinal part2 = exp3h(a, p, n, k - 1);
                return Padd(part1, part2, n);
            }
        }

        internal static CNFOrdinal exp3(CNFOrdinal a, int k)
        {
            if (k == 0) return One;
            if (k == 1) return a;

            if (a.IsLimitOrdinal)
            {
                return exp2(a, k);
            }
            else
            {
                CNFOrdinal c = a.GetLimitTermsOrdinal();
                CNFOrdinal np = a.GetFiniteTermOrdinal();
                int nVal = np.IsZero ? 0 : np[0].Coefficient;
                return Padd(exp2(c, k), exp3h(c, np, nVal, k - 1), nVal);
            }
        }

        internal static CNFOrdinal exp4(CNFOrdinal a, CNFOrdinal b)
        {
            CNFOrdinal factor = a[0].Exponent.Multiply(b.GetLimitTermsOrdinal());
            CNFOrdinal term = new CNFOrdinal(new CNFOrdinalTerm[] { new CNFOrdinalTerm(factor, 1) }, 0, 1, true);
            CNFOrdinal np = b.GetFiniteTermOrdinal();
            int npVal = np.IsZero ? 0 : np[0].Coefficient;
            CNFOrdinal exp3Result = exp3(a, npVal);
            return term.Multiply(exp3Result);
        }
    }
}
