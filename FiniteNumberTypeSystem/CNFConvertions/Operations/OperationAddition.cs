using CNFConvertions.Number;
using System.Numerics;

namespace CNFConvertions.Operations
{
    public class OperationAddition : IOperation
    {
        private IExpression a;
        private IExpression b;

        public OperationAddition(IExpression a, IExpression b)
        {
            this.a = a;
            this.b = b;
        }

        //a - BigInt
        private ResultPair EvaluateNumbers(BigInt a, BigInt b)
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
                BigInt knuthB = new BigInt((int)Math.Floor(BigInteger.Log10(sum) + 1));
                res = new KnuthUpArrow(knuthA, knuthB, 1);
            }

            return new ResultPair(res, res);
        }
        private ResultPair EvaluateNumbers(BigInt a, KnuthUpArrow b)
        {
            BigInt? converted = b.ToBigInt();
            return converted is null ? new ResultPair(b, b) : EvaluateNumbers(a, converted);
        }
        private ResultPair EvaluateNumbers(BigInt a, FGH b) => new ResultPair(b, b);


        //a - KnuthUpArrow
        private ResultPair EvaluateNumbers(KnuthUpArrow a, BigInt b)
        {
            BigInt? converted = a.ToBigInt();
            return converted is null ? new ResultPair(a, a) : EvaluateNumbers(converted, b);
        }
        private ResultPair EvaluateNumbers(KnuthUpArrow a, KnuthUpArrow b)
        {
            if (a.CompareTo(b) < 0)
            {
                KnuthUpArrow upperBound = new KnuthUpArrow(b.A + 1, b.B, b.N);
                return new ResultPair(b, upperBound);
            }
            else
            {
                KnuthUpArrow upperBound = new KnuthUpArrow(a.A + 1, a.B, a.N);
                return new ResultPair(a, upperBound);
            }
        }
        private ResultPair EvaluateNumbers(KnuthUpArrow a, FGH b) => new ResultPair(b, b);


        //a - FGHFunction
        private ResultPair EvaluateNumbers(FGH a, BigInt b) => new ResultPair(a, a);

        private ResultPair EvaluateNumbers(FGH a, KnuthUpArrow b) => new ResultPair(a, a);

        private ResultPair EvaluateNumbers(FGH a, FGH b)
        {
            if (a.CompareTo(b) < 0)
            {
                FGH upperBound = new FGH(b.Alpha, b.N + 1);
                return new ResultPair(b, upperBound);
            }
            else
            {
                FGH upperBound = new FGH(a.Alpha, a.N + 1);
                return new ResultPair(a, upperBound);
            }
        }


        private ResultPair EvaluateNumbers(INumber a, INumber b)
        {
            if (a.GetType() == typeof(BigInt))
            {
                BigInt itemA = (BigInt)a;

                if (b.GetType() == typeof(BigInt))
                {
                    BigInt itemB = (BigInt)b;
                    return EvaluateNumbers(itemA, itemB);
                }
                else if (b.GetType() == typeof(KnuthUpArrow))
                {
                    KnuthUpArrow itemB = (KnuthUpArrow)b;
                    return EvaluateNumbers(itemA, itemB);
                }
                else if (b.GetType() == typeof(FGH))
                {
                    FGH itemB = (FGH)b;
                    return EvaluateNumbers(itemA, itemB);
                }
                else throw new NotImplementedException();
            }
            else if (a.GetType() == typeof(KnuthUpArrow))
            {
                KnuthUpArrow itemA = (KnuthUpArrow)a;

                if (b.GetType() == typeof(BigInt))
                {
                    BigInt itemB = (BigInt)b;
                    return EvaluateNumbers(itemA, itemB);
                }
                else if (b.GetType() == typeof(KnuthUpArrow))
                {
                    KnuthUpArrow itemB = (KnuthUpArrow)b;
                    return EvaluateNumbers(itemA, itemB);
                }
                else if (b.GetType() == typeof(FGH))
                {
                    FGH itemB = (FGH)b;
                    return EvaluateNumbers(itemA, itemB);
                }
                else throw new NotImplementedException();
            }
            else if (a.GetType() == typeof(FGH))
            {
                FGH itemA = (FGH)a;

                if (b.GetType() == typeof(BigInt)) { 
                    BigInt itemB = (BigInt)b;
                    return EvaluateNumbers(itemA, itemB);
                }
                else if (b.GetType() == typeof(KnuthUpArrow)) 
                { 
                    KnuthUpArrow itemB = (KnuthUpArrow)b;
                    return EvaluateNumbers(itemA, itemB);
                }
                else if (b.GetType() == typeof(FGH)) 
                { 
                    FGH itemB = (FGH)b;
                    return EvaluateNumbers(itemA, itemB);
                }
                else throw new NotImplementedException();
            }
            else throw new NotImplementedException();
        }


        public override ResultPair Evaluate()
        {
            ResultPair numberA = a.Evaluate();
            ResultPair numberB = b.Evaluate();

            ResultPair resA = EvaluateNumbers(numberA.LowerBound, numberB.LowerBound);
            ResultPair resB = EvaluateNumbers(numberA.UpperBound, numberB.UpperBound);

            INumber resLowerbound = resA.LowerBound.CompareTo(resB.LowerBound) < 0 ? resA.LowerBound : resB.LowerBound;
            INumber resUpperbound = resA.UpperBound.CompareTo(resB.UpperBound) > 0 ? resA.UpperBound : resB.UpperBound;

            return new ResultPair(resLowerbound, resUpperbound);
        }
    }
}