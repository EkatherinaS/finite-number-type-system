using System.Numerics;

namespace CNFConvertions.Number
{
    public class BigInt : INumber
    {
        private static readonly BigInteger MAX = BigInteger.Pow(10, Constants.EXPONENT);

        private static readonly BigInt MAX_VALUE = new BigInt(MAX);
        private static readonly BigInt MIN_VALUE = new BigInt(1);

        private BigInteger value;

        public BigInt(int value)
        {
            if (value < 1)
            {
                throw new ArgumentException("BigInt must be positive");
            }
            this.value = value;
        }

        public BigInt(BigInteger value)
        {
            if (value < 1 || value > MAX)
            {
                throw new ArgumentException("BigInt must be poaitive");
            }
            this.value = value;
        }

        public bool Equals(BigInt other) => value.Equals(other.value);

        public static BigInt GetMax() => MAX_VALUE;

        public static BigInt GetMin() => MIN_VALUE;

        public override string ToString() => value.ToString();

        public override string ToLaTeX() => value.ToString();

        public override int CompareTo(INumber? other)
        {
            if (other == null) return 0;

            if (other.GetType() == typeof(BigInt))
            {
                BigInt item = (BigInt)other;
                return value.CompareTo(item.value);
            }
            if (other.GetType() == typeof(KnuthUpArrow))
            {
                //TODO: compare BigInt & KnuthUpArrow
                return 1;
            }
            if (other.GetType() == typeof(FGH))
            {
                return 1;
            }

            throw new NotImplementedException();
        }

        public override int GetHashCode() => value.GetHashCode();

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            BigInt? item = obj as BigInt;

            if (item is null) return false;

            return this.value.Equals(item.value);
        }


        public static BigInt Pow(BigInt a, int exponent) => new BigInt(BigInteger.Pow(a.value, exponent));
        public static BigInt ModPow(BigInt a, int exponent, int mod) => new BigInt(BigInteger.ModPow(a.value, exponent, mod));

        public static implicit operator BigInteger(BigInt bigInt) => bigInt.value;

        public static BigInt operator +(BigInt a, BigInt b) => new BigInt(a.value + b.value);
        public static BigInt operator *(BigInt a, BigInt b) => new BigInt(a.value * b.value);
        public static BigInt operator +(BigInt a, int b) => new BigInt(a.value + b);
        public static BigInt operator *(BigInt a, int b) => new BigInt(a.value * b);

        public static bool operator >=(BigInt a, BigInt b) => a.value >= b.value;
        public static bool operator <=(BigInt a, BigInt b) => a.value <= b.value;
        public static bool operator >=(BigInt a, int b) => a.value >= b;
        public static bool operator <=(BigInt a, int b) => a.value <= b;

        public static bool operator >(BigInt a, BigInt b) => a.value > b.value;
        public static bool operator <(BigInt a, BigInt b) => a.value < b.value;
        public static bool operator >(BigInt a, int b) => a.value > b;
        public static bool operator <(BigInt a, int b) => a.value < b;

        public static bool operator ==(BigInt a, BigInt b) => a.value == b.value;
        public static bool operator !=(BigInt a, BigInt b) => a.value != b.value;
        public static bool operator ==(BigInt a, int b) => a.value == b;
        public static bool operator !=(BigInt a, int b) => a.value != b;
    }
}
