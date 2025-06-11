using System;
using CNFConvertions.Number;

namespace CNFConvertions.Operations
{
    [System.Serializable]
    public class OperationFGH : IOperation
    {
        // Constructor might be needed if IExpression a, b are used.
        // public OperationFGH(IExpression a, IExpression b)
        // {
        //     this.a = a;
        //     this.b = b;
        // }

        // Evaluate() is inherited from IOperation and should work with the new EvaluateNumbers dispatch.
        // If specific Evaluate logic is needed for OperationFGH beyond what IOperation provides,
        // it can be overridden. For now, we assume IOperation.Evaluate() is sufficient.
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

        // Commutative helpers are virtual in IOperation, so they don't need to be overridden
        // if the default behavior (swapping and calling the corresponding abstract method) is acceptable.
    }
}
