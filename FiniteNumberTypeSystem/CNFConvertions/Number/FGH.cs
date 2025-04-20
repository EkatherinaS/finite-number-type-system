using Assets.Scripts.AssemblyMath;


namespace CNFConvertions.Number
{
    public class FGH : INumber
    {
        private CNFOrdinal alpha;
        private BigInt n;

        private static readonly CNFOrdinal MIN_ALPHA = new CNFOrdinal(
            new List<CNFOrdinalTerm>
            {
                new CNFOrdinalTerm(CNFOrdinal.Zero, Constants.ARROW_COUNT)
            }
        );

        private static readonly FGH MIN_VALUE = new FGH(MIN_ALPHA, new BigInt(3));

        public FGH(CNFOrdinal alpha, BigInt n)
        {
            if (alpha.CompareTo(MIN_ALPHA) < 0)
            {
                throw new ArgumentException("Base must be bigger than " + Constants.ARROW_COUNT);
            }
            if (n < 3)
            {
                throw new ArgumentException("Number must be bigger than 3");
            }
            this.alpha = alpha;
            this.n = n;
        }
        public CNFOrdinal Alpha { get { return alpha; } }
        public BigInt N { get { return n; } }

        public bool Equals(FGH other) => alpha.Equals(other.alpha) && n.Equals(other.n);

        public static FGH GetMin() => MIN_VALUE;

        public override string ToString() => "f(" + alpha.ToString() + ", " + n.ToString() + ")";

        public override string ToLaTeX() => "f_{" + alpha.ToLaTeX() + "}(" + n.ToLaTeX() + ")";

        public override int CompareTo(INumber? other)
        {
            if (other == null) return 0;

            if (other.GetType() == typeof(BigInt))
            {
                return -1;
            }
            if (other.GetType() == typeof(KnuthUpArrow))
            {
                //TODO: compare FGHFunction & KnuthUpArrow
                return -1;
            }
            if (other.GetType() == typeof(FGH))
            {
                FGH item = (FGH) other;
                if (alpha.Equals(item.alpha))
                {
                    return n.CompareTo(item.n);
                }
                return alpha.CompareTo(item.alpha);
            }

            throw new NotImplementedException();
        }
    }
}
