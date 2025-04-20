using CNFConvertions.Number;

namespace CNFConvertions
{
    public class ResultPair
    {
        private INumber lowerbound;
        private INumber upperbound;

        public INumber LowerBound { get { return lowerbound; } }
        public INumber UpperBound { get { return upperbound; } }

        public ResultPair(INumber lowerbound, INumber upperbound)
        {
            this.lowerbound = lowerbound;
            this.upperbound = upperbound;
        }
    }
}
