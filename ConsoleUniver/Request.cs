using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml.Schema;

namespace ConsoleUniver
{
    public class Request
    {
        public enum enTypeRequest
        {
            requestNodef = 0,
            requestFindMax,
            requestFindMin,
            requestFindEqu,
            requestFindMore,
            requestFindLess,
            requestFindEquStr,
            requestSortMin,
            requestSortMax
        }

        public struct defRequest
        {
            public string Name;
            public List<Type> types;
        }

        public static Dictionary<enTypeRequest, defRequest> RequestIO = new Dictionary<enTypeRequest, defRequest>()
        {
            {
                enTypeRequest.requestNodef,
                new defRequest()
                    {Name = "Отменить поиск", types = new List<Type>() {typeof(int), typeof(string), typeof(DateTime)}}
            },
            {
                enTypeRequest.requestFindMax,
                new defRequest()
                    {Name = "Поиск максимальных значений", types = new List<Type>() {typeof(int)}}
            },
            {
                enTypeRequest.requestFindMin,
                new defRequest()
                    {Name = "Поиск минимальных значений", types = new List<Type>() {typeof(int)}}
            },
            {
                enTypeRequest.requestFindEqu,
                new defRequest()
                    {Name = "Поиск значений равных", types = new List<Type>() {typeof(int)}}
            },
            {
                enTypeRequest.requestFindMore,
                new defRequest()
                    {Name = "Поиск значений больших чем", types = new List<Type>() {typeof(int)}}
            },
            {
                enTypeRequest.requestFindLess,
                new defRequest()
                    {Name = "Поиск значений меньших", types = new List<Type>() {typeof(int)}}
            },
            {
                enTypeRequest.requestFindEquStr,
                new defRequest()
                    {Name = "Поиск совпадающих по строке", types = new List<Type>() {typeof(string)}}
            },
            {
                enTypeRequest.requestSortMin,
                new defRequest()
                {
                    Name = "Сортировка по возрастанию",
                    types = new List<Type>() {typeof(int), typeof(string), typeof(DateTime)}
                }
            },
            {
                enTypeRequest.requestSortMax,
                new defRequest()
                {
                    Name = "Сортировка по убыванию",
                    types = new List<Type>() {typeof(int), typeof(string), typeof(DateTime)}
                }
            },
        };

        public enTypeRequest EnTypeRequest { get; set; }
        public int Value { get; set; }

        public int Count { get; set; }
        public string StrValue { get; set; }

        public MethodInfo Method { get; set; }

        public Request(MethodInfo method)
        {
            if (method == null) return;

            int key;
            Type mtype = method.ReturnType; //.DeclaringType;
            UniversityIOConsoleManager.PrintAndReadTypeRequest(out key, mtype);

            EnTypeRequest = (enTypeRequest) key;
            int value = 0;
            string strvalue = null;
            switch (EnTypeRequest)
            {
                case enTypeRequest.requestFindMax:
                    break;
                case enTypeRequest.requestFindMin:
                    break;
                case enTypeRequest.requestFindEqu:
                    UniversityIOConsoleManager.ReadIntValue(out value);
                    break;
                case enTypeRequest.requestFindMore:
                    UniversityIOConsoleManager.ReadIntValue(out value);
                    break;
                case enTypeRequest.requestFindLess:
                    UniversityIOConsoleManager.ReadIntValue(out value);
                    break;
                case enTypeRequest.requestFindEquStr:
                    UniversityIOConsoleManager.ReadStrValue(out strvalue);
                    break;
            }

            this.Value = value;
            this.StrValue = strvalue;
            this.Method = method;
        }


        public bool RequestExe(List<Student> Elements,
            out List<Student> OutElements)
        {
            OutElements = new List<Student>();
            if (Elements.Count == 0) return false;

            switch (EnTypeRequest)
            {
                case enTypeRequest.requestFindMax:


                    int maxValue = Convert.ToInt32(Method.Invoke(Elements[0], null));
                    foreach (var element in Elements)
                        if (Convert.ToInt32(Method.Invoke(element, null)) >= maxValue)
                            maxValue = Convert.ToInt32(Method.Invoke(element, null));

                    foreach (var element in Elements)
                    {
                        if (Convert.ToInt32(Method.Invoke(element, null)) == maxValue)
                            OutElements.Add(element);
                    }

                    break;

                case enTypeRequest.requestFindMin:

                    int minValue = Convert.ToInt32(Method.Invoke(Elements[0], null));
                    foreach (var element in Elements)
                        if (Convert.ToInt32(Method.Invoke(element, null)) < minValue)
                            minValue = Convert.ToInt32(Method.Invoke(element, null));


                    foreach (var element in Elements)
                    {
                        if (Convert.ToInt32(Method.Invoke(element, null)) == minValue)
                            OutElements.Add(element);
                    }

                    break;
                case enTypeRequest.requestFindEqu:
                    foreach (var m in Elements)
                        if (Method.Invoke(m, null).Equals(Value))
                        {
                            OutElements.Add(m);
                        }

                    break;

                case enTypeRequest.requestFindMore:
                    foreach (var element in Elements)
                    {
                        if (Convert.ToInt32(Method.Invoke(element, null)) >= Value)
                            OutElements.Add(element);
                    }

                    break;
                case enTypeRequest.requestFindLess:
                    foreach (var element in Elements)
                    {
                        if (Convert.ToInt32(Method.Invoke(element, null)) <= Value)
                            OutElements.Add(element);
                    }

                    break;
                case enTypeRequest.requestFindEquStr:
                    foreach (var m in Elements)
                        if (Method.Invoke(m, null).ToString().Contains(StrValue))
                        {
                            OutElements.Add(m);
                        }

                    break;

                case enTypeRequest.requestSortMin:
                case enTypeRequest.requestSortMax:
                {
                    foreach (var t in Elements)
                    {
                        OutElements.Add(t);
                    }

                    ;


                    OutElements.Sort(delegate(Student x, Student y)
                    {
                        if (Method.ReturnType == typeof(int))
                        {
                            int xt = Convert.ToInt32(Method.Invoke(x, null));
                            int yt = Convert.ToInt32(Method.Invoke(y, null));
                            return (xt.CompareTo(yt));
                        }

                        if (Method.ReturnType == typeof(DateTime))
                        {
                            DateTime xt = Convert.ToDateTime(Method.Invoke(x, null));
                            DateTime yt = Convert.ToDateTime(Method.Invoke(y, null));
                            return (xt.CompareTo(yt));
                        }

                        if (Method.ReturnType == typeof(string))
                        {
                            string xt = Convert.ToString(Method.Invoke(x, null));
                            string yt = Convert.ToString(Method.Invoke(y, null));
                            return (xt.CompareTo(yt));
                        }

                        return (x.StudentId.CompareTo(y.StudentId));
                    });


                    if (EnTypeRequest == enTypeRequest.requestSortMax)
                    {
                        OutElements.Reverse();
                    }

                    break;
                }
            }

            return true;
        }
    }
}