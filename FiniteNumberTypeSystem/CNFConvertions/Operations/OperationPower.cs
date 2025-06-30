using System.Numerics;
using CNFConvertions.Number;

namespace CNFConvertions.Operations
{
    [System.Serializable]
    public class OperationPower : IOperation
    {
        public OperationPower(IExpression a, IExpression b)
        {
            this.a = a;
            this.b = b;
        }

        public override IExpression? Simplify()
        {
            IExpression? simplified = a?.Simplify();
            if (simplified != null) return new OperationPower(simplified, b);

            simplified = b?.Simplify();
            if (simplified != null) return new OperationPower(a, simplified);

            return Evaluate().UpperBound;
        }

        public override string ToLatex() => "(" + a.ToLatex() + "^" + b.ToLatex() + ")";
        public override string ToLatexCompressed() => "(" + a.ToLatexCompressed() + "^" + b.ToLatexCompressed() + ")";

        // Evaluate() is inherited. Override if specific logic is needed.
        // public override ResultPair Evaluate()
        // {
        //     throw new NotImplementedException();
        // }

        protected override ResultPair EvaluateNumbers(BigInt a, BigInt b)
        {
            if (a == 1) return new ResultPair(new BigInt(1), new BigInt(1), false);
            if (a == 2 && b > 3318)
            {
                //Approximation for b*log_3(2)
                BigInt bFixed = new BigInt(b.N * 20599 / 32635);
                KnuthUpArrow knuth = new KnuthUpArrow(new BigInt(3), bFixed, 1);
                return new ResultPair(knuth, knuth, false);
            }
            //3318 - max degree that'll fit in BigInt -> 2^3318 < 10^1000 and 2^3319 > 10^1000
            if (b <= 3318)
            {
                BigInteger res = BigInteger.Pow(a.N, (int)b.N);
                if (BigInt.IsConvertible(res)) return new ResultPair(new BigInt(res), new BigInt(res), false);
            }
            return new ResultPair(new KnuthUpArrow(a, b, 1), new KnuthUpArrow(a, b, 1), false);

        }

        protected override ResultPair EvaluateNumbers(KnuthUpArrow a, KnuthUpArrow b)
        {
            if (a.N == 1 && b.N < 3)
            {
                OperationMultiplication opMul = new OperationMultiplication(a.B, b);
                ResultPair pMul = opMul.Evaluate();

                OperationPower opPowLB = new OperationPower(a.A, pMul.LowerBound);
                OperationPower opPowUB = new OperationPower(a.A, pMul.UpperBound);

                ResultPair pLB = opPowLB.Evaluate();
                ResultPair pUB = opPowUB.Evaluate();

                INumber lb = pLB.LowerBound < pUB.LowerBound ? pLB.LowerBound : pUB.LowerBound;
                INumber ub = pLB.UpperBound > pUB.UpperBound ? pLB.UpperBound : pUB.UpperBound;

                return new ResultPair(lb, ub, pUB.WasIncremented);
            }
            if (a.N == 2 && b.N < 3)
            {
                KnuthUpArrow knuth = new KnuthUpArrow(a.A, a.B, a.N - 1);
                OperationMultiplication opMul = new OperationMultiplication(knuth, b);
                ResultPair pMul = opMul.Evaluate();

                OperationPower opPowLB = new OperationPower(a.A, pMul.LowerBound);
                OperationPower opPowUB = new OperationPower(a.A, pMul.UpperBound);

                ResultPair pLB = opPowLB.Evaluate();
                ResultPair pUB = opPowUB.Evaluate();

                INumber lb = pLB.LowerBound < pUB.LowerBound ? pLB.LowerBound : pUB.LowerBound;
                INumber ub = pLB.UpperBound > pUB.UpperBound ? pLB.UpperBound : pUB.UpperBound;

                return new ResultPair(lb, ub, pUB.WasIncremented);
            }

            KnuthUpArrow min = a < b ? a : b;
            KnuthUpArrow max = a > b ? a : b;

            BigInt? minBI = min.ToBigInt();
            BigInt? maxBI = max.ToBigInt();

            if (minBI is null) return new ResultPair(max, max.SuccB(), true);
            if (maxBI is null) return EvaluateNumbers(minBI, max);
            return EvaluateNumbers(minBI, maxBI);
        }

