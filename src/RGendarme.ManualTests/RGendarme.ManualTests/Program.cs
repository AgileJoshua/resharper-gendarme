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

    public class My21Collection : IEnumerable<string>
    {
        
    }


    public class MyCollection : System.Collections.Queue
    {
        
    }

    public class MyStream //: System.IO.Stream
    {
        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead
        {
            get { throw new NotImplementedException(); }
        }

        public override bool CanSeek
        {
            get { throw new NotImplementedException(); }
        }

        public override bool CanWrite
        {
            get { throw new NotImplementedException(); }
        }

        public override long Length
        {
            get { throw new NotImplementedException(); }
        }

        public override long Position { get; set; }
    }

    public class Attribute
    {

    }

    public class MyException : Exception
    {
        
    }

    public abstract class ClsAttribute : System.Attribute
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
