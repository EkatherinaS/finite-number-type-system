using System;
using System.Collections.Generic;

namespace Assets.Scripts.AssemblyMath
{
    public partial class CNFOrdinal
    {
        internal static (CNFOrdinalTerm head, CNFOrdinal tail) GetHeadAndTail(CNFOrdinal a)
        {
            if (a.IsZero)
                throw new InvalidOperationException("Zero ordinal has no head and tail.");
            CNFOrdinalTerm head = a[0];
            CNFOrdinal tail = a.Slice(1, a.Count - 1);
            return (head, tail);
        }

        internal CNFOrdinal GetLimitTermsOrdinal()
        {
            if (IsZero)
                return Zero;
            if (Count == 0)
                return Zero;
            CNFOrdinalTerm last = this[Count - 1];
            if (last.Exponent.IsZero)
            {
                List<CNFOrdinalTerm> newTerms = new List<CNFOrdinalTerm>();
                for (int i = 0; i < Count - 1; i++)
                    newTerms.Add(this[i]);
                return new CNFOrdinal(newTerms.ToArray(), 0, newTerms.Count, true);
            }
            else
            {
                return this;
            }
        }

        public int ToFiniteValue()
        {
            if (!IsFinite)
                throw new InvalidOperationException("Ordinal is not finite.");
            return this[0].Coefficient;
        }

        internal static List<CNFOrdinalTerm> Normalize(List<CNFOrdinalTerm> rawTerms)
        {
            List<CNFOrdinalTerm> sorted = new List<CNFOrdinalTerm>(rawTerms);
            sorted.Sort(delegate (CNFOrdinalTerm t1, CNFOrdinalTerm t2)
            {
                return t2.Exponent.CompareTo(t1.Exponent);
            });
            List<CNFOrdinalTerm> normalized = new List<CNFOrdinalTerm>();
            for (int i = 0; i < sorted.Count; i++)
            {
                CNFOrdinalTerm term = sorted[i];
                if (normalized.Count > 0 &&
                    normalized[normalized.Count - 1].Exponent.CompareTo(term.Exponent) == 0)
                {
                    CNFOrdinalTerm last = normalized[normalized.Count - 1];
                    normalized[normalized.Count - 1] = new CNFOrdinalTerm(last.Exponent, last.Coefficient + term.Coefficient);
                }
                else
                {
                    normalized.Add(term);
                }
            }
            for (int i = normalized.Count - 1; i >= 0; i--)
            {
                if (normalized[i].Coefficient == 0)
                    normalized.RemoveAt(i);
            }
            return normalized;
        }

        internal static CNFOrdinal RestN(CNFOrdinal a, int n)
        {
            if (n <= 0)
                return a;
            if (n >= a.Count)
                return Zero;
            return a.Slice(n, a.Count - n);
        }

        internal static CNFOrdinal Padd(CNFOrdinal a, CNFOrdinal b, int n)
        {
            if (n <= 0)
                return a.Add(b);
            if (n == a.Count)
                return a.Add(b);
            if (n > a.Count)
                return b;

            CNFOrdinal prefix = a.Slice(0, n);
            CNFOrdinal remainder = a.Slice(n, a.Count - n).Add(b);
            CNFOrdinalTerm[] result = new CNFOrdinalTerm[prefix.Count + remainder.Count];
            Array.Copy(prefix._terms, prefix._offset, result, 0, prefix.Count);
            Array.Copy(remainder._terms, remainder._offset, result, prefix.Count, remainder.Count);
            return new CNFOrdinal(result, 0, result.Length, true);
        }

        internal static int c1(CNFOrdinal a, CNFOrdinal b)
        {
            if (a.Count <= 16)
            {
                int count = 0;
                for (int i = 0; i < a.Count; i++)
                {
                    if (a[i].Exponent.CompareTo(b) > 0)
                        count++;
                    else
                        break;
                }
                return count;
            }
            else
            {
                int low = 0;
                int high = a.Count;
                while (low < high)
                {
                    int mid = low + (high - low) / 2;
                    if (a[mid].Exponent.CompareTo(b) > 0)
                        low = mid + 1;
                    else
                        high = mid;
                }
                return low;
            }
        }

        internal static int c2(CNFOrdinal a, CNFOrdinal b, int n)
        {
            return n + c1(RestN(a, n), b);
        }
    }
}
