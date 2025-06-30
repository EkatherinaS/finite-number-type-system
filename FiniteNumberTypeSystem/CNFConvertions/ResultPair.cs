using CNFConvertions.Number;

namespace CNFConvertions
{
    public class ResultPair
    {
        private INumber lowerbound;
        private INumber upperbound;

        //true - was just incremented (++)
        //false - close to actual result
        private bool wasIncremented;

        public INumber LowerBound { get { return lowerbound; } }
        public INumber UpperBound { get { return upperbound; } }
        public bool WasIncremented { get { return wasIncremented; } }

        public ResultPair(INumber lowerbound, INumber upperbound, bool wasIncremented)
        {
            this.lowerbound = lowerbound;
            this.upperbound = upperbound;
            this.wasIncremented = wasIncremented;
        }
    }
}
