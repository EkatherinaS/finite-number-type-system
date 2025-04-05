using System;
using System.Collections.Generic;

namespace Assets.Scripts.AssemblyMath
{
    /// <summary>
    /// Represents an ordinal (finite or below ε₀) in Cantor Normal Form.
    /// Zero is represented as an empty sequence.
    /// </summary>
    public partial class CNFOrdinal : IComparable<CNFOrdinal>
    {
        internal readonly CNFOrdinalTerm[] _terms;
        internal readonly int _offset;
        internal readonly int _length;

        public int Count => _length;

        public CNFOrdinalTerm this[int i] => _terms[_offset + i];

        public CNFOrdinal(List<CNFOrdinalTerm> terms)
        {
            List<CNFOrdinalTerm> normalized = Normalize(terms);
            _terms = normalized.ToArray();
            _offset = 0;
            _length = _terms.Length;
        }

        internal CNFOrdinal(CNFOrdinalTerm[] terms, int offset, int length, bool alreadyNormalized)
        {
            if (!alreadyNormalized)
            {
                List<CNFOrdinalTerm> list = new List<CNFOrdinalTerm>();
                for (int i = offset; i < offset + length; i++)
                    list.Add(terms[i]);
                List<CNFOrdinalTerm> normalized = Normalize(list);
                _terms = normalized.ToArray();
                _offset = 0;
                _length = _terms.Length;
            }
            else
            {
                _terms = terms;
                _offset = offset;
                _length = length;
            }
        }

        public List<CNFOrdinalTerm> ToList()
        {
            List<CNFOrdinalTerm> list = new List<CNFOrdinalTerm>(_length);
            for (int i = 0; i < _length; i++)
                list.Add(this[i]);
            return list;
        }

        public static CNFOrdinal FromFinite(int value)
        {
            if (value < 0)
                throw new ArgumentException("Finite ordinal must be non-negative.", nameof(value));
            if (value == 0)
                return Zero;
            return new CNFOrdinal(new CNFOrdinalTerm[] { new CNFOrdinalTerm(Zero, value) }, 0, 1, true);
        }

        public static readonly CNFOrdinal Zero = new CNFOrdinal(new CNFOrdinalTerm[0], 0, 0, true);
        public static readonly CNFOrdinal One = new CNFOrdinal(new CNFOrdinalTerm[] { new CNFOrdinalTerm(Zero, 1) }, 0, 1, true);
        public static readonly CNFOrdinal Omega = new CNFOrdinal(new CNFOrdinalTerm[] { new CNFOrdinalTerm(One, 1) }, 0, 1, true);

        #region Properties

        public bool IsZero { get { return _length == 0; } }
        public bool IsFinite { get { return IsZero || (_length == 1 && this[0].Exponent.Equals(Zero)); } }
        public bool IsOne { get { return !IsZero && IsFinite && this[0].Coefficient == 1; } }
        public bool IsLimitOrdinal { get { return !IsZero && !IsFinite && GetFiniteTermOrdinal().IsZero; } }
        public bool IsSuccessorOrdinal { get { return !IsZero && !IsLimitOrdinal; } }

        #endregion

        internal CNFOrdinal Slice(int startIndex, int length)
        {
            if (startIndex < 0 || startIndex + length > _length)
                throw new ArgumentOutOfRangeException();
            return new CNFOrdinal(_terms, _offset + startIndex, length, true);
        }

        internal CNFOrdinal Prepend(CNFOrdinalTerm term)
        {
            CNFOrdinalTerm[] newTerms = new CNFOrdinalTerm[_length + 1];
            newTerms[0] = term;
            Array.Copy(_terms, _offset, newTerms, 1, _length);
            return new CNFOrdinal(newTerms, 0, newTerms.Length, true);
        }

        internal static CNFOrdinal Concat(CNFOrdinal a, CNFOrdinal b)
        {
            CNFOrdinalTerm[] result = new CNFOrdinalTerm[a.Count + b.Count];
            Array.Copy(a._terms, a._offset, result, 0, a.Count);
            Array.Copy(b._terms, b._offset, result, a.Count, b.Count);
            return new CNFOrdinal(result, 0, result.Length, true);
        }
    }
}
