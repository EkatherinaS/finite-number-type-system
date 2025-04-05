using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNFConvertions
{
    internal class BigInt : INumber<BigInt>
    {
        public BigInt(int value) { }

        public override string ToString()
        {
            return "1";
        }

        public int CompareTo(BigInt? other)
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

        public BigInt GetMax()
        {
            return new BigInt(100);
        }

        public BigInt GetMin()
        {
            return new BigInt(0);
        }
    }
}
