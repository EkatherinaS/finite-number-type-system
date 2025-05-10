using System.Numerics;
using System.Runtime.CompilerServices;

namespace CNFConvertions.Number
{
    public class KnuthUpArrow : INumber
    {
        private static readonly KnuthUpArrow MAX_VALUE = new KnuthUpArrow(BigInt.GetMax(), BigInt.GetMax(), Constants.ARROW_COUNT);
        private static readonly KnuthUpArrow MIN_VALUE = new KnuthUpArrow(3, 3, 1);
        private static readonly KnuthUpArrow TRITRI_2 = new KnuthUpArrow(new BigInt(3), new BigInt(7625597484987), 2);
        private static readonly KnuthUpArrow TRITRI_3 = new KnuthUpArrow(3, 3, 3);

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

        public KnuthUpArrow(int a, int b, int n)
        {
            if (n < 1 || n > Constants.ARROW_COUNT) throw new ArgumentException("Number of arrows must be a value from the interval 1..1000");
            if (a < 3) throw new ArgumentException("Base must be bigger than 3");
            if (b < 3) throw new ArgumentException("Exponent must be bigger than 3");

            this.a = new BigInt(a);
            this.b = new BigInt(b);
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
            string arrows = " ";
            for (int i = 0; i < n; i++)
            {
                arrows += "\\uparrow ";
            }
            return baseStr + arrows + expStr;
        }

        public static KnuthUpArrow GetMax() => MAX_VALUE;
        public static KnuthUpArrow GetMin() => MIN_VALUE;

        private static KnuthUpArrow? ToOneArrow(KnuthUpArrow twoArrow)
        {
            KnuthUpArrow right = new KnuthUpArrow(twoArrow.A, new BigInt(((BigInteger)twoArrow.B) - 1), 2);
            BigInt? newB = right.ToBigInt();
            if (newB is null) return null;
            else return new KnuthUpArrow(twoArrow.A, newB, 1);
        }

        public override int CompareTo(INumber? other)
        {
            if (other is null) return 0;

            if (other.GetType() == typeof(BigInt)) return -other.CompareTo(this);
            if (other.GetType() == typeof(KnuthUpArrow))
            {
                KnuthUpArrow item = (KnuthUpArrow)other;

                if (n == 1 && item.N > 2) return -1;
                if (n == 2 && item.N > 3) return -1;
                if (n == 3 && item.N > 3) return -1;

                if (n == 1 && item.N == 1)
                {
                    bool isBigIntConvertible = (BigInteger)b * CountDigits(a) <= 1000;
                    bool isOtherBigIntConvertible = (BigInteger)item.B * CountDigits(item.A) <= 1000;

                    if (isBigIntConvertible && !isOtherBigIntConvertible) return -1;
                    if (!isBigIntConvertible && isOtherBigIntConvertible) return 1;

                    int cdpThis = CountDigitsPow(b, a);
                    int cdpOther = CountDigitsPow(item.B, item.A);

                    if (cdpThis == cdpOther && isBigIntConvertible && isOtherBigIntConvertible) return ToBigInt().CompareTo(item.ToBigInt());
                    else return cdpThis.CompareTo(cdpOther);
                }

                if (n == 2 && item.N == 2)
                {
                    KnuthUpArrow? oneArrowThis = ToOneArrow(this);
                    KnuthUpArrow? oneArrowOther = ToOneArrow(item);

                    if (oneArrowThis is not null && oneArrowOther is null) return -1;
                    else if (oneArrowThis is null && oneArrowOther is not null) return 1;
                    else if (oneArrowThis is not null && oneArrowOther is not null) return oneArrowThis.CompareTo(oneArrowOther);
                    else
                    {
                        BigInteger a1, b1, a2, b2;
                        if (b <= item.B)
                        {
                            a1 = a; a2 = item.A;
                            b1 = b; b2 = item.B;
                        }
                        else
                        {
                            a1 = item.A; a2 = a;
                            b1 = item.B; b2 = b;
                        }
                        BigInt leftCmp = new BigInt(a1 * (BigInteger)BigInteger.Log(a1));
                        KnuthUpArrow rightCmp = new KnuthUpArrow(item.A, new BigInt((b2 - b1 + 1) * (BigInteger)BigInteger.Log(a2)), 2);
                        return leftCmp.CompareTo(rightCmp);
                    }
                }

                if (n == 3 && item.N == 3)
                {
                    BigInteger diff = BigInteger.Abs((BigInteger)b - item.B);
                    if (diff == 0) return a.CompareTo(item.A);
                    if (diff >= 2) return b.CompareTo(item.B);
                    else
                    {
                        // x.a^^x.a < y.a -> y
                        
                        KnuthUpArrow x = new KnuthUpArrow(a, a, 2);
                        if (x < item.A) return -1;

                        // y.a^^y.a < x.a -> x
                        
                        KnuthUpArrow y = new KnuthUpArrow(item.A, item.A, 2);
                        if (y < a) return 1;

                        // x.a^^x.a > y.a -> x.a^^x.a в BigInt и сравниваем x.a^^(x.a^^x.a) и y.a^^y.a

                        BigInt? xBigInt = x.ToBigInt();
                        if (x >= item.A && xBigInt is not null)
                        {
                            KnuthUpArrow xBig = new KnuthUpArrow(a, xBigInt, 2);
                            return xBig.CompareTo(y);
                        }

                        // y.a^^y.a > x.a -> y.a^^y.a в BigInt и сравниваем y.a^^(y.a^^y.a) и x.a^^x.a

                        BigInt? yBigInt = y.ToBigInt();
                        if (y >= a && yBigInt is not null)
                        {
                            KnuthUpArrow yBig = new KnuthUpArrow(item.A, yBigInt, 2);
                            return yBig.CompareTo(x);
                        }
                    }
                }


                if (n == 1 && item.N == 2)
                {
                    KnuthUpArrow? oneArrowOther = ToOneArrow(item);
                    if (oneArrowOther is null) return -1;
                    else return CompareTo(oneArrowOther);
                }

                if (n == 2 && item.N == 3)
                {
                    if (TRITRI_3.Equals(item.N)) return CompareTo(TRITRI_2);
                    else return -1;
                }

                if (n < 4 && item.N < 4) return item.CompareTo(this);

                if (b == item.b) return a.CompareTo(item.a);
                return b.CompareTo(item.b);
            }
            if (other.GetType() == typeof(FGH)) return 1;

            throw new NotImplementedException();
        }

