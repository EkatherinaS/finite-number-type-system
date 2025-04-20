using CNFConvertions.Number;

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
        private INumber EvaluateNumbers(BigInt a, BigInt b) => a + b;

        private INumber EvaluateNumbers(BigInt a, KnuthUpArrow b)
        {
            //TODO: Add BigInt to KnuthUpArrow
            return b;
        }
        private INumber EvaluateNumbers(BigInt a, FGH b) => b;


        //a - KnuthUpArrow
        private INumber EvaluateNumbers(KnuthUpArrow a, BigInt b)
        {
            return a;
        }

        private INumber EvaluateNumbers(KnuthUpArrow a, KnuthUpArrow b)
        {
            return a;
        }
        private INumber EvaluateNumbers(KnuthUpArrow a, FGH b)
        {
            return b;
        }

        //a - FGHFunction
        private INumber EvaluateNumbers(FGH a, BigInt b) => a;

        private INumber EvaluateNumbers(FGH a, KnuthUpArrow b)
        {
            return a;
        }
        private INumber EvaluateNumbers(FGH a, FGH b)
        {
            return b;
        }

        private INumber EvaluateNumbers(INumber a, INumber b)
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

            INumber resA = EvaluateNumbers(numberA.LowerBound, numberB.LowerBound);
            INumber resB = EvaluateNumbers(numberA.UpperBound, numberB.UpperBound);

            return new ResultPair(resA, resB);
        }
    }
}