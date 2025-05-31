using Assets.Scripts.AssemblyMath;

namespace CNFTestsNamespace
{
    internal class CNFHelper
    {
        public static CNFOrdinal GetOrdinal(int n) => new CNFOrdinal(
            new List<CNFOrdinalTerm>
            {
                new CNFOrdinalTerm(CNFOrdinal.Zero, n)
            }
        );
    }
}
