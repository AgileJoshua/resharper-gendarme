using System;
using System.IO;



namespace First.New_Namespace
{
//    public delegate void MouseUpCallback(int x, int y, MouseButtons buttons);
//
//    public class MouseController
//    {
//        private MouseUpCallback mouse_up_callback;
//
//        public void RaiseMouseUp(Message msg)
//        {
//            if (mouse_up_callback != null)
//            {
//                mouse_up_callback(msg.X, msg.Y, msg.Buttons);
//            }
//        }
//
//        public void ProcessMessage(Message msg)
//        {
//            switch (msg.Id)
//            {
//                case MessageId.MouseUp:
//                    {
//                        RaiseMouseUp(msg);
//                        break;
//                    }
//                // ... more ...
//                default:
//                    break;
//            }
//        }
//    }

//    public class DoesNotOverloadOperatorEquals
//    {
//        public static int operator +(DoesNotOverloadOperatorEquals a)
//        {
//            return 0;
//        }
//
//        public static int operator -(DoesNotOverloadOperatorEquals a)
//        {
//            return 0;
//        }
//    }
//
//    struct OverridesEquals
//    {
//        public override bool Equals(object obj)
//        {
//            return base.Equals(obj);
//        }
//    }

//    class DoesNotImplementAlternative
//    {
//        public static int operator - (DoesNotImplementAlternative a, DoesNotImplementAlternative b)
//        {
//            return 0;
//        }
//
//        public void Add(int i, int y)
//        {
//            
//        }
//    }

//    namespace MyStuff.Internal
//    {
//        public class Helper 
//        {
//        }
//    }

//    public enum SmallEnum : ushort 
//    {
//        Zero,
//        One
//    }

//    public class MissingGetHashCode
//    {
//        public override bool Equals(object obj)
//        {
//            return this == obj;
//        }
//    }
//
//    // 
//    public class MissingEquals 
//    {
//        public override int GetHashCode()
//        {
//            return 42;
//        }
//    }

//    public enum MyEnum
//    {
//        Yes,
//        No,
//        reserved
//    }
    
//    public class My_Custom_Class
//    {
//
//        public int My_Field;
//
//        public void My_Method(string my_string)
//        {
//        }
//    }

//    namespace Foo.Lang.Compiler
//    {
//        class DoesNotOverrideEquals : IDisposable
//        {
//            public virtual void Dispose()
//            {
//                throw new NotImplementedException();
//            }
//
//            public void Dispose (bool i, float f)
//            {
//                
//            }
//
//            public virtual void Dispose(bool i)
//            {
//
//            }
//
//        }

//        public class  CompilerContext
//        {
//        }
//
//        public class Context
//        {
//            
//        }
//
//        public class MainClass
//        {
//            public void Main ()
//            {
//            }
//        }
//    }

//    public interface IDo
//    {
//        int Print(string me);
//    }
//
//    public class Do : IDo
//    {
//        public int Print (string notMe)
//        {
//            return 1;
//        }
//    }

//    class PostOffice
//    {
//        public void SendLetter(Letter letter)
//        {
//        }
//        public void SendPackage(Package package)
//        {
//        }
//
//        public static bool IsPackageValid(Package package)
//        {
//            return package.HasAddress && package.HasStamp;
//        }
//    }

//    public enum SmallEnum : byte
//    {
//        Zero,
//        One
//    }

//    public class GoodProtectedFinalizer
//    {
//        // compiler makes it protected
//        public ~GoodProtectedFinalizer()
//        {
//        }
//    }
//
//    public class MissingGetHashCode
//    {
////        public override bool Equals(object obj)
////        {
////            return this == obj;
////        }
//
//        public override int GetHashCode()
//        {
//            return 1;
//        }
//    }



}

//abstract public class MyClass
//{
//    public MyClass()
//    {
//    }
//}

namespace RGendarme.ManualTests
{
//    public class SecondClasd //: ITestInt
//    {
//        protected int Hello = 1;
//
//        public void Do(int i, string z)
//        {
//            
//        }
//
////        int Another(float f)
////        {
////            
////        }
//    }

//    public interface ITestInt
//    {
//        void Do(int i, string y);
//
//        //int Another(float d);
//    }

//    [AttributeUsage(AttributeTargets.All)]
//    public sealed class AttributeWithRequiredPropertiesAttribute : Attribute
//    {
//        private readonly int _storedFoo;
//        private readonly string _storedBar;
//
//        // we have no corresponding property with the name 'Bar' so the rule will fail
//        public AttributeWithRequiredPropertiesAttribute(int foo, string bar)
//        {
//            _storedFoo = foo;
//            //_storedBar = bar;
//        }
//
//        public string Bar
//        {
//            get
//            {
//                return _storedBar;
//            }
//        }
//
//        public int Foo
//        {
//            get
//            {
//                return _storedFoo;
//            }
//        }
//    }
}