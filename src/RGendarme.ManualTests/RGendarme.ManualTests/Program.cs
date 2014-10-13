using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.Policy;

namespace RGendarme.ManualTests
{
    public delegate void StringListEvent(IStringList sender);
    public interface IStringList
    {
        // void Add(string s);
        //       int Count { get; }
        //        event StringListEvent Changed;
        //         string this[int index] { get; set; }
    }

    public class HelloCollection : System.Collections.Stack
    {
        
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
