using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    private static string consoleOutputFilePath = "consoleOutput.txt";
    private static List<Student> students = new List<Student>();

    static void Main(string[] args)
    {
        LoadStudents();
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Добавить студента");
            Console.WriteLine("2. Изменить студента");
            Console.WriteLine("3. Найти студента");
            Console.WriteLine("4. Вывести всех студентов");
            Console.WriteLine("5. Удалить студента");
            Console.WriteLine("0. Выход");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddStudent();
                    break;
                case "2":
                    EditStudent();
                    break;
                case "3":
                    FindStudent();
                    break;
                case "4":
                    DisplayAllStudents();
                    break;
                case "5":
                    DeleteStudent();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Неверный выбор, попробуйте снова.");
                    break;
            }
            Console.ReadKey();
        }
    }

    private static void LoadStudents()
    {
        if (File.Exists(consoleOutputFilePath))
        {
            var lines = File.ReadAllLines(consoleOutputFilePath);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 3)
                {
                    students.Add(new Student(parts[0], parts[1], parts[2]));
                }
            }
        }
    }

    private static void SaveStudents()
    {
        File.WriteAllLines(consoleOutputFilePath, students.Select(s => $"{s.FirstName},{s.LastName},{s.Patronymic}"));
    }

    private static void AddStudent()
    {
        Console.Write("Введите имя: ");
        var firstName = Console.ReadLine();
        Console.Write("Введите фамилию: ");
        var lastName = Console.ReadLine();
        Console.Write("Введите отчество: ");
        var patronymic = Console.ReadLine();

        if (students.Any(s => s.LastName == lastName && s.Patronymic == patronymic))
        {
            Console.WriteLine("Студент есть на базе.");
        }
        else
        {
            students.Add(new Student(firstName, lastName, patronymic));
            SaveStudents();
            Console.WriteLine("Студент добавлен.");
        }
    }

    private static void EditStudent()
    {
        Console.Write("Введите фамилию или отчество студента для редактирования: ");
        var searchTerm = Console.ReadLine();
        var student = students.FirstOrDefault(s => s.LastName == searchTerm || s.Patronymic == searchTerm);

        if (student != null)
        {
            Console.Write("Введите новое имя: ");
            student.FirstName = Console.ReadLine();
            Console.Write("Введите новую фамилию: ");
            student.LastName = Console.ReadLine();
            Console.Write("Введите новое отчество: ");
            student.Patronymic = Console.ReadLine();
            SaveStudents();
            Console.WriteLine("Отредактирован.");
        }
        else
        {
            Console.WriteLine("Студент не найден.");
        }
    }

    private static void FindStudent()
    {
        Console.Write("Введите фамилию или отчество студента для поиска: ");
        var searchTerm = Console.ReadLine();
        var foundStudents = students.Where(s => s.LastName == searchTerm || s.Patronymic == searchTerm).ToList();

        if (foundStudents.Any())
        {
            Console.WriteLine("Найденные студенты:");
            foreach (var student in foundStudents)
            {
                Console.WriteLine(student);
            }
        }
        else
        {
            Console.WriteLine("Студенты не найдены.");
        }
    }

    private static void DisplayAllStudents()
    {
        var sortedStudents = students.OrderBy(s => s.LastName).ToList();
        Console.WriteLine("Список всех студентов:");
        foreach (var student in sortedStudents)
        {
            Console.WriteLine(student);
        }
    }

    private static void DeleteStudent()
    {
        Console.Write("Введите имя студента для удаления: ");
        var firstName = Console.ReadLine();
        Console.Write("Введите фамилию студента для удаления: ");
        var lastName = Console.ReadLine();
        Console.Write("Введите отчество студента для удаления: ");
        var patronymic = Console.ReadLine();

        var studentToRemove = students.FirstOrDefault(s => s.FirstName == firstName && s.LastName == lastName && s.Patronymic == patronymic);
        if (studentToRemove != null)
        {
            students.Remove(studentToRemove);
            SaveStudents();
            Console.WriteLine("Студент удален.");
        }
        else
        {
            Console.WriteLine("Студент не найден.");
        }
    }
}

class Student
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }

    public Student(string firstName, string lastName, string patronymic)
    {
        FirstName = firstName;
        LastName = lastName;
        Patronymic = patronymic;
    }

    public override string ToString()
    {
        return $"{FirstName} {LastName} {Patronymic}";
    }
}
