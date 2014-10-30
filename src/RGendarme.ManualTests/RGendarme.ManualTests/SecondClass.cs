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
}