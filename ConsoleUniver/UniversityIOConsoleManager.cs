using System;
using System.Collections.Generic;
using System.Reflection;

namespace ConsoleUniver
{
    public static class UniversityIOConsoleManager
    {
        private static Dictionary<string, string> keyMethods = new Dictionary<string, string>()
        {
            {"STUDENTID", "Номер студенческого билета "},
            {"FIO", "Фамилия Имя Отчество студента "},
            {"BIRTHDAY", "Дата рождения (dd.mm.yyyy) "},
            {"AGE", "Возраст, лет "},
            {"FACULTET", "Факультет "},
            {"GROUP", "Группа "},
            {"SUBJECT", "Предмет "},
            {"BALLS", "Оценки "},
            {"MEDIUM", "Средний балл "},
        };

        private static int university;

        public static void UniversityInit(University university)
        {
            Console.Write("Введите название университета: ");
            university.NameOfUniversity = Console.ReadLine();
            Console.Write("Введите адрес университета: ");
            university.AddressOfUniversity = Console.ReadLine();
            university.Facultets = new List<string>();
            university.Students = new List<Student>();
            Console.Write("Введите названия факультетов через запятую: ");
            string[] facultetsTemp = Console.ReadLine().Split(",");
            foreach (var item in facultetsTemp)
            {
                university.Facultets.Add(item.Trim());
            }
        }

        public static void PrintFacultetsUniversityInfo(University university)
        {
            Console.WriteLine();
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Факультеты на выбор:");
            int i = 0;
            foreach (var item in university.Facultets)
            {
                Console.WriteLine(++i + " - " + item);
            }

            Console.WriteLine("--------------------------------------");
        }

        public static void PrintUniversityInfo(University university)
        {
            Console.WriteLine("--------------------------------------");
            Console.WriteLine($"Университет - {university.NameOfUniversity}");
            Console.WriteLine($"Адрес - {university.AddressOfUniversity}");
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Факультеты:");
            foreach (var item in university.Facultets)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("--------------------------------------");
        }

        public static string newString()
        {
            return Console.ReadLine();
        }

        public static int newInt()
        {
            int n;
            while (!Int32.TryParse(Console.ReadLine(), out n))
                Console.WriteLine("Повторите ввод (целое число)");

            return n;
        }


        public static DateTime newDateTime()
        {
            DateTime d;
            while (!DateTime.TryParse(Console.ReadLine(), out d))
                Console.WriteLine("Повторите ввод (dd.mm.yyyy) ");
            return d;
        }

        public static string newFacultet(University university)
        {
            UniversityIOConsoleManager.PrintFacultetsUniversityInfo(university);
            string str = Console.ReadLine();
            int n = 0;
            while (!(Int32.TryParse(str, out n) && (n <= university.Facultets.Count)))
            {
                Console.WriteLine("Повторите ввод");
                str = Console.ReadLine();
            }

            return n.ToString();
        }

        public static void ExeSetMethod(MethodInfo m, Object obj)
        {
            if (m.Name.Contains("Facultet"))
            {
                Type t = ((object) obj).GetType();

                FieldInfo[] fields = t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                foreach (var field in fields)
                {
                    if (field.Name.Contains("university"))
                    {
                        string str = newFacultet((University) field.GetValue(obj));
                        m.Invoke(obj, new object[] {str});
                        break;
                    }
                }

                return;
            }

            if (m.GetParameters()[0].ParameterType == typeof(int))
            {
                int n = newInt();
                m.Invoke(obj, new object[] {n});
            }

            if (m.GetParameters()[0].ParameterType == typeof(string))
            {
                string str = newString();
                m.Invoke(obj, new object[] {str});
            }

            if (m.GetParameters()[0].ParameterType == typeof(DateTime))
            {
                DateTime d = newDateTime();
                m.Invoke(obj, new object[] {d});
            }
        }

