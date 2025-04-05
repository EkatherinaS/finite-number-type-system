namespace CNFConvertions
{
    interface INumber<T> : IComparable<T>
    {
        string ToString();

        T GetMax();

        T GetMin();
    }
}