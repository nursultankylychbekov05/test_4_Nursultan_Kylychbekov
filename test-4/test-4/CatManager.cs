using System.Text.Json;
namespace test_4;

public class CatManager
{
    private List<Cat> _cats = new List<Cat>();
    private readonly string _filePath = "../../../cats.json";

    public List<Cat> Cats => _cats;

    public delegate void GlobalNotificationHandler(string message);
    public event GlobalNotificationHandler OnNotification;

    public void Load()
    {
        if (!File.Exists(_filePath))
        {
            return;
        }

        try
        {
            string json = File.ReadAllText(_filePath);
            List<Cat> loaded = JsonSerializer.Deserialize<List<Cat>>(json);
            if (loaded != null)
            {
                _cats = loaded;
                for (int i = 0; i < _cats.Count; i++)
                {
                    _cats[i].Initialize();
                    _cats[i].OnStatMaxed += HandleStatMaxed;
                    _cats[i].OnCatDied += HandleCatDied;
                }
            }
        }
        catch
        {
            _cats = new List<Cat>();
        }
    }

    public void Save()
    {
        try
        {
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(_cats, options);
            File.WriteAllText(_filePath, json);
        }
        catch (Exception ex)
        {
            if (OnNotification != null)
            {
                OnNotification.Invoke("Ошибка записи файла: " + ex.Message);
            }
        }
    }

    public void AddCat(Cat cat)
    {
        cat.Initialize();
        cat.OnStatMaxed += HandleStatMaxed;
        cat.OnCatDied += HandleCatDied;
        _cats.Add(cat);
        Save();
    }

    private void HandleStatMaxed(Cat cat, string statName)
    {
        if (OnNotification != null)
        {
            OnNotification.Invoke("[Уведомление] " + cat.Name + " чувствует себя превосходно! Характеристика '" + statName + "' на максимуме (100)!");
        }
    }

    private void HandleCatDied(Cat cat)
    {
        if (OnNotification != null)
        {
            OnNotification.Invoke("[Внимание] Кот " + cat.Name + " погиб... Слишком низкие показатели жизненных сил.");
        }
        _cats.Remove(cat);
        Save();
    }

    public void SortCats()
    {
        CatLifeComparer comparer = new CatLifeComparer();
        _cats.Sort(comparer);
    }
}