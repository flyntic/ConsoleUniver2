using System;
using System.Collections.Generic;

namespace ConsoleUniver
{
    public class University
    {
        public string NameOfUniversity { get; set; }
        public string AddressOfUniversity { get; set; }
        public List<string> Facultets { get; set; }
        public List<Student> Students { get; set; }

        public University()
        {
            UniversityIOConsoleManager.UniversityInit(this);
        }

        public void PrintInfo()
        {
            UniversityIOConsoleManager.PrintUniversityInfo(this);
            UniversityIOConsoleManager.PrintStudents(this.Students);
        }

        public bool StudentsInit()
        {
            return UniversityIOConsoleManager.StudentInit(this);
        }

        public bool RequestStudentField()
        {
            Type studentType = typeof(Student);
            Request check;
            if (!UniversityIOConsoleManager.ReadRequest(studentType, out check))
                return false;

            List<Student> OutStudents;
            if (check.RequestExe(this.Students, out OutStudents))
                UniversityIOConsoleManager.PrintStudents(OutStudents);
            // Сообщение об ошибке
            return true;
        }
    }
}