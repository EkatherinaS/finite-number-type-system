using System.Numerics;

namespace CNFConvertions.Number
{
    public class KnuthUpArrow : INumber
    {
        private static readonly KnuthUpArrow MAX_VALUE = new KnuthUpArrow(BigInt.GetMax(), BigInt.GetMax(), Constants.ARROW_COUNT);
        private static readonly KnuthUpArrow MIN_VALUE = new KnuthUpArrow(new BigInt(3), new BigInt(3), 1);

        private BigInt a, b;
        private int n;

        public BigInt A { get { return a; } set { a = value; } }
        public BigInt B { get { return b; } set { b = value; } }
        public int N { get { return n; } set { n = value; } }

        public KnuthUpArrow(BigInt a, BigInt b, int n)
        {
            if (n < 1 || n > Constants.ARROW_COUNT) throw new ArgumentException("Number of arrows must be a value from the interval 1..1000");
            
            if (a < 3) throw new ArgumentException("Base must be bigger than 3");

            if (b < 3) throw new ArgumentException("Exponent must be bigger than 3");

            this.a = a;
            this.b = b;
            this.n = n;
        }

        public override string ToString()
        {
            string baseStr = a.ToString();
            string expStr = b.ToString();
            string arrows = "";
            for (int i = 0; i < n; i++)
            {
                arrows += "↑";
            }
            return baseStr + arrows + expStr;
        }

        public override string ToLaTeX()
        {
            string baseStr = a.ToString();
            string expStr = b.ToString();
            string arrows = "";
            for (int i = 0; i < n; i++)
            {
                arrows += "↑";
            }
            return baseStr + arrows + expStr;
        }

        public static KnuthUpArrow GetMax() => MAX_VALUE;
        public static KnuthUpArrow GetMin() => MIN_VALUE;

        public override int CompareTo(INumber? other)
        {
            if (other == null) return 0;

            if (other.GetType() == typeof(BigInt))
            {
                //TODO: compare BigInt & KnuthUpArrow
                return -1;

            }
            if (other.GetType() == typeof(KnuthUpArrow))
            {
                KnuthUpArrow item = (KnuthUpArrow)other;
                if (n != item.n) { return n.CompareTo(item.n); }

                if (n == 1)
                {
                    BigInt res1 = b * a;
                    BigInt res2 = item.b * item.a;
                    return res1.CompareTo(res2);
                }
                else
                {
                    if (a != item.a) return a.CompareTo(item.a);
                    return b.CompareTo(item.b);
                }
            }
            if (other.GetType() == typeof(FGH))
            {
                //TODO: compare FGHFunction & KnuthUpArrow
                return 1;
            }

            throw new NotImplementedException();
        }

        public bool Equals(KnuthUpArrow other) => a == other.a && b == other.b && n == other.n;


        internal BigInt? ToBigInt()
        {
            bool toBigInt = false;

            if (n == 1)
            {
                toBigInt = BigInteger.Multiply(b, (int)BigInteger.Log10(a)) <= 1000;
            }
            else if (n == 2)
            {
                toBigInt = a <= new BigInt(4) && b <= new BigInt(3);
            }

            if (toBigInt)
            {
                return new BigInt (Arrow(a, b, n));
            }

            return null;
        }

        //https://gist.github.com/javierllaca/59b1e3fe5eaa0d25705d
        static BigInteger Arrow(BigInteger a, BigInteger b, int n)
        {
            if (n == 1)
            {
                return Enumerable.Repeat(a, (int)b).Aggregate(BigInteger.One, (x, y) => x * y);
            }
            return Enumerable.Repeat(a, (int)b).Aggregate(BigInteger.One, (x, y) => Arrow(y, x, n - 1));
        }


        //https://gist.github.com/graninas/358f9c7b80944b7e6a3fe56c6fe09a57

    }
}
