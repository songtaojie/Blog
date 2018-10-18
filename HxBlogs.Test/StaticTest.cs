using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Test
{
    public class StaticTest
    {
        public StaticTest()
        {
            Console.WriteLine("构造方法");
        }
        static StaticTest()
        {
            Console.WriteLine("静态构造方法");
        }
        public static void Hello()
        {
            Console.WriteLine("静态方法");
        }
        public void Hello2()
        {
            Console.WriteLine("静态方法");
        }
    }
}