        public static bool StudentInit(University university)
        {
            Console.WriteLine(" ");
            Console.Write("Введите количество студентов к добавлению (0 - не добавлять):");
            int quantityStudents = Convert.ToInt32(Console.ReadLine());
            if (quantityStudents == 0) return false;
            for (int i = 0; i < quantityStudents; i++)
            {
                Student student = new Student(university);
                MethodInfo[] methods = typeof(Student).GetMethods(BindingFlags.Instance | BindingFlags.Public);
                foreach (MethodInfo method in methods)
                {
                    if (IsSetMethod(method.Name))
                    {
                        Console.Write(ConvertField(method.Name) + " : ");
                        ExeSetMethod(method, student);
                    }
                }

                university.Students.Add(student);
                Console.WriteLine("--------------------------------------");
            }

            return true;
        }

        private static string ConvertField(string fieldname)
        {
            string[] names = fieldname.Split('<', '_', '>');

            foreach (var str in names)
            {
                if (keyMethods.ContainsKey(str.ToUpper()))
                    return keyMethods[str.ToUpper()];
            }

            return null;
        }

        private static bool IsGetMethod(string fieldname)
        {
            return (fieldname.Contains("get"));
        }

        private static bool IsSetMethod(string fieldname)
        {
            return (fieldname.Contains("set"));
        }

        public static void PrintStudents(List<Student> Students)
        {
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Список студентов: ");
            Console.WriteLine("--------------------------------------");
            Type tstudent = typeof(Student);

            MethodInfo[] methods = tstudent.GetMethods(BindingFlags.Instance | BindingFlags.Public);


            foreach (var student in Students)
            {
                foreach (MethodInfo method in methods)
                {
                    if (IsGetMethod(method.Name))
                        Console.WriteLine(ConvertField(method.Name) + " : " + method.Invoke(student, null));
                }

                Console.WriteLine("--------------------------------------");
            }
        }

        public static void PrintAndReadTypeRequest(out int key, Type t)
        {
            foreach (var f in Request.RequestIO)
                if (f.Value.types.Contains(t))
                    Console.WriteLine((int) f.Key + " - " + f.Value.Name);

            Console.WriteLine("Выберите поиск ");
            key = 0;
            while (!((Int32.TryParse(Console.ReadLine(), out key)) && (key <= Request.RequestIO.Count)))

                Console.WriteLine("Повторите выбор ( 0 - " + Request.RequestIO.Count + " ) ");
        }

        public static void ReadIntValue(out int value)
        {
            Console.WriteLine("Введите значение для сравнения ");
            value = 0;
            while (!(Int32.TryParse(Console.ReadLine(), out value)))
                Console.WriteLine("Повторите выбор ( 0 - " + Request.RequestIO.Count + " ) ");
        }

        public static void ReadStrValue(out string str)
        {
            Console.WriteLine("Введите строку поиска ");
            str = Console.ReadLine();
        }

        public static void ReadCount(out int count)
        {
            Console.WriteLine("Введите число результатов для вывода ");
            count = 0;
            while (!(Int32.TryParse(Console.ReadLine(), out count)))
                Console.WriteLine("Повторите выбор  ");
        }


        public static bool ReadRequest(Type t, out Request request)
        {
            MethodInfo[] methods = t.GetMethods(BindingFlags.Instance | BindingFlags.Public);

            Console.WriteLine(" ");
            Console.WriteLine("Выберите признак по которому произвести поиск или сортировку : ");
            Console.WriteLine("0 - поиск или сортировка не требуются");
            int i = 0;


            foreach (var method in methods)
            {
                ++i;

                if (IsGetMethod(method.Name))
                    Console.WriteLine($"{i} - {ConvertField(method.Name)}");
            }

            int n;
            while (!(Int32.TryParse(Console.ReadLine(), out n) && (n <= i)))
                Console.WriteLine("Повторите выбор  ");

            if (n == 0)
            {
                //как обойти проблему с созданием параметра out перед возвратом!!!
                request = new Request(null);
                return false;
            }

            Console.WriteLine("Задайте тип поиска и его параметры :");

            request = new Request(methods[n - 1]);
            return true;
        }
    }
}