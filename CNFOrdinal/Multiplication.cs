using System;
using System.Collections.Generic;

namespace Assets.Scripts.AssemblyMath
{
    public partial class CNFOrdinal
    {
        public CNFOrdinal Multiply(CNFOrdinal other)
        {
            return pmult(this, other, 0);
        }

        internal static CNFOrdinal FiniteMultiply(CNFOrdinal a, CNFOrdinal b)
        {
            if (a.IsZero || b.IsZero)
                return Zero;
            int aVal = a[0].Coefficient;
            int bVal = b[0].Coefficient;
            int prod = aVal * bVal;
            return new CNFOrdinal(new CNFOrdinalTerm[] { new CNFOrdinalTerm(Zero, prod) }, 0, 1, true);
        }

        internal static CNFOrdinal pmult(CNFOrdinal a, CNFOrdinal b, int n)
        {
            if (a.IsZero || b.IsZero)
                return Zero;
            if (a.IsFinite && b.IsFinite)
                return FiniteMultiply(a, b);
            if (b.IsFinite)
            {
                int finiteVal = b[0].Coefficient;
                CNFOrdinalTerm newHead = new CNFOrdinalTerm(a[0].Exponent, a[0].Coefficient * finiteVal);
                CNFOrdinal headOrdinal = new CNFOrdinal(new CNFOrdinalTerm[] { newHead }, 0, 1, true);
                CNFOrdinal tail = RestN(a, 1);
                return Concat(headOrdinal, tail);
            }
            else
            {
                CNFOrdinal fe_a = a[0].Exponent;
                CNFOrdinal fe_b = b[0].Exponent;
                int m = c2(fe_a, fe_b, n);
                CNFOrdinal newExp = Padd(fe_a, fe_b, m);
                int newCoef = b[0].Coefficient;
                CNFOrdinal bTail = RestN(b, 1);
                CNFOrdinal tailProd = pmult(a, bTail, m);
                List<CNFOrdinalTerm> list = new List<CNFOrdinalTerm>();
                list.Add(new CNFOrdinalTerm(newExp, newCoef));
                for (int i = 0; i < tailProd.Count; i++)
                    list.Add(tailProd[i]);
                return new CNFOrdinal(list.ToArray(), 0, list.Count, true);
            }
        }

        public static CNFOrdinal operator *(CNFOrdinal a, CNFOrdinal b)
        {
            return a.Multiply(b);
        }
    }
}
