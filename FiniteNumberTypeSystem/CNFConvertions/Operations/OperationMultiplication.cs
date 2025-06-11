using CNFConvertions.Number;

namespace CNFConvertions.Operations
{
    [System.Serializable]
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
                BigInt knuthA = new BigInt(10);
                BigInt knuthB = new BigInt(INumber.CountDigits(a) - 1 + INumber.CountDigits(b) - 1 + 1);
                res = new KnuthUpArrow(knuthA, knuthB, 1);
            }
            else res = temp;

            return new ResultPair(res, res);
        }

        protected override ResultPair EvaluateNumbers(KnuthUpArrow a, KnuthUpArrow b)
        {
            BigInt? ai = a.ToBigInt();
            BigInt? bi = b.ToBigInt();
            if (!(ai is null) && !(bi is null)) return EvaluateNumbers(ai, bi);
            if ((ai is null) && !(bi is null)) return EvaluateNumbers(bi, a);
            if (!(ai is null) && (bi is null)) return EvaluateNumbers(ai, b);

            //LB = (x.a^(y.b * log(x.a, y.a) + x.b)) UB = LB++
            if (a.N == 1 && b.N == 1)
            {
                if (a.A > b.A || (a.A == b.A && a.B < b.B)) return EvaluateNumbers(b, a);
                BigInt temp = BigInt.Sum(BigInt.Mul(b.B, BigInt.Log(a.A, b.A)), a.B);
                INumber lb = new KnuthUpArrow(a.A, temp, 1);
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
                //LB = (x.a^(log(x.a, y) + x.b)) UB = LB++
                BigInt temp = BigInt.Sum(BigInt.Log(b.A, a), b.B);
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
