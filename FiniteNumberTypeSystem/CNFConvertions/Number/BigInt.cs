using System;
using System.Numerics;

namespace CNFConvertions.Number
{
    [System.Serializable]
    public class BigInt : INumber
    {
        private static readonly BigInteger MAX = BigInteger.Pow(10, Constants.EXPONENT);
        private static readonly BigInteger LOG_BASE_MAX = new BigInteger(double.MaxValue);

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
                if (!(itemBigInt is null)) return CompareTo(itemBigInt);
                else return -1;
            }

            if (other.GetType() == typeof(FGH)) return -1;

            throw new NotImplementedException();
        }

        public override INumber Succ()
        {
            if (n < MAX_VALUE) return new BigInt(n + 1);
            else return new KnuthUpArrow(10, CountDigits(n) - 1, 1);
        }

        public BigInt Prev()
        {
            if (n > MIN_VALUE) return new BigInt(n - 1);
            else throw new ArgumentException("BigInt cannot be smaller than " + MIN_VALUE);
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

        public static BigInt? Sum(BigInt a, BigInt b)
        {
            BigInteger res = BigInteger.Add(a.n, b.n);
            if (IsConvertible(res)) return new BigInt(res);
            else return null;
        }

        public static BigInt? Mul(BigInt a, BigInt b)
        {
            BigInteger res = BigInteger.Multiply(a.n, b.n);
            if (IsConvertible(res)) return new BigInt(res);
            else return null;
        }

        public static BigInt? Pow(BigInt a, int exponent)
        {
            BigInteger res = BigInteger.Pow(a.n, exponent);
            if (IsConvertible(res)) return new BigInt(res);
            else return null;
        }

        public static BigInt? Log(BigInt b, BigInt val)
        {
            //while double is enough
            if (b.N <= LOG_BASE_MAX)
            {
                double baseDouble = (double)b.N;
                return new BigInt((BigInteger)Math.Ceiling(BigInteger.Log(val.N, baseDouble)));
            }
            //otherwise get approximate value - log_a(b) = lg(b) / lg(a)
            else
            {
                BigInteger baseLg = (BigInteger)BigInteger.Log10(b.n);
                BigInteger valLg = (BigInteger)BigInteger.Log10(val.n);
                BigInteger res = BigInteger.Divide(valLg, baseLg);
                if (IsConvertible(res)) return new BigInt(res);
                else return null;
            }
        }

        public static bool operator >=(BigInt a, int b) => a.n >= b;
        public static bool operator <=(BigInt a, int b) => a.n <= b;

        public static bool operator >(BigInt a, int b) => a.n > b;
        public static bool operator <(BigInt a, int b) => a.n < b;

        public static bool operator ==(BigInt a, int b) => a.n == b;
        public static bool operator !=(BigInt a, int b) => a.n != b;
    }
}
