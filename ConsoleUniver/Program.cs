using ConsoleUniver;

public class Program
{
    static void Main(string[] args)
    {
        University vatu = new University();
        bool goend;
        do
        {
            goend = false;
            goend |= vatu.StudentsInit();
            vatu.PrintInfo();
            goend |= vatu.RequestStudentField();
        } while (goend);
    }
}