namespace CNFConvertions
{
    [System.Serializable]
    public abstract class IExpression
    {
        public abstract ResultPair Evaluate();

        //TODO: ToLatex
    }
}
