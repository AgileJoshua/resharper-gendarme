﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;

//namespace One.Two.Three.Four.Five
//{
//    internal class Helper
//    {
//    }

//    public sealed class MyClass
//    {
//        // note that C# compilers won't allow this to compile
//        public int GetAnswer()
//        {
//            return 42;
//        }
//    }

//}

namespace RGendarme.ManualTests
{
//    public sealed class MyClass1
//    {
//        private int someValue;
//    }
//
//    // =======================
//    public sealed class MyClass2
//    {
//        private int GetAnswer()
//        {
//            return 42;
//        }
//    }

    abstract public class AbstractDispose // : IDisposable
    {
        IntPtr field;

        // the field should be disposed in the type that declares it
//        public void Dispose()
//        {
//            
//        }
    }


//    class DoesNotOverloadAdd
//    {
//        public void Foo()
//        {
//            var rules = new Dictionary<string, string>
//	        {
//    	        {"-", "+"},
//    	        {"+", "-"},
//    	        {"+", "/"} // <-- error in runtime!
//	        };
//
//            var rule1 = new Dictionary<int, string>
//	        {
//    	        {1, "+"},
//    	        {2, "-"},
//    	        {1, "/"} // <-- error in runtime!
//	        };
//
//           
//        }
//
//        public bool NextJob (ref int id, string display)
//        {
//            display = String.Format("Job #{0}", id++);
//
//            if (id < 0)
//                return false;
//            
//            return true;
//        }
//
//        public static int operator - (DoesNotOverloadAdd left, DoesNotOverloadAdd right)
//        {
//            return 0;
//        }
//
//        public static int operator + (DoesNotOverloadAdd left, DoesNotOverloadAdd right)
//        {
//            return 0;
//        }
//
////        public static int operator *(DoesNotOverloadAdd left, DoesNotOverloadAdd right)
////        {
////            return 0;
////        }
//
//        public static int operator /(DoesNotOverloadAdd left, DoesNotOverloadAdd right)
//        {
//            return 0;
//        }
//
////        public static int operator >(DoesNotOverloadAdd left, DoesNotOverloadAdd right)
////        {
////            return 0;
////        }
////
////        public static int operator <(DoesNotOverloadAdd left, DoesNotOverloadAdd right)
////        {
////            return 0;
////        }
////
////        public static int operator >=(DoesNotOverloadAdd left, DoesNotOverloadAdd right)
////        {
////            return 0;
////        }
//
////        public static int operator <=(DoesNotOverloadAdd left, DoesNotOverloadAdd right)
////        {
////            return 0;
////        }
//
////        public static bool operator true(DoesNotOverloadAdd ope)
////        {
////            return true;
////        }
//
////        public static bool operator false(DoesNotOverloadAdd ope)
////        {
////            return false;
////        }
//
//    }
//
//    public class CPhone
//    {
//        public string IsConnectionString()
//        {
//            return "hello world";
//        }
//    }
//
//    [AttributeUsage(AttributeTargets.All)]
//    public class MyAttribute : Attribute
//    {
//        
//    }
//
//    public class Call<Mechanism>
//    {
//        // ...
//    }

//    [Serializable]
////    [Flags]
//    enum Options
//    {
//        First = 1,
//        Second = 2,
//        Third = 4,
//        All = First | Second | Third,
//    }
    

//    public delegate void StringListEvent(IStringList sender);
//    public interface IStringList
//    {
//        // void Add(string s);
//        //       int Count { get; }
//        //        event StringListEvent Changed;
//        //         string this[int index] { get; set; }
//    }

//    abstract public class ComPlusSecurity
//    {
//        abstract public void LogIn();
//        abstract public void LogOut();
//    }

//    public class Bad
//    {
//        public event ResolveEventHandler BeforeResolve;
//        public event ResolveEventHandler AfterResolve;
//
//        private double _seed;
//        public double Seed
//        {
//            // no get since there's no use case for it
//            set
//            {
//                _seed = value;
//            }
//
//            get { return _seed; }
//        }
//
//        public bool this[DateTime date, int i1, long l2, string s3, double d1, float f2]
//        {
//            get
//            {
//                return false;
//            }
//        }
//    }
//
//    public class HelloCollection : System.Collections.Stack
//    {
//        public int this[int x, int y]
//        {
//            get
//            {
//                return 0;
//            }
//        }
//    }

//    public enum Answer
//    {
//        AnswerYes,
//        AnswerNo,
//        AnswerMaybe,
//    }

//    [Flags]
//    [Serializable]
//    enum Access
//    {
//        Read = 0,
//        Write = 1
//    }

//    public abstract class ClsAttribute : Attribute
//    {
//
//        public void Dispose()
//        {
//            throw new NotImplementedException();
//        }
//    }

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
