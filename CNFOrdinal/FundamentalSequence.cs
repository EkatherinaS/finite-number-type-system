using System;
using System.Collections.Generic;

namespace Assets.Scripts.AssemblyMath
{
    public partial class CNFOrdinal
    {
        /// <summary>
        /// For limit ordinals, returns the n'th ordinal in it's fundamental sequence.
        /// </summary>
        public CNFOrdinal GetNthOrdinalInSequence(int n)
        {
            if (!this.IsLimitOrdinal)
                throw new Exception("GetNthOrdinalInSequence is defined only for limit ordinals.");
            if (this.Count > 1)
            {
                CNFOrdinal prefix = this.Slice(0, this.Count - 1);
                CNFOrdinal last = this.Slice(this.Count - 1, 1);
                CNFOrdinal newLast = last.GetNthOrdinalInSequence(n);
                return prefix.Add(newLast);
            }
            else
            {
                CNFOrdinalTerm term = this[0];
                if (term.Coefficient > 1)
                {
                    CNFOrdinal firstPart = new CNFOrdinal(new CNFOrdinalTerm[] { new CNFOrdinalTerm(term.Exponent, term.Coefficient - 1) }, 0, 1, true);
                    CNFOrdinal secondPart = new CNFOrdinal(new CNFOrdinalTerm[] { new CNFOrdinalTerm(term.Exponent, 1) }, 0, 1, true);
                    CNFOrdinal nth = secondPart.GetNthOrdinalInSequence(n);
                    return firstPart.Add(nth);
                }
                else
                {
                    if (term.Exponent.Equals(One))
                        return new CNFOrdinal(new CNFOrdinalTerm[] { new CNFOrdinalTerm(Zero, n) }, 0, 1, true);
                    if (term.Exponent.IsSuccessorOrdinal)
                    {
                        CNFOrdinal bMinusOne = subtractOneFromSuccessor(term.Exponent);
                        return new CNFOrdinal(new CNFOrdinalTerm[] { new CNFOrdinalTerm(bMinusOne, n) }, 0, 1, true);
                    }
                    if (term.Exponent.IsLimitOrdinal)
                    {
                        CNFOrdinal nthExp = term.Exponent.GetNthOrdinalInSequence(n);
                        return new CNFOrdinal(new CNFOrdinalTerm[] { new CNFOrdinalTerm(nthExp, 1) }, 0, 1, true);
                    }
                    return new CNFOrdinal(new CNFOrdinalTerm[] { new CNFOrdinalTerm(Zero, n) }, 0, 1, true);
                }
            }
        }

        public CNFOrdinal GetPredecessorOrNthOrdinalInSequence(int n)
        {
            if (this.IsLimitOrdinal)
                return GetNthOrdinalInSequence(n);
            else
                return subtractOneFromSuccessor(this);
        }

        internal CNFOrdinal GetFiniteTermOrdinal()
        {
            if (IsZero)
                return Zero;
            CNFOrdinalTerm last = this[this.Count - 1];
            if (last.Exponent.IsZero)
                return new CNFOrdinal(new CNFOrdinalTerm[] { last }, 0, 1, true);
            return Zero;
        }

        internal static CNFOrdinal subtractOneFromSuccessor(CNFOrdinal a)
        {
            CNFOrdinal finitePart = a.GetFiniteTermOrdinal();
            if (finitePart.IsZero)
                throw new Exception("Ordinal does not have a finite successor part.");
            int value = finitePart[0].Coefficient;
            List<CNFOrdinalTerm> newTerms = new List<CNFOrdinalTerm>();
            for (int i = 0; i < a.Count - 1; i++)
                newTerms.Add(a[i]);
            if (value > 1)
            {
                newTerms.Add(new CNFOrdinalTerm(Zero, value - 1));
            }
            return new CNFOrdinal(newTerms.ToArray(), 0, newTerms.Count, true);
        }
    }
}
