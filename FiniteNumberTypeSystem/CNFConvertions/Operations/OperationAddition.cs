using CNFConvertions.Number;
using System.Numerics;

namespace CNFConvertions.Operations
{
    [System.Serializable]
    public class OperationAddition : IOperation
    {
        public OperationAddition(IExpression a, IExpression b)
        {
            this.a = a;
            this.b = b;
        }

        public override IExpression? Simplify()
        {
            IExpression? simplified = a?.Simplify();
            if (simplified != null) return new OperationAddition(simplified, b);

            simplified = b?.Simplify();
            if (simplified != null) return new OperationAddition(a, simplified);

            return Evaluate().UpperBound;
        }

        public override string ToLatex() => "(" + a.ToLatex() + " + " + b.ToLatex() + ")";
        public override string ToLatexCompressed() => "(" + a.ToLatexCompressed() + " + " + b.ToLatexCompressed() + ")";


        protected override ResultPair EvaluateNumbers(BigInt a, BigInt b)
        {
            INumber res;
            BigInteger sum = a.N + b.N;
            if (BigInt.IsConvertible(sum))
            { 
                res = new BigInt(sum);
            }
            else
            {
                BigInt knuthA = new BigInt(10);
                BigInt knuthB = new BigInt(BigInt.CountDigits(sum));
                res = new KnuthUpArrow(knuthA, knuthB, 1);
            }

            return new ResultPair(res, res, false);
        }

        protected override ResultPair EvaluateNumbers(KnuthUpArrow a, KnuthUpArrow b)
        {
            if (a.CompareTo(b) < 0) return new ResultPair(b, b.Succ(), true);
            else return new ResultPair(a, a.Succ(), true);
        }

        protected override ResultPair EvaluateNumbers(FGH a, FGH b)
        {
            if (a.CompareTo(b) < 0) return new ResultPair(b, b.Succ(), true);
            else return new ResultPair(a, a.Succ(), true);
        }


        protected override ResultPair EvaluateNumbers(BigInt a, KnuthUpArrow b)
        {
            BigInt? converted = b.ToBigInt();
            return converted is null ? new ResultPair(b, b.Succ(), true) : EvaluateNumbers(a, converted);
        }

        protected override ResultPair EvaluateNumbers(BigInt a, FGH b) => new ResultPair(b, b.Succ(), true);

        protected override ResultPair EvaluateNumbers(KnuthUpArrow a, FGH b) => new ResultPair(b, b.Succ(), true);


        // The following are handled by the base class IOperation using the specific overrides above
        // public ResultPair EvaluateNumbers(KnuthUpArrow a, BigInt b) => EvaluateNumbers(b, a);
        // public ResultPair EvaluateNumbers(FGH a, BigInt b) => EvaluateNumbers(b, a);
        // public ResultPair EvaluateNumbers(FGH a, KnuthUpArrow b) => EvaluateNumbers(b, a);
    }
}
