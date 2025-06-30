using System;
using System.Collections.Generic;
using Assets.Scripts.AssemblyMath;


namespace CNFConvertions.Number
{
    public class FGH : INumber
    {
        private CNFOrdinal alpha;
        private BigInt n;

        public CNFOrdinal Alpha { get { return alpha; } set { alpha = value; } }
        public BigInt N { get { return n; } set { n = value; } }

        private static readonly CNFOrdinal ONE = new CNFOrdinal(
            new List<CNFOrdinalTerm>
            {
                new CNFOrdinalTerm(CNFOrdinal.Zero, 1)
            }
        );

        private static readonly CNFOrdinal MIN_ALPHA = new CNFOrdinal(
            new List<CNFOrdinalTerm>
            {
                new CNFOrdinalTerm(CNFOrdinal.Zero, Constants.ARROW_COUNT)
            }
        );

        private static readonly FGH MIN_VALUE = new FGH(MIN_ALPHA, new BigInt(3));


        public FGH(CNFOrdinal alpha, BigInt n)
        {
            if (alpha.CompareTo(MIN_ALPHA) < 0) throw new ArgumentException("Base must be bigger than " + Constants.ARROW_COUNT);
            if (n < 3) throw new ArgumentException("Number must be bigger than 3");
            this.alpha = alpha;
            this.n = n;
        }

        public FGH(int alpha, int n)
        {
            if (alpha.CompareTo(Constants.ARROW_COUNT) < 0) throw new ArgumentException("Base must be bigger than " + Constants.ARROW_COUNT);
            if (n < 3) throw new ArgumentException("Number must be bigger than 3");
            this.alpha = new CNFOrdinal(
                new List<CNFOrdinalTerm>
                {
                    new CNFOrdinalTerm(CNFOrdinal.Zero, alpha)
                }
            );
            this.n = new BigInt(n);
        }

        public override string ToLatex() => "f_{" + alpha.ToLatex() + "}(" + n.ToLatex() + ")";
        public override string ToLatexCompressed() => "f_{" + alpha.ToLatex() + "}(" + n.ToLatexCompressed() + ")";


        public static FGH GetMin() => MIN_VALUE;

        public override string ToString() => "f(" + alpha.ToString() + ", " + n.ToString() + ")";

        public override int CompareTo(INumber? other)
        {
            if (other is null) return 0;

            if (other.GetType() == typeof(BigInt)) return 1;
            if (other.GetType() == typeof(KnuthUpArrow)) return 1;
            if (other.GetType() == typeof(FGH))
            {
                FGH item = (FGH) other;
                if (alpha.Equals(item.alpha)) return n.CompareTo(item.n);
                else return alpha.CompareTo(item.alpha);
            }

            throw new NotImplementedException();
        }

        public override INumber Succ()
        {
            INumber nextN = n.Succ();
            if (nextN.GetType() == typeof(BigInt))
            {
                return new FGH(alpha, (BigInt)nextN);
            }
            else return new FGH(alpha + ONE, new BigInt(3));
        }
    }
}
