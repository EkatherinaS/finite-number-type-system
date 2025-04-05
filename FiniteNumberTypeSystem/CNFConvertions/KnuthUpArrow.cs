using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNFConvertions
{
    internal class KnuthUpArrow : INumber<KnuthUpArrow>
    {
        public KnuthUpArrow(int value) { }

        public override string ToString()
        {
            return "1↑↑1";
        }

        public int CompareTo(KnuthUpArrow? other)
        {
            return 0;
        }

        public override bool Equals(object? obj)
        {
            return false;
        }

        public override int GetHashCode()
        {
            return 1;
        }
        public KnuthUpArrow GetMax()
        {
            return new KnuthUpArrow(100);
        }

        public KnuthUpArrow GetMin()
        {
            return new KnuthUpArrow(0);
        }
    }
}
