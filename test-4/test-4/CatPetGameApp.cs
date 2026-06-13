namespace test_4;

public class CatPetGameApp
{
    private readonly CatManager _manager = new CatManager();
    private readonly Random _rand = new Random();

    public void Run()
    {
        _manager.OnNotification += ShowNotification;
        _manager.Load();

        while (true)
        {
            _manager.SortCats();
            DisplayTable();

            Console.WriteLine("\n--- МЕНЮ УПРАВЛЕНИЯ ПИТОМЦАМИ ---");
            Console.WriteLine("1. Добавить нового кота");
            Console.WriteLine("2. Взаимодействие с котом (Кормить / Играть / Лечить)");
            Console.WriteLine("3. Выход из приложения");
            Console.Write("Выберите действие: ");
            string choice = Console.ReadLine();
            
            if (choice == null) choice = "";

            switch (choice)
            {
                case "1":
                    AddNewCat();
                    break;
                case "2":
                    InteractWithCat();
                    break;
                case "3":
                    return;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Неверный ввод. Попробуйте снова.");
                    Console.ResetColor();
                    break;
            }
        }
    }

    private void ShowNotification(string message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n" + message);
        Console.ResetColor();
    }

    private void DisplayTable()
    {
        Console.WriteLine("\n========================================================================================");
        Console.WriteLine("| №  | Имя             | Возраст | Сытость | Настроение | Здоровье | Ср. Уровень Жизни |");
        Console.WriteLine("========================================================================================");

        if (_manager.Cats.Count == 0)
        {
            Console.WriteLine("|                      Список пуст. Заведите своего первого питомца!                   |");
        }
        else
        {
            for (int i = 0; i < _manager.Cats.Count; i++)
            {
                Cat cat = _manager.Cats[i];
                Console.WriteLine("| " + (i + 1).ToString().PadRight(2) + " | " + cat.Name.PadRight(15) + " | " + cat.Age.ToString().PadRight(7) + " | " + cat.Satiety.ToString().PadRight(7) + " | " + cat.Mood.ToString().PadRight(10) + " | " + cat.Health.ToString().PadRight(8) + " | " + cat.GetAverageLife().ToString("F2").PadRight(17) + " |");
            }
        }
        Console.WriteLine("========================================================================================");
    }

    private void AddNewCat()
    {
        try
        {
            Console.Write("Введите имя питомца: ");
            string name = Console.ReadLine();
            if (name == null) name = "";
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception("Имя не может состоять из пробелов или быть пустым.");
            }

            Console.Write("Введите возраст питомца: ");
            string ageStr = Console.ReadLine();
            if (ageStr == null) ageStr = "";

            int age = Convert.ToInt32(ageStr);
            if (age < 0)
            {
                throw new Exception("Возраст не может быть отрицательным числом.");
            }
            if (age > 30) 
            {
                throw new Exception("Коты столько не живут :( Введите реалистичный возраст (от 0 до 30 лет).");
            }

            Cat newCat = new Cat
            {
                Name = name,
                Age = age,
                Satiety = 50,
                Mood = 50,
                Health = 50
            };

            _manager.AddCat(newCat);
            Console.WriteLine("Питомец " + name + " успешно добавлен в систему!");
        }
        catch (FormatException)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ошибка ввода: возраст должен быть целым числом.");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ошибка: " + ex.Message);
            Console.ResetColor();
        }
    }

    private void InteractWithCat()
    {
        if (_manager.Cats.Count == 0)
        {
            Console.WriteLine("Список пуст. Не с кем взаимодействовать.");
            return;
        }

        try
        {
            Console.Write("Введите номер кота из таблицы: ");
            string indexStr = Console.ReadLine();
            if (indexStr == null) indexStr = "";

            int index = Convert.ToInt32(indexStr);
            if (index < 1 || index > _manager.Cats.Count)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Кот под таким номером отсутствует.");
                Console.ResetColor();
                return;
            }

            Cat selectedCat = _manager.Cats[index - 1];

            Console.WriteLine("\nВыбран питомец: " + selectedCat.Name);
            Console.WriteLine("1. Покормить");
            Console.WriteLine("2. Поиграть");
            Console.WriteLine("3. Лечить");
            Console.Write("Выберите действие: ");

            string action = Console.ReadLine();
            if (action == null) action = "";

            switch (action)
            {
                case "1":
                    selectedCat.Feed(_rand);
                    _manager.Save();
                    break;
                case "2":
                    selectedCat.Play(_rand);
                    _manager.Save();
                    break;
                case "3":
                    selectedCat.Heal(_rand);
                    _manager.Save();
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Неизвестная команда.");
                    Console.ResetColor();
                    break;
            }
        }
        catch (FormatException)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ошибка ввода: номер кота должен быть целым числом.");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ошибка выполнения: " + ex.Message);
            Console.ResetColor();
        }
    }
}