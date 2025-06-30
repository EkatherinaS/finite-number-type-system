namespace CNFConvertions
{
    public abstract class IExpression
    {
        public abstract ResultPair Evaluate();

        public abstract IExpression? Simplify();

        public abstract string ToLatex();

        public abstract string ToLatexCompressed();
    }
}
