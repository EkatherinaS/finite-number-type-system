using System;
using CNFConvertions.Number;
using System.Numerics;

namespace CNFConvertions.Operations
{
    public class OperationMultiplication : IOperation
    {
        public OperationMultiplication(IExpression a, IExpression b)
        {
            this.a = a;
            this.b = b;
        }

        // Evaluate() is inherited. Override if specific logic is needed.
        // public override ResultPair Evaluate()
        // {
        //     throw new NotImplementedException();
        // }

        protected override ResultPair EvaluateNumbers(BigInt a, BigInt b)
        {
            INumber res;
            int length = BigInt.CountDigits(a) + BigInt.CountDigits(b) + 1;
            if (length <= Constants.ARROW_COUNT)
            {
                res = new BigInt(BigInteger.Multiply(a, b));
            }
            else
            {
                BigInt knuthA = new BigInt(10);
                BigInt knuthB = new BigInt(length);
                res = new KnuthUpArrow(knuthA, knuthB, 1);
            }
            return new ResultPair(res, res);
        }

        protected override ResultPair EvaluateNumbers(KnuthUpArrow a, KnuthUpArrow b)
        {
            INumber upperbound, lowerbound;
            if (a.N == 1 && b.N == 1)
            {
                OperationAddition op = new OperationAddition(a.A, b.A);
                INumber newB = op.Evaluate().UpperBound;

                //INumber newB = a.A ^ (b.B / BigInteger.Log(a.A, (double)b.A) + a.B);
                if (newB.GetType() == typeof(BigInt))
                {
                    if (a.A < b.A)
                    {
                        lowerbound = new KnuthUpArrow(a.A, (BigInt)newB, a.N);
                        upperbound = new KnuthUpArrow(b.A, (BigInt)newB, a.N);
                    }
                    else
                    {
                        lowerbound = new KnuthUpArrow(a.A, (BigInt)newB, a.N);
                        upperbound = new KnuthUpArrow(b.A, (BigInt)newB, a.N);
                    }
                }
                else
                {
                    lowerbound = new KnuthUpArrow(b.A, BigInt.GetMax(), a.N);
                    upperbound = new KnuthUpArrow(3, 3, a.N + 1);
                }
                return new ResultPair(lowerbound, upperbound);
            }
            else if (a.N == 1 && b.N == 2)
            {
                KnuthUpArrow? oneArrowB = KnuthUpArrow.ToOneArrow(b);
                //TODO: 1 arrow & 2 arrow multiplication
                if (oneArrowB is null) throw new NotImplementedException();
                else return EvaluateNumbers(a, oneArrowB);
            }
            else if (a.N == 2 && b.N == 1) return EvaluateNumbers(b, a);
            else if (a.N == 2 && b.N == 2)
            {
                //TODO: 2 arrow & 2 arrow multiplication
                throw new NotImplementedException();
            }
            //TODO: 2+ arrow multiplication
            else throw new NotImplementedException();
        }

        protected override ResultPair EvaluateNumbers(FGH a, FGH b)
        {
            if (a > b) return new ResultPair(a, a.Succ());
            else return new ResultPair(b, b.Succ());
        }

        protected override ResultPair EvaluateNumbers(BigInt a, KnuthUpArrow b)
        {
            if (b.N > 2) return new ResultPair(b, b.Succ());

            BigInt? bigIntB = b.ToBigInt();
            if (bigIntB != null) return EvaluateNumbers(bigIntB, a);

            if (b.N == 1 && a < b.A) return new ResultPair(b, b.Succ());
            if (b.N == 1 && a >= b.A) return new ResultPair(b.Succ(), b.Succ().Succ());

            INumber nextA = b.A.Succ();
            if (nextA.GetType() == typeof(BigInt)) return new ResultPair(b, b.Succ());

            KnuthUpArrow cmp = new KnuthUpArrow((BigInt)nextA, b.B.Prev(), 2);
            if (a > cmp) return new ResultPair(b.Succ(), b.Succ().Succ());
            else return new ResultPair(b, b.Succ());
        }

        protected override ResultPair EvaluateNumbers(BigInt a, FGH b) => new ResultPair(b, b.Succ());

        protected override ResultPair EvaluateNumbers(KnuthUpArrow a, FGH b) => new ResultPair(b, b.Succ());
    }
}
