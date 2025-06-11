using System;
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

        // Evaluate() is inherited. Override if specific logic is needed.
        // public override ResultPair Evaluate()
        // {
        //     throw new NotImplementedException();
        // }

        protected override ResultPair EvaluateNumbers(BigInt a, BigInt b)
        {
            throw new NotImplementedException();
        }

        protected override ResultPair EvaluateNumbers(KnuthUpArrow a, KnuthUpArrow b)
        {
            throw new NotImplementedException();
        }

        protected override ResultPair EvaluateNumbers(FGH a, FGH b)
        {
            throw new NotImplementedException();
        }

        protected override ResultPair EvaluateNumbers(BigInt a, KnuthUpArrow b)
        {
            throw new NotImplementedException();
        }

        protected override ResultPair EvaluateNumbers(BigInt a, FGH b)
        {
            throw new NotImplementedException();
        }

        protected override ResultPair EvaluateNumbers(KnuthUpArrow a, FGH b)
        {
            throw new NotImplementedException();
        }
    }
}
