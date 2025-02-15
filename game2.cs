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

            Console.WriteLine("\n�������� ��������:");
            Console.WriteLine("1. ������� ��� ID ��������� �� �����");
            Console.WriteLine("2. ����� ������� �� ID");
            Console.WriteLine("3. ����� ������� �� �����");
            Console.WriteLine("0. �����");

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
                    Console.WriteLine("�������� �����. ���������� �����.");
                    break;
            }

            Console.WriteLine("\n������� ����� ������� ��� �����������...");
            Console.ReadKey();
        }
    }

    static void DisplayProcesses()
    {
        var currentProcess = Process.GetCurrentProcess();
        var processes = Process.GetProcesses();

        Console.WriteLine("������ ���������� ���������:");
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
        Console.Write("������� ��� �������� (��� ��� �����): ");
        string name = Console.ReadLine();

        try
        {
            var processes = Process.GetProcessesByName(name);
            if (processes.Length == 0)
            {
                Console.WriteLine("�������� � ����� ������ �� �������.");
                return;
            }

            Console.WriteLine("ID ���������:");
            foreach (var process in processes)
            {
                Console.WriteLine($"ID: {process.Id} - {process.ProcessName}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"������: {ex.Message}");
        }
    }

    static void KillProcessById()
    {
        Console.Write("������� ID �������� ��� ����������: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            try
            {
                var process = Process.GetProcessById(id);
                process.Kill();
                process.WaitForExit(); 
                Console.WriteLine($"������� � ID {id} ��� ��������.");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("������� � ����� ID �� ������.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"������: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("�������� ������ ID.");
        }
    }

    static void KillProcessByName()
    {
        Console.Write("������� ��� �������� ��� ����������: ");
        string name = Console.ReadLine();

        try
        {
            var processes = Process.GetProcessesByName(name);
            if (processes.Length == 0)
            {
                Console.WriteLine("�������� � ����� ������ �� �������.");
                return;
            }

            foreach (var process in processes)
            {
                process.Kill();
                process.WaitForExit(); 
                Console.WriteLine($"������� {process.ProcessName} (ID: {process.Id}) ��� ��������.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"������: {ex.Message}");
        }
    }
}
