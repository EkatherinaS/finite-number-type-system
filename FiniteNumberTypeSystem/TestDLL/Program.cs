using CNFConvertions;
using CNFConvertions.Number;
using CNFConvertions.Operations;
using Assets.Scripts.AssemblyMath;

public class Program
{
    private static CNFOrdinal GetOrdinal(int n) => new CNFOrdinal(
        new List<CNFOrdinalTerm>
        {
                new CNFOrdinalTerm(CNFOrdinal.Zero, n)
        }
    );

    static void ShowOperation(INumber a, INumber b, ResultPair p, string op)
    {
        Console.WriteLine(a.ToString() + " " + op + " " + b.ToString() + " = (" + p.LowerBound + ", " + p.UpperBound + ")");
    }

    static void Main(string[] args)
    {
        BigInt a = new BigInt(1);
        BigInt b = new BigInt(2);
        BigInt c = new BigInt(3);
        BigInt d = new BigInt(4);
        BigInt e = new BigInt(7);

        Console.WriteLine("Numbers:");
        Console.WriteLine(a.ToString());
        Console.WriteLine(b.ToString());
        Console.WriteLine(c.ToString());
        Console.WriteLine(d.ToString());
        Console.WriteLine(e.ToString());

        OperationAddition op1 = new OperationAddition(a, b);
        ResultPair p1 = op1.Evaluate();

        ShowOperation(a, b, p1, "+");
        Console.WriteLine(p1.LowerBound.Equals(c));
        Console.WriteLine(p1.UpperBound.Equals(c));

        OperationAddition op2 = new OperationAddition(a, c);
        ResultPair p2 = op2.Evaluate();

        ShowOperation(a, c, p2, "+");
        Console.WriteLine(p2.LowerBound.Equals(d));
        Console.WriteLine(p2.UpperBound.Equals(d));

        OperationAddition op = new OperationAddition(op1, op2);
        ResultPair p = op.Evaluate();

        Console.WriteLine("Small tree:");
        Console.WriteLine(p.LowerBound.Equals(e));
        Console.WriteLine(p.UpperBound.Equals(e));

        KnuthUpArrow knuth = new KnuthUpArrow(d, e, 4);
        Console.WriteLine("Knuth's Up Arrow: " + knuth.ToString());
        Console.WriteLine("Knuth's Up Arrow LaTeX: " + knuth.ToLaTeX());

        CNFOrdinal ord = GetOrdinal(4200);
        FGH fgh = new FGH(ord, e);
        Console.WriteLine("FGH: " + fgh.ToString());
        Console.WriteLine("FGH LaTeX: " + fgh.ToLaTeX());
    }
}
