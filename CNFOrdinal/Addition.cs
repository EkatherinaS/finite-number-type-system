using System.Collections.Generic;

namespace Assets.Scripts.AssemblyMath
{
    public partial class CNFOrdinal
    {
        public CNFOrdinal Add(CNFOrdinal other)
        {
            return AddInternal(this, other);
        }

        internal static CNFOrdinal AddInternal(CNFOrdinal a, CNFOrdinal b)
        {
            if (a.IsZero)
                return b;
            if (b.IsZero)
                return a;
            if (a.IsFinite && b.IsFinite)
            {
                int aVal = a[0].Coefficient;
                int bVal = b[0].Coefficient;
                int sum = aVal + bVal;
                return new CNFOrdinal(new CNFOrdinalTerm[] { new CNFOrdinalTerm(Zero, sum) }, 0, 1, true);
            }

            List<CNFOrdinalTerm> result = new List<CNFOrdinalTerm>();
            int cmpInitial = a[0].Exponent.CompareTo(b[0].Exponent);
            if (cmpInitial < 0)
            {
                return new CNFOrdinal(b.ToList().ToArray(), 0, b.Count, true);
            }
            else if (cmpInitial == 0)
            {
                int combinedCoef = a[0].Coefficient + b[0].Coefficient;
                result.Add(new CNFOrdinalTerm(a[0].Exponent, combinedCoef));
                for (int j = 1; j < b.Count; j++)
                    result.Add(b[j]);
                return new CNFOrdinal(result.ToArray(), 0, result.Count, true);
            }
            else
            {
                int i = 0;
                while (i < a.Count)
                {
                    int cmp = a[i].Exponent.CompareTo(b[0].Exponent);
                    if (cmp > 0)
                    {
                        result.Add(a[i]);
                        i++;
                    }
                    else if (cmp == 0)
                    {
                        int combinedCoef = a[i].Coefficient + b[0].Coefficient;
                        result.Add(new CNFOrdinalTerm(a[i].Exponent, combinedCoef));
                        for (int j = 1; j < b.Count; j++)
                            result.Add(b[j]);
                        return new CNFOrdinal(result.ToArray(), 0, result.Count, true);
                    }
                    else
                    {
                        for (int j = 0; j < b.Count; j++)
                            result.Add(b[j]);
                        return new CNFOrdinal(result.ToArray(), 0, result.Count, true);
                    }
                }
                for (int j = 0; j < b.Count; j++)
                    result.Add(b[j]);
                return new CNFOrdinal(result.ToArray(), 0, result.Count, true);
            }
        }

        public static CNFOrdinal operator +(CNFOrdinal a, CNFOrdinal b)
        {
            return a.Add(b);
        }
    }
}
