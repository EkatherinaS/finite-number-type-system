using CNFConvertions.Number;

namespace CNFConvertions.Operations
{
    public class OperationMultiplication : IOperation
    {
        public OperationMultiplication(IExpression a, IExpression b)
        {
            this.a = a;
            this.b = b;
        }

        protected override ResultPair EvaluateNumbers(BigInt a, BigInt b)
        {
            INumber res;

            BigInt? temp = BigInt.Mul(a, b);
            if (temp is null)
            {
                int length = BigInt.CountDigits(a) + BigInt.CountDigits(b) + 1;
                BigInt knuthA = new BigInt(10);
                BigInt knuthB = new BigInt(length);
                res = new KnuthUpArrow(knuthA, knuthB, 1);
            }
            else res = temp;

            return new ResultPair(res, res);
        }

        protected override ResultPair EvaluateNumbers(KnuthUpArrow a, KnuthUpArrow b)
        {
            //LB = (x.a^(y.b / log(y.a, x.a) + x.b)); UB = LB++;
            if (a.N == 1 && b.N == 1)
            {
                BigInt temp = BigInt.Div(a.A, BigInt.Sum(BigInt.Log(b.A, a.A), a.B));
                INumber lb = new KnuthUpArrow(b.A, temp, 1);
                return new ResultPair(lb, lb.Succ());
            }

            //eval as one arrow if possible
            KnuthUpArrow? knuthOneA = a.ToOneArrow();
            KnuthUpArrow? knuthOneB = b.ToOneArrow();
            if (!(knuthOneA is null || knuthOneB is null)) return EvaluateNumbers(knuthOneA, knuthOneB);

            if (a.N <= 2 && b.N <= 2) {
                if (a > b) return new ResultPair(a, a.SuccB());
                else return new ResultPair(b, b.SuccB());
            }

            if (a > b) return new ResultPair(a, a.SuccA());
            else return new ResultPair(b, b.SuccA());
        }

        protected override ResultPair EvaluateNumbers(FGH a, FGH b)
        {
            if (a > b) return new ResultPair(a, a.Succ());
            else return new ResultPair(b, b.Succ());
        }

        protected override ResultPair EvaluateNumbers(BigInt a, KnuthUpArrow b)
        {
            //eval as two BigInt-s if possible
            BigInt? bi = b.ToBigInt();
            if (!(bi is null)) return EvaluateNumbers(a, bi);

            //eval if one arrow
            if (b.N == 1)
            {
                //LB = (y.a^(1/ log(x, y.a) + y.b)); UB = LB++;
                BigInt temp = BigInt.Div(new BigInt(1), BigInt.Sum(BigInt.Log(a, b.A), b.B));
                INumber lb = new KnuthUpArrow(b.A, temp, 1);
                return new ResultPair(lb, lb.Succ());
            }

            //eval as one arrow if possible
            KnuthUpArrow? knuthOne  = b.ToOneArrow();
            if (!(knuthOne is null)) return EvaluateNumbers(a, knuthOne);

            //y .a "+1" в UB
            return new ResultPair(b, b.Succ());
        }

        protected override ResultPair EvaluateNumbers(BigInt a, FGH b) => new ResultPair(b, b.Succ());

        protected override ResultPair EvaluateNumbers(KnuthUpArrow a, FGH b) => new ResultPair(b, b.Succ());
    }
}
