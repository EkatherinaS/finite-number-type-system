using System;
using System.Text;

namespace Assets.Scripts.AssemblyMath
{
    public partial class CNFOrdinal : IComparable<CNFOrdinal>
    {
        public override string ToString()
        {
            if (IsZero)
                return "0";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Count; i++)
            {
                CNFOrdinalTerm term = this[i];
                if (i > 0)
                    sb.Append(" + ");
                if (term.Exponent.IsZero)
                {
                    sb.Append(term.Coefficient);
                }
                else
                {
                    // Build ω^(exponent)
                    if (term.Exponent.IsOne)
                    {
                        sb.Append("ω");
                    }
                    else
                    {
                        bool isAtomic = term.Exponent.IsFinite || term.Exponent.Equals(Omega);
                        if (isAtomic)
                        {
                            sb.Append("ω^");
                            sb.Append(term.Exponent.ToString());
                        }
                        else
                        {
                            sb.Append("ω^(");
                            sb.Append(term.Exponent.ToString());
                            sb.Append(")");
                        }
                    }
                    if (term.Coefficient != 1)
                    {
                        sb.Append("*");
                        sb.Append(term.Coefficient);
                    }
                }
            }
            return sb.ToString();
        }

        public string ToLaTeX()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Count; i++)
            {
                if (i > 0)
                    sb.Append(" + ");

                CNFOrdinalTerm term = this[i];
                if (term.Exponent.IsZero)
                {
                    sb.Append(term.Coefficient);
                }
                else
                {
                    sb.Append(@"\omega");
                    if (!term.Exponent.Equals(One))
                    {
                        sb.Append("^{");
                        sb.Append(term.Exponent.ToLaTeX());
                        sb.Append("}");
                    }
                    if (term.Coefficient != 1)
                    {
                        sb.Append(@"\cdot ");
                        sb.Append(term.Coefficient);
                    }
                }
            }
            return sb.ToString();
        }

        public int CompareTo(CNFOrdinal other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            if (this.IsZero && other.IsZero)
                return 0;
            if (this.IsZero)
                return -1;
            if (other.IsZero)
                return 1;
            int minCount = this.Count < other.Count ? this.Count : other.Count;
            for (int i = 0; i < minCount; i++)
            {
                int cmpExp = this[i].Exponent.CompareTo(other[i].Exponent);
                if (cmpExp != 0)
                    return cmpExp;
                int cmpCoef = this[i].Coefficient.CompareTo(other[i].Coefficient);
                if (cmpCoef != 0)
                    return cmpCoef;
            }
            return this.Count.CompareTo(other.Count);
        }

        public override bool Equals(object obj)
        {
            if (obj is CNFOrdinal other)
                return this.CompareTo(other) == 0;
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            for (int i = 0; i < Count; i++)
            {
                hash = hash * 31 + this[i].Exponent.GetHashCode();
                hash = hash * 31 + this[i].Coefficient.GetHashCode();
            }
            return hash;
        }
    }
}
