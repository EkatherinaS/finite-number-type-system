using CNFConvertions.Number;

namespace CNFConvertions.Operations
{
    public abstract class IOperation : IExpression
    {
        protected IExpression? a;
        protected IExpression? b;

        protected abstract ResultPair EvaluateNumbers(INumber a, INumber b);

        public override ResultPair Evaluate()
        {
            if (a is null || b is null) throw new NotImplementedException();

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
