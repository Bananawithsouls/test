using System;
using System.Diagnostics;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            DisplayProcesses();

            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1. Вывести все ID процессов по имени");
            Console.WriteLine("2. Убить процесс по ID");
            Console.WriteLine("3. Убить процесс по имени");
            Console.WriteLine("0. Выход");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    GetProcessIdsByName();
                    break;
                case "2":
                    KillProcessById();
                    break;
                case "3":
                    KillProcessByName();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }

    static void DisplayProcesses()
    {
        var currentProcess = Process.GetCurrentProcess();
        var processes = Process.GetProcesses();

        Console.WriteLine("Список запущенных процессов:");
        foreach (var process in processes)
        {
            if (process.Id == currentProcess.Id)
                Console.WriteLine($"* {process.ProcessName} (ID: {process.Id})");
            else
                Console.WriteLine($"{process.ProcessName} (ID: {process.Id})");
        }
    }

    static void GetProcessIdsByName()
    {
        Console.Write("Введите имя процесса (или его часть): ");
        string name = Console.ReadLine();

        try
        {
            var processes = Process.GetProcessesByName(name);
            if (processes.Length == 0)
            {
                Console.WriteLine("Процессы с таким именем не найдены.");
                return;
            }

            Console.WriteLine("ID процессов:");
            foreach (var process in processes)
            {
                Console.WriteLine($"ID: {process.Id} - {process.ProcessName}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void KillProcessById()
    {
        Console.Write("Введите ID процесса для завершения: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            try
            {
                var process = Process.GetProcessById(id);
                process.Kill();
                process.WaitForExit(); 
                Console.WriteLine($"Процесс с ID {id} был завершен.");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Процесс с таким ID не найден.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Неверный формат ID.");
        }
    }

    static void KillProcessByName()
    {
        Console.Write("Введите имя процесса для завершения: ");
        string name = Console.ReadLine();

        try
        {
            var processes = Process.GetProcessesByName(name);
            if (processes.Length == 0)
            {
                Console.WriteLine("Процессы с таким именем не найдены.");
                return;
            }

            foreach (var process in processes)
            {
                process.Kill();
                process.WaitForExit(); 
                Console.WriteLine($"Процесс {process.ProcessName} (ID: {process.Id}) был завершен.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}
