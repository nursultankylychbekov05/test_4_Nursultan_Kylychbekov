namespace test_4;

public class Cat
{
    public string Name { get; set; }
    private int _age;
    
    public int Age
    {
        get { return _age; }
        set { _age = value; UpdateState(); }
    }
    
    public int Satiety { get; set; }
    public int Mood { get; set; }
    public int Health { get; set; }

    private ICatAgeState _state;
    
    public delegate void StatMaxedHandler(Cat cat, string statName);
    public event StatMaxedHandler OnStatMaxed;

    public delegate void CatDiedHandler(Cat cat);
    public event CatDiedHandler OnCatDied;

    public Cat()
    {
    }

    public void Initialize()
    {
        UpdateState();
    }

    private void UpdateState()
    {
        if (_age <= 5)
        {
            _state = new YoungState();
        }
        else if (_age <= 10)
        {
            _state = new AdultState();
        }
        else
        {
            _state = new SeniorState();
        }
    }

    public double GetAverageLife()
    {
        return (Satiety + Mood + Health) / 3.0;
    }

    private void ChangeStats(int satDiff, int moodDiff, int healDiff)
    {
        Satiety += satDiff;
        Mood += moodDiff;
        Health += healDiff;
        
        if (Satiety > 100)
        {
            Satiety = 100;
            if (OnStatMaxed != null) OnStatMaxed.Invoke(this, "Сытость");
        }
        if (Mood > 100)
        {
            Mood = 100;
            if (OnStatMaxed != null) OnStatMaxed.Invoke(this, "Настроение");
        }
        if (Health > 100)
        {
            Health = 100;
            if (OnStatMaxed != null) OnStatMaxed.Invoke(this, "Здоровье");
        }
        
        if (Satiety <= 0 || Mood <= 0 || Health <= 0)
        {
            if (OnCatDied != null) OnCatDied.Invoke(this);
        }
    }

    public void Feed(Random rand)
    {
        if (rand.Next(1, 101) <= 15)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n[Событие] {Name} съел несвежую рыбку и отравился!");
            Console.ResetColor();
            ChangeStats(0, -15, -15);
            return;
        }

        int inc = _state.GetIncrementStep();
        int dec = _state.GetDecrementStep();
        ChangeStats(inc, inc, -dec);
    }

    public void Play(Random rand)
    {
        if (rand.Next(1, 101) <= 15)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n[Событие] {Name} слишком сильно разыгрался и порезал лапку!");
            Console.ResetColor();
            ChangeStats(-_state.GetDecrementStep(), -10, -15);
            return;
        }

        int inc = _state.GetIncrementStep();
        int dec = _state.GetDecrementStep();
        ChangeStats(-dec, inc, -dec);
    }

    public void Heal(Random rand)
    {
        if (rand.Next(1, 101) <= 15)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n[Событие] У {Name} началась аллергическая реакция на укол!");
            Console.ResetColor();
            ChangeStats(0, -20, -20);
            return;
        }

        int inc = _state.GetIncrementStep();
        int dec = _state.GetDecrementStep();
        ChangeStats(0, -dec, inc);
    }
}