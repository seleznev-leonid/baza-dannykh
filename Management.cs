// Management.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StudentBase
{
    public class Management
    {
        private const string FileName = "students.txt";

        // Удаляем поле students, будем всегда работать с файлом напрямую
        public const string CommandAddStudent = "add";
        public const string CommandDeleteStudent = "delete";
        public const string CommandSearchStudent = "search";
        public const string CommandPrintStudent = "print";
        public const string CommandExit = "exit";

        public void Start()
        {
            bool isStart = true;
            string userCommand;

            while (isStart)
            {
                Console.Clear();
                Console.WriteLine("Добро пожаловать в базу студентов НКТС.");
                Console.WriteLine($"для добавления студента введите {CommandAddStudent}");
                Console.WriteLine($"для удаления студента введите {CommandDeleteStudent}");
                Console.WriteLine($"для поиска студента введите {CommandSearchStudent}");
                Console.WriteLine($"для вывода студентов введите {CommandPrintStudent}");
                Console.WriteLine($"для выхода введите {CommandExit}");
                Console.Write("Ввод:");
                userCommand = Console.ReadLine();
                Console.WriteLine();

                switch (userCommand)
                {
                    case CommandAddStudent:
                        AddStudent();
                        break;
                    case CommandDeleteStudent:
                        DeleteStudent();
                        break;
                    case CommandSearchStudent:
                        SearchStudent();
                        break;
                    case CommandPrintStudent:
                        PrintStudents();
                        break;
                    case CommandExit:
                        isStart = false;
                        break;
                    default:
                        Console.WriteLine("Я не знаю такой команды");
                        break;
                }

                if (isStart)
                {
                    Console.WriteLine("Продолжить...");
                    Console.ReadKey();
                }
            }
        }

        private List<Student> LoadStudents()
        {
            try
            {
                if (!File.Exists(FileName)) return new List<Student>();

                return File.ReadAllLines(FileName)
                    .Select(line => Student.FromString(line))
                    .Where(s => s != null)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки: {ex.Message}");
                return new List<Student>();
            }
        }

        private void SaveStudents(List<Student> students)
        {
            try
            {
                File.WriteAllLines(FileName, students.Select(save => save.ToString()));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения: {ex.Message}");
            }
        }

        private void AddStudent()
        {
            var students = LoadStudents();
            var student = new Student();

            Console.Write("Фамилия: ");
            student.LastName = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Имя: ");
            student.FirstName = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Отчество: ");
            student.MiddleName = Console.ReadLine()?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(student.LastName) ||
                string.IsNullOrWhiteSpace(student.FirstName))
            {
                Console.WriteLine("Ошибка: Фамилия и Имя обязательны!");
                return;
            }

            bool exists = students.Any(add =>
                add.LastName.Equals(student.LastName, StringComparison.OrdinalIgnoreCase) &&
                add.FirstName.Equals(student.FirstName, StringComparison.OrdinalIgnoreCase) &&
                add.MiddleName.Equals(student.MiddleName, StringComparison.OrdinalIgnoreCase));

            if (exists)
            {
                Console.WriteLine("Ошибка: Студент уже существует!");
            }
            else
            {
                students.Add(student);
                SaveStudents(students);
                Console.WriteLine("Студент добавлен!");
            }
        }

        private void DeleteStudent()
        {
            var students = LoadStudents();
            Console.Write("Введите фамилию для удаления: ");
            var lastName = Console.ReadLine()?.Trim() ?? "";

            var toRemove = students
                .Where(delet => delet.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (toRemove.Any())
            {
                students = students.Except(toRemove).ToList();
                SaveStudents(students);
                Console.WriteLine($"Удалено: {toRemove.Count}");
            }
            else
            {
                Console.WriteLine("Студенты не найдены");
            }
        }

        private void SearchStudent()
        {
            var students = LoadStudents();
            Console.Write("Введите фамилию или отчество: ");
            var query = Console.ReadLine()?.Trim().ToLower() ?? "";

            var results = students.Where(search =>
                (search.LastName?.ToLower() ?? "").Contains(query) ||
                (search.MiddleName?.ToLower() ?? "").Contains(query)
            ).ToList();

            PrintStudentList(results);
        }

        private void PrintStudents()
        {
            var students = LoadStudents();
            PrintStudentList(students);
        }

        private void PrintStudentList(List<Student> list)
        {
            Console.WriteLine($"Найдено: {list.Count}");
            foreach (var student in list)
            {
                Console.WriteLine($"{student.LastName} {student.FirstName} {student.MiddleName}");
            }
        }
    }
}