        public bool Equals(KnuthUpArrow other) => a == other.a && b == other.b && n == other.n;

        public BigInt? ToBigInt()
        {
            bool toBigInt = false;

            if (n == 1) toBigInt = BigInteger.Multiply(b, (int)Math.Ceiling(BigInteger.Log10(a))) <= 1000;
            if (n == 2) toBigInt = (a <= new BigInt(4)) && (b <= new BigInt(3));

            if (toBigInt) return new BigInt (Arrow(a, b, n));
            else return null;
        }

        public override INumber Succ()
        {
            if (a < BigInt.GetMax()) return new KnuthUpArrow((BigInt)a.Succ(), b, n);
            if (b < BigInt.GetMax())
            {
                if (n < 3) return new KnuthUpArrow(a, (BigInt)b.Succ(), n);
                else return new KnuthUpArrow(new BigInt(3), (BigInt)b.Succ(), n);
            }
            if (n < Constants.ARROW_COUNT)
            {
                //KnuthUpArrow(10^1000, 10^1000, 1) => KnuthUpArrow(3, 5, 2)
                if (n == 1) return new KnuthUpArrow(3, 5, 2);

                //KnuthUpArrow(10^1000, 10^1000, 2) => KnuthUpArrow(3, 4, 3)
                if (n == 2) return new KnuthUpArrow(3, 4, 3);

                return new KnuthUpArrow(3, 3, n + 1);
            }
            return new FGH(n, 3);
        }


        //https://gist.github.com/graninas/358f9c7b80944b7e6a3fe56c6fe09a57
        //https://gist.github.com/javierllaca/59b1e3fe5eaa0d25705d
        static BigInteger Arrow(BigInteger a, BigInteger b, int n)
        {
            if (n == 1) return Enumerable.Repeat(a, (int)b).Aggregate(BigInteger.One, (x, y) => x * y);
            return Enumerable.Repeat(a, (int)b).Aggregate(BigInteger.One, (x, y) => Arrow(y, x, n - 1));
        }
    }
}