        protected override ResultPair EvaluateNumbers(KnuthUpArrow a, BigInt b)
        {
            BigInt? aInt = a.ToBigInt();
            if (!(aInt is null)) return EvaluateNumbers(aInt, b);

            if (a.N == 1)
            {
                //bigint * bigint => resultpair(res, res) -- lb ub are same
                OperationMultiplication opMul = new OperationMultiplication(a.B, b);
                ResultPair pMul = opMul.Evaluate();

                return EvaluateNumbers(a.A, pMul.LowerBound);
            }

            if (a.N == 2)
            {
                KnuthUpArrow smaller = new KnuthUpArrow(a.A, a.B, 1);
                OperationMultiplication opMul = new OperationMultiplication(smaller, b);
                ResultPair pMul = opMul.Evaluate();

                OperationPower opPowLB = new OperationPower(a.A, pMul.LowerBound);
                OperationPower opPowUB = new OperationPower(a.A, pMul.UpperBound);

                ResultPair pLB = opPowLB.Evaluate();
                ResultPair pUB = opPowUB.Evaluate();

                INumber lb = pLB.LowerBound < pUB.LowerBound ? pLB.LowerBound : pUB.UpperBound;
                INumber ub = pLB.UpperBound > pUB.UpperBound ? pLB.UpperBound : pUB.UpperBound;

                return new ResultPair(lb, ub, pUB.WasIncremented);
            }

            return new ResultPair(a, a.SuccA(), true);
        }


        protected override ResultPair EvaluateNumbers(BigInt a, KnuthUpArrow b)
        {
            BigInt? bInt = b.ToBigInt();
            if (!(bInt is null)) return EvaluateNumbers(a, bInt);

            if (b.N == 1)
            {
                //UB: r = ceil(log_2(log_2(x)))
                BigInt r;
                BigInt two = new BigInt(2);
                BigInt? rTemp = BigInt.Log(two, a);
                if (!(rTemp is null)) rTemp = BigInt.Log(two, rTemp);
                if (rTemp is null || rTemp == new BigInt(1)) r = new BigInt(2);
                else r = rTemp;

                BigInteger min, max;
                if (b.A.CompareTo(b.B) > 0) { min = b.B; max = b.A; } 
                else { min = b.A; max = b.B; }
                min -= r;

                BigInteger kLb = INumber.BinarySearch(min, max, (BigInteger x) => {
                    if (x - r < 3 || x < 3) return false;
                    else return b.CompareTo(new KnuthUpArrow(new BigInt(x), new BigInt(x - 1), 1)) <= 0;
                    }
                ) - 1;

                BigInteger kUb = INumber.BinarySearch(min, max, (BigInteger x) => {
                    if (x - r < 3 || x < 3) return false;
                    else return b.CompareTo(new KnuthUpArrow(new BigInt(x), new BigInt(x - r), 1)) <= 0;
                    }
                );

                KnuthUpArrow lb = new KnuthUpArrow(new BigInt(kLb), new BigInt(3), 2);
                KnuthUpArrow ub = new KnuthUpArrow(new BigInt(kUb), new BigInt(3), 2);
                return new ResultPair(lb, ub, true);
            }

            if (b.N == 2)
            {
                int k;
                if (a.CompareTo(b.A) < 0) k = 0;
                else if (a.CompareTo(new KnuthUpArrow(b.A, b.A, 1)) < 0) k = 1;
                else if (a.CompareTo(new KnuthUpArrow(b.A, new BigInt(3), 2)) < 0) k = 2;
                else if (a.CompareTo(new KnuthUpArrow(b.A, new BigInt(4), 2)) < 0) k = 3;
                else k = 4;

                KnuthUpArrow lb, ub;

                BigInt? lbTemp = k == 0 ? b.B : BigInt.Sum(b.B, new BigInt(k));
                if (lbTemp is null) return new ResultPair(new KnuthUpArrow(4, 3, 3), new KnuthUpArrow(5, 3, 3), false);
                else lb = new KnuthUpArrow(b.A, lbTemp, 2);

                BigInt? ubTemp = BigInt.Sum(lbTemp, new BigInt(1));
                if (ubTemp is null) return new ResultPair(lb, new KnuthUpArrow(4, 3, 3), false);
                else ub = new KnuthUpArrow(b.A, ubTemp, 2);

                return new ResultPair(lb, ub, false);
            }

            return new ResultPair(b, b.SuccA(), true);
        }

        protected override ResultPair EvaluateNumbers(FGH a, FGH b)
        {
            if (a > b) return new ResultPair(a, a.Succ(), true);
            else return new ResultPair(b, b.Succ(), true);
        }

        protected override ResultPair EvaluateNumbers(BigInt a, FGH b) => new ResultPair(b, b.Succ(), true);

        protected override ResultPair EvaluateNumbers(KnuthUpArrow a, FGH b) => new ResultPair(b, b.Succ(), true);
    }
}