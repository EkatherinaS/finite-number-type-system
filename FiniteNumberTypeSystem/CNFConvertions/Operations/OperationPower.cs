using CNFConvertions.Number;

namespace CNFConvertions.Operations
{
    internal class OperationPower : IOperation
    {
        public OperationPower(IExpression a, IExpression b)
        {
            this.a = a;
            this.b = b;
        }

        public override ResultPair Evaluate()
        {
            throw new NotImplementedException();
        }

        protected override ResultPair EvaluateNumbers(INumber a, INumber b)
        {
            throw new NotImplementedException();
        }
    }
}
