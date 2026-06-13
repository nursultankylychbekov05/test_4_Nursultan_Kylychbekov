namespace test_4;

public class CatLifeComparer : IComparer<Cat>
{
    public int Compare(Cat x, Cat y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        double avgX = x.GetAverageLife();
        double avgY = y.GetAverageLife();

        if (avgY > avgX) return 1;
        if (avgY < avgX) return -1;
        return 0;
    }
}