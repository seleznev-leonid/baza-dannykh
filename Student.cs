// Student.cs
using System;

namespace StudentBase
{
    public class Student
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }

        public override string ToString()
        {
            return $"{LastName},{FirstName},{MiddleName}";
        }

        // Student.cs
        // Student.cs
        public static Student FromString(string data)
        {
            try
            {
                // Исправленная строка: правильное использование Split
                var parts = data.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length < 3)
                {
                    Console.WriteLine($"Ошибка в строке: {data} - недостаточно данных");
                    return null;
                }

                return new Student
                {
                    LastName = parts[0].Trim(),
                    FirstName = parts[1].Trim(),
                    MiddleName = parts[2].Trim()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка преобразования: {ex.Message}");
                return null;
            }
        }
    }
}