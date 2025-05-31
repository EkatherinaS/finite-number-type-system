using System;
using System.Linq;
using System.Numerics;

namespace CNFConvertions.Number
{
    public abstract class INumber : IExpression, IComparable<INumber>
    {
        public abstract override string ToString();

        public abstract string ToLaTeX();

        public abstract int CompareTo(INumber? other);

        public abstract INumber Succ();

        public bool Equals(INumber? other) => this.CompareTo(other) == 0;

        public override ResultPair Evaluate() => new ResultPair(this, this);

        public static bool operator >=(INumber a, INumber b) => a.CompareTo(b) >= 0;
        public static bool operator <=(INumber a, INumber b) => a.CompareTo(b) <= 0;

        public static bool operator >(INumber a, INumber b) => a.CompareTo(b) > 0;
        public static bool operator <(INumber a, INumber b) => a.CompareTo(b) < 0;

        public static bool operator ==(INumber a, INumber b) => a.CompareTo(b) == 0;
        public static bool operator !=(INumber a, INumber b) => a.CompareTo(b) != 0;
        
        public static int CountDigits(BigInteger value)
        {
            if (value.IsZero || value < 10) return 1;

            int base10Digits = (int)Math.Ceiling(BigInteger.Log10(value));
            var reference = BigInteger.Pow(10, base10Digits);

            if (value >= reference) base10Digits++;

            return base10Digits;
        }

        /*
        Для a^b где a,b <= 10^1000:
        digits(a^b) = b × log10(a) + 1 = b × (digits(a) - 1) + floor(b × log10(mantissa(a))) + 1

        Где этот хвостик от мантиссы можно посчитать через логарифм у double'а который потом кидаем в fixed point арифметику. 
        То, что там будет всего scale=15 цифр точности не очень страшно -- когда они переполнятся, то первое большое слагаемое 
        станет очень большим и заполнит все наши 1000 значимых цифр

        floor(b × log10(mantissa(a)) = b × scaled_mantissa_log / (BigInteger(10^scale))
        scaled_mantissa_log = floor(Math.Log10(leadingDigits(a, scale)) × 10^scale)

        И надо еще разобрать случай когда меньше цифр чем 15 у a.
        */
        public static BigInteger CountDigitsPow(BigInteger a, BigInteger b)
        {
            const int scale = 15;
            if (a.IsZero) return 1;
            if (b.IsZero) return 1;

            //digits(a)
            int digitsA = CountDigits(a);

            if (b == 1) return digitsA;
            if (IsPowerOf10(a)) return b * (BigInteger)Math.Ceiling(BigInteger.Log10(a)) + 1;

            if (digitsA <= scale)
            {
                double logA = Math.Log10((double)a);
                double result = (double)b * logA;
                return (int)Math.Floor(result) + 1;
            }

            //Где этот хвостик от мантиссы можно посчитать через логарифм у double'а который потом кидаем в fixed point арифметику
            string aStr = a.ToString();
            string leadingStr = aStr.Substring(0, scale);
            double leadingDigits = double.Parse(leadingStr);

            //scaled_mantissa_log = floor(Math.Log10(leadingDigits(a, scale)) × 10 ^ scale)
            BigInteger scaledMantissaLog = new BigInteger(Math.Floor(Math.Log10(leadingDigits) * Math.Pow(10, scale)));

            //b × (digits(a) - 1) 
            BigInteger term1 = (b * (digitsA - 1));

            //floor(b × log10(mantissa(a)) = b × scaled_mantissa_log / BigInteger(10 ^ scale)
            BigInteger term2 = (b * scaledMantissaLog) / BigInteger.Pow(10, scale);

            // digits(a ^ b) = b × log10(a) + 1 = b × (digits(a) - 1) + floor(b × log10(mantissa(a)) + 1
            BigInteger totalDigits = term1 + term2 + 1;
            return (int)totalDigits;
        }

        private static bool IsPowerOf10(BigInteger a)
        {
            if (a < 1) return false;
            string s = a.ToString();
            return s[0] == '1' && s.Substring(1).All(c => c == '0');
        }
    }
}