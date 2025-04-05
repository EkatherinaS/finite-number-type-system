using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CNFConvertions
{
    internal class FGHFunction : INumber<FGHFunction>
    {
        public FGHFunction(int value) { }

        public override string ToString()
        {
            return "f_1(1)";
        }

        public int CompareTo(FGHFunction? other)
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

        public FGHFunction GetMax()
        {
            return new FGHFunction(100);
        }

        public FGHFunction GetMin()
        {
            return new FGHFunction(0);
        }
    }
}
