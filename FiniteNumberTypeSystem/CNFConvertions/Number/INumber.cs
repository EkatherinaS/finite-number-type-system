namespace CNFConvertions.Number
{
    public abstract class INumber : IExpression, IComparable<INumber>
    {
        public abstract override string ToString();

        public abstract string ToLaTeX();

        public abstract int CompareTo(INumber? other);

        public override ResultPair Evaluate() => new ResultPair(this, this);
    }
}