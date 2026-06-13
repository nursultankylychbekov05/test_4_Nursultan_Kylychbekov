namespace test_4;

public interface ICatAgeState
{
    int GetIncrementStep();
    int GetDecrementStep();
}

public class YoungState : ICatAgeState
{
    public int GetIncrementStep()
    {
        return 10;
    }

    public int GetDecrementStep()
    {
        return 2;
    }
}

public class AdultState : ICatAgeState
{
    public int GetIncrementStep()
    {
        return 5;
    }

    public int GetDecrementStep()
    {
        return 5;
    }
}

public class SeniorState : ICatAgeState
{
    public int GetIncrementStep()
    {
        return 2;
    }

    public int GetDecrementStep()
    {
        return 10;
    }
}