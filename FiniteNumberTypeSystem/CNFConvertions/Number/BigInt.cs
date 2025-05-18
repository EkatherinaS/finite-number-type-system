using System;
using System.Numerics;

namespace CNFConvertions.Number
{
    public class BigInt : INumber
    {
        private static readonly BigInteger MAX = BigInteger.Pow(10, Constants.EXPONENT);

        private static readonly BigInt MAX_VALUE = new BigInt(MAX);
        private static readonly BigInt MIN_VALUE = new BigInt(1);


        private BigInteger n;
        public BigInteger N { get { return n; } set { n = value; } }

        public BigInt(int n)
        {
            if (n < 1) throw new ArgumentException("BigInt must be positive");
            this.n = n;
        }

        public BigInt(BigInteger n)
        {
            if (n < 1) throw new ArgumentException("BigInt must be positive");
            if (n > MAX) throw new ArgumentException("BigInt must be in range 1..10^1000");
            this.n = n;
        }

        public bool Equals(BigInt other) => n.Equals(other.n);

        public static BigInt GetMax() => MAX_VALUE;

        public static BigInt GetMin() => MIN_VALUE;

        public override string ToString() => n.ToString();

        public override string ToLaTeX() => n.ToString();

        public override int CompareTo(INumber? other)
        {
            if (other is null) return 0;

            if (other.GetType() == typeof(BigInt))
            {
                BigInt item = (BigInt)other;
                return n.CompareTo(item.n);
            }

            if (other.GetType() == typeof(KnuthUpArrow))
            {
                KnuthUpArrow item = (KnuthUpArrow)other;
                BigInt? itemBigInt = item.ToBigInt();
                if (itemBigInt != null) return CompareTo(itemBigInt);
                else return 1;
            }

            if (other.GetType() == typeof(FGH)) return 1;

            throw new NotImplementedException();
        }

        public override INumber Succ()
        {
            if (n < MAX_VALUE) return new BigInt(n + 1);
            else return new KnuthUpArrow(10, CountDigits(n), 1);
        }

        public override int GetHashCode() => n.GetHashCode();

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            BigInt? item = obj as BigInt;
            if (item is null) return false;
            return this.n.Equals(item.n);
        }

        public static implicit operator BigInteger(BigInt bigInt) => bigInt.n;

        public static bool IsConvertible(BigInteger n) => (n >= 1 && n <= MAX);
        public static BigInt Pow(BigInt a, int exponent) => new BigInt(BigInteger.Pow(a.n, exponent));
        public static BigInt ModPow(BigInt a, int exponent, int mod) => new BigInt(BigInteger.ModPow(a.n, exponent, mod));

        public static bool operator >=(BigInt a, int b) => a.n >= b;
        public static bool operator <=(BigInt a, int b) => a.n <= b;

        public static bool operator >(BigInt a, int b) => a.n > b;
        public static bool operator <(BigInt a, int b) => a.n < b;

        public static bool operator ==(BigInt a, int b) => a.n == b;
        public static bool operator !=(BigInt a, int b) => a.n != b;
    }
}
