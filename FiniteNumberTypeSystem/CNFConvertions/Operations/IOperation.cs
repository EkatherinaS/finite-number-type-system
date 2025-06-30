using System;
using CNFConvertions.Number;

namespace CNFConvertions.Operations
{
    public abstract class IOperation : IExpression
    {
        protected IExpression? a;
        protected IExpression? b;


        // Specific operation implementations for different number type combinations
        protected abstract ResultPair EvaluateNumbers(BigInt a, BigInt b);
        protected abstract ResultPair EvaluateNumbers(KnuthUpArrow a, KnuthUpArrow b);
        protected abstract ResultPair EvaluateNumbers(FGH a, FGH b);
        protected abstract ResultPair EvaluateNumbers(BigInt a, KnuthUpArrow b);
        protected abstract ResultPair EvaluateNumbers(BigInt a, FGH b);
        protected abstract ResultPair EvaluateNumbers(KnuthUpArrow a, FGH b);
        
        // Helper methods for commutative operations
        protected virtual ResultPair EvaluateNumbers(KnuthUpArrow a, BigInt b) => EvaluateNumbers(b, a);
        protected virtual ResultPair EvaluateNumbers(FGH a, BigInt b) => EvaluateNumbers(b, a);
        protected virtual ResultPair EvaluateNumbers(FGH a, KnuthUpArrow b) => EvaluateNumbers(b, a);

        protected ResultPair EvaluateNumbers(INumber a, INumber b)
        {
            // Dispatch to the correct overloaded method based on runtime types
            if (a is BigInt bigA)
            {
                if (b is BigInt bigB) return EvaluateNumbers(bigA, bigB);
                if (b is KnuthUpArrow knuthB) return EvaluateNumbers(bigA, knuthB);
                if (b is FGH fghB) return EvaluateNumbers(bigA, fghB);
            }
            else if (a is KnuthUpArrow knuthA)
            {
                if (b is BigInt bigB) return EvaluateNumbers(knuthA, bigB); // Uses virtual helper
                if (b is KnuthUpArrow knuthB) return EvaluateNumbers(knuthA, knuthB);
                if (b is FGH fghB) return EvaluateNumbers(knuthA, fghB);
            }
            else if (a is FGH fghA)
            {
                if (b is BigInt bigB) return EvaluateNumbers(fghA, bigB); // Uses virtual helper
                if (b is KnuthUpArrow knuthB) return EvaluateNumbers(fghA, knuthB); // Uses virtual helper
                if (b is FGH fghB) return EvaluateNumbers(fghA, fghB);
            }

            // Fallback or error if types are not handled
            throw new NotImplementedException($"Operation is not implemented for the combination of types: {a.GetType().Name} and {b.GetType().Name}");
        }

        public override ResultPair Evaluate()
        {
            if (a is null || b is null) throw new NotImplementedException();

            ResultPair numberA = a.Evaluate();
            ResultPair numberB = b.Evaluate();

            ResultPair resA = EvaluateNumbers(numberA.LowerBound, numberB.LowerBound);
            ResultPair resB = EvaluateNumbers(numberA.UpperBound, numberB.UpperBound);

            ResultPair maxPair = numberA.UpperBound.CompareTo(numberB.UpperBound) > 0 ? numberA : numberB;

            INumber resLowerbound = resA.LowerBound.CompareTo(resB.LowerBound) < 0 ? resA.LowerBound : resB.LowerBound;
            bool wasIncremented = resA.UpperBound.CompareTo(resB.UpperBound) > 0 ? resA.WasIncremented : resB.WasIncremented;

            INumber resUpperbound;

            if (maxPair.WasIncremented && wasIncremented) 
                resUpperbound = maxPair.UpperBound;
            else 
                resUpperbound = resA.UpperBound.CompareTo(resB.UpperBound) > 0 ? resA.UpperBound : resB.UpperBound;

            return new ResultPair(resLowerbound, resUpperbound, maxPair.WasIncremented && wasIncremented);
        }
    }
}
