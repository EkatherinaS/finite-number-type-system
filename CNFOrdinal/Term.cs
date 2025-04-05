using System;

namespace Assets.Scripts.AssemblyMath
{
    /// <summary>
    /// Represents a single term in Cantor Normal Form: ω^(Exponent) * Coefficient.
    /// </summary>
    public struct CNFOrdinalTerm
    {
        /// <summary>
        /// The exponent (itself a CNFOrdinal).
        /// </summary>
        public CNFOrdinal Exponent { get; }
        /// <summary>
        /// The positive integer coefficient.
        /// </summary>
        public int Coefficient { get; }

        public CNFOrdinalTerm(CNFOrdinal exponent, int coefficient)
        {
            if (coefficient <= 0)
                throw new ArgumentException("Coefficient must be positive.", nameof(coefficient));
            Exponent = exponent;
            Coefficient = coefficient;
        }
    }
}
