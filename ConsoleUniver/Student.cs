using System;

namespace ConsoleUniver


{
    public class Student
    {
        public static int id = 0;
        public string FIO { get; set; }
        public int StudentId { get; }
        public DateTime Birthday { get; set; }

        private int nFacultet = 0;

        private University university { get; }

        public string Facultet
        {
            get { return university.Facultets[nFacultet - 1]; }
            set { nFacultet = Int32.Parse(value); }
        }

        public string Group { get; set; }

        public int Age
        {
            get
            {
                int age;
                if ((DateTime.Now.Month > Birthday.Month) ||
                    ((DateTime.Now.Month == Birthday.Month) && (DateTime.Now.Day >= Birthday.Day)))
                    age = DateTime.Now.Year - Birthday.Year;
                else
                    age = DateTime.Now.Year - Birthday.Year - 1;
                return age;
            }
        }

        public Student(University university)
        {
            id++;
            this.StudentId = id;
            this.university = university;
        }
    }
}