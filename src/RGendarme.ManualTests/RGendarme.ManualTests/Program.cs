using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;

namespace One.Two.Three.Five
{
    internal class Helper
    {
    }
}

namespace RGendarme.ManualTests
{
    public class CPhone
    {
        
    }

    public class Call<Mechanism>
    {
        // ...
    }
    

    public delegate void StringListEvent(IStringList sender);
    public interface IStringList
    {
        // void Add(string s);
        //       int Count { get; }
        //        event StringListEvent Changed;
        //         string this[int index] { get; set; }
    }

    abstract public class ComPlusSecurity
    {
        abstract public void LogIn();
        abstract public void LogOut();
    }

    public class Bad
    {
        public event ResolveEventHandler BeforeResolve;
        public event ResolveEventHandler AfterResolve;

        private double _seed;
        public double Seed
        {
            // no get since there's no use case for it
            set
            {
                _seed = value;
            }

            get { return _seed; }
        }

        public bool this[DateTime date, int i1, long l2, string s3, double d1, float f2]
        {
            get
            {
                return false;
            }
        }
    }

    public class HelloCollection : System.Collections.Stack
    {
        public int this[int x, int y]
        {
            get
            {
                return 0;
            }
        }
    }

    public enum Answer
    {
        AnswerYes,
        AnswerNo,
        AnswerMaybe,
    }

    public abstract class ClsAttribute : Attribute
    {

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    class Program
    {
        public int value = 3;

        private static string field;

        static void Main(string[] args)
        {
            string s = "abc";

            double x = 0.0;
            double y = System.Math.Pow(x, 2.0);
            double z = System.Math.Pow(x, 3.0);
            double n = System.Math.Pow(x, 4.0);

            if (string.IsNullOrEmpty("hello world") == false)
            {

            }
        }
    }
}
