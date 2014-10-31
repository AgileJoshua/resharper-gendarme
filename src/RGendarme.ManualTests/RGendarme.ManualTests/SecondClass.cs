using System;
using System.IO;

namespace RGendarme.ManualTests
{
    public class SecondClasd //: ITestInt
    {
        protected int Hello = 1;

        public void Do(int i, string z)
        {
            
        }

//        int Another(float f)
//        {
//            
//        }
    }

    public interface ITestInt
    {
        void Do(int i, string y);

        //int Another(float d);
    }

    [AttributeUsage(AttributeTargets.All)]
    public sealed class AttributeWithRequiredPropertiesAttribute : Attribute
    {
        private readonly int _storedFoo;
        private readonly string _storedBar;

        // we have no corresponding property with the name 'Bar' so the rule will fail
        public AttributeWithRequiredPropertiesAttribute(int foo, string bar)
        {
            _storedFoo = foo;
            _storedBar = bar;
        }

        public string Bar
        {
            get
            {
                return _storedBar;
            }
        }

//        public int Foo
//        {
//            get
//            {
//                return _storedFoo;
//            }
//        }
    }
